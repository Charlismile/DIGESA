using DIGESA.Components;
using DIGESA.Components.Account;
using DIGESA.Data;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using DIGESA.Repositorios.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Servicios Blazor
builder.Services.AddControllers();
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddBlazorBootstrap();
builder.Services.AddCascadingAuthenticationState();

// ==========================
// Bases de datos
// ==========================
var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Identity DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(defaultConnection));

// DIGESA DB
builder.Services.AddDbContextFactory<DbContextDigesa>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DigesaConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configuración cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Autorización
builder.Services.AddAuthorization();

// ==========================
// Servicios propios
// ==========================
// Registrar servicios en Program.cs
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICommon, CommonServices>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IPaciente, PacienteService>();
builder.Services.AddScoped<ISolicitudService, SolicitudService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITransferenciaService, TransferenciaService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IReporteService, ReporteService>();

// Servicios programados (background services)
// builder.Services.AddHostedService<NotificacionBackgroundService>();
// builder.Services.AddHostedService<InactivacionBackgroundService>();

// HttpClient para Active Directory
builder.Services.AddHttpClient<IActiveDirectory, ActiveDirectoryService>();

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

// Response compression
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

// Swagger / API Explorer
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Pipeline
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
app.UseResponseCompression();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapAdditionalIdentityEndpoints();

app.Run();
