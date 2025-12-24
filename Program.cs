using DIGESA.Components;
using DIGESA.Components.Account;
using DIGESA.Data;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using DIGESA.Repositorios.InterfacesCannabis;
using DIGESA.Repositorios.Services;
using DIGESA.Repositorios.ServiciosCannabis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using IEmailSender = DIGESA.Repositorios.InterfacesCannabis.IEmailSender;

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
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


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
    options.UseSqlServer(defaultConnection));

// Configuración cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Autorización
builder.Services.AddAuthorization();
builder.Services.AddScoped<IDatabaseProvider, DatabaseProviderService>();
builder.Services.AddScoped<IdentityRedirectManager>();


// HttpClient para Active Directory
builder.Services.AddHttpClient<IActiveDirectory, ActiveDirectoryService>();

// Registrar servicios de cannabis
builder.Services.AddScoped<IServicioConfiguracion, ServicioConfiguracion>();
builder.Services.AddScoped<IServicioRenovaciones, ServicioRenovaciones>();
builder.Services.AddScoped<IServicioHistorial, ServicioHistorial>();
builder.Services.AddScoped<IServicioNotificaciones, ServicioNotificaciones>();
builder.Services.AddScoped<IServicioMedicos, ServicioMedicos>();
builder.Services.AddScoped<IServicioFarmacias, ServicioFarmacias>();
builder.Services.AddScoped<IServicioQr, ServicioQr>();
builder.Services.AddScoped<IPaciente, PacienteService>();
builder.Services.AddScoped<ISolicitudCannabisService, SolicitudCannabisService>();
builder.Services.AddScoped<IEmailSender, EmailSenderService>();

// Servicios comunes
builder.Services.AddScoped<IUserData, UserDataService>();
builder.Services.AddScoped<ICommon, CommonServices>();

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