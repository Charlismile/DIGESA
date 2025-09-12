using Blazored.Toast;
using DIGESA.Components;
using DIGESA.Components.Account;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DIGESA.Data;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Repositorios.Interfaces;
using DIGESA.Repositorios.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorBootstrap();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredToast();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContextFactory<DbContextDigesa>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionDigesa"));
});

// ✅ Identity para usuarios externos
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ✅ Autenticación híbrida (Identity + Active Directory local)
builder.Services.AddAuthentication(options =>
{
    // Identity por defecto
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    // Pero si el usuario está en AD, usar Negotiate
    options.DefaultChallengeScheme = NegotiateDefaults.AuthenticationScheme;
})
.AddNegotiate()            // Usuarios internos (AD local)
.AddIdentityCookies();     // Usuarios externos (Identity)

// ✅ Autorización
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

// Servicios propios
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<ICommon, CommonServices>();
builder.Services.AddScoped<IPaciente, PacienteService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader();
    });
});

// Configuración de tu servicio externo de AD (si lo necesitas todavía)
builder.Services.Configure<ActiveDirectoryApiModel>(builder.Configuration.GetSection("API_INFO"));
builder.Services.AddHttpClient<IActiveDirectory, ActiveDirectoryService>(client =>
{
    var cfg = builder.Configuration.GetSection("API_INFO").Get<ActiveDirectoryApiModel>();
    client.BaseAddress = new Uri(cfg.BaseUrl);
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Crear roles iniciales
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var roleName in new[] { "Administrador", "Médico", "Solicitud", "Externo" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();
