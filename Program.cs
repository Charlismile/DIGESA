using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DIGESA.Components;
using DIGESA.Components.Account;
using DIGESA.Data;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using DIGESA.Repositorios.Services;

// using DIGESA.Validadores;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// builder.Services.AddScoped<ISolicitudService, SolicitudService>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddCascadingAuthenticationState();

// builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddBlazoredToast();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContextFactory<DbContextDigesa>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionDigesa"));
});

// RegistroDto del contexto de tu dominio (Pacientes, Médicos, etc.)
builder.Services.AddDbContext<DbContextDigesa>(options =>
    options.UseSqlServer(connectionString));

// RegistroDto de Identity (usa ApplicationUser si lo personalizaste)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Si usas roles, asegúrate de tener esta línea
builder.Services.AddAuthorization();

// RegistroDto de servicios adicionales
builder.Services.AddControllers();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<ICommon, CommonServices>();
// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Crear roles iniciales (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    foreach (var roleName in new[] { "Administrador", "Médico", "Solicitud" })
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Mapea los endpoints de Identity (Razor Pages)
app.MapAdditionalIdentityEndpoints();

app.Run();