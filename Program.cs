using Blazored.Toast;
using DIGESA.Components;
using DIGESA.Components.Account;
using DIGESA.Data;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using DIGESA.Repositorios.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IDatabaseProvider = DIGESA.Repositorios.Interfaces.IDatabaseProvider;

var builder = WebApplication.CreateBuilder(args);

// ==========================
// Logging
// ==========================
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// ==========================
// Configuración de servicios
// ==========================
builder.Services.AddControllers();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorBootstrap();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddBlazoredToast();

// ==========================
// Bases de datos
// ==========================
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection") 
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(defaultConnection));

builder.Services.AddDbContextFactory<DbContextDigesa>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionDigesa"));
});

// ==========================
// Identity (usuarios externos)
// ==========================
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // Desactivamos confirmación de email por ahora
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ==========================
// Configuración de cookies de autenticación
// ==========================
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    // Evitar loop en login
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/Account/Login"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        }

        context.Response.Redirect(context.RedirectUri);
        return Task.CompletedTask;
    };
});

// ==========================
// Autorización
// ==========================
builder.Services.AddAuthorization(options =>
{
    // No obliga login a todas las páginas por defecto
    options.FallbackPolicy = null;
});


// ==========================
// Servicios propios
// ==========================
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<ICommon, CommonServices>();
builder.Services.AddScoped<IPaciente, PacienteService>();
builder.Services.AddScoped<IDatabaseProvider, DatabaseProviderService>();
builder.Services.AddScoped<IUserData, UserDataService>();
builder.Services.AddScoped<IdentityRedirectManager>();


// ==========================
// CORS
// ==========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ==========================
// Compresión de respuesta
// ==========================
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true; // Activa la compresión en HTTPS
});

// ==========================
// Configuración Active Directory API
// ==========================
builder.Services.Configure<ActiveDirectoryApiModel>(builder.Configuration.GetSection("API_INFO"));
builder.Services.AddHttpClient<IActiveDirectory, ActiveDirectoryService>(client =>
{
    var cfg = builder.Configuration.GetSection("API_INFO").Get<ActiveDirectoryApiModel>();
    client.BaseAddress = new Uri(cfg.BaseUrl);
});

// ==========================
// Swagger / API Explorer
// ==========================
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// ==========================
// Crear roles y usuario admin en desarrollo
// ==========================
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    foreach (var roleName in new[] { "Administrador", "Médico", "Solicitud", "Externo" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminEmail = "admin@minsa.gob.pa";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var user = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Admin123*"); // contraseña temporal
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Administrador");
        }
    }
}

// ==========================
// Configure the HTTP request pipeline
// ==========================
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseResponseCompression(); // ✅ ahora ya funciona

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Endpoints de Identity /Account
app.MapAdditionalIdentityEndpoints();

app.Run();
