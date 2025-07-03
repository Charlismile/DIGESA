using Blazored.Toast;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DIGESA.Components;
using DIGESA.Components.Account;
using DIGESA.Data;
using DIGESA.Mappings;
using DIGESA.Models.Entities.BDUbicaciones;
using DIGESA.Models.Entities.DBDIGESA;

// using DIGESA.Validadores;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<IDbUbicacionesService, DbUbicacionesService>();
builder.Services.AddBlazorBootstrap();
builder.Services.AddCascadingAuthenticationState();

//implementacion
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IDiagnosticoRepository, DiagnosticoRepository>();
builder.Services.AddScoped<ITratamientoRepository, TratamientoRepository>();
// Agregar AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddBlazoredToast();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//Ubicaciones
builder.Services.AddDbContext<DbUbicacionPanama>(options =>
    options.UseSqlServer("UbicacionConnection"));
// Registro del contexto principal de Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Registro del contexto de tu dominio (Pacientes, Médicos, etc.)
builder.Services.AddDbContext<DbContextDigesa>(options =>
    options.UseSqlServer(connectionString));

// Registro de Identity (usa ApplicationUser si lo personalizaste)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Si usas roles, asegúrate de tener esta línea
builder.Services.AddAuthorization();

// Registro de servicios adicionales
builder.Services.AddControllers();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

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

    foreach (var roleName in new[] { "Administrador", "Médico", "Paciente" })
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