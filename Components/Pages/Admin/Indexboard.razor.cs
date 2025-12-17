using DIGESA.Data;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Components.Pages.Admin;

public partial class Indexboard : ComponentBase
{
    private bool isLoading = true;
    private DynamicModal ModalForm = default!;
    private ApplicationUser LoggedUser = new();

    private List<AccionPanel> PanelAcciones { get; set; } = new();
    private List<EstadoPanel> PanelEstados { get; set; } = new();

    [Inject] private ISolicitudService SolicitudService { get; set; } = default!;
    [Inject] private IUserData UserService { get; set; } = default!;
    [Inject] private IDbContextFactory<DbContextDigesa> ContextFactory { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        isLoading = true;
        InicializarAcciones();

        // Obtener usuario autenticado
        var auth = await AuthStateProvider.GetAuthenticationStateAsync();
        var username = auth.User.Identity?.Name;
        if (!string.IsNullOrEmpty(username))
        {
            LoggedUser = await UserService.GetUser(username);
        }

        // Cargar estados desde base de datos
        await using var db = await ContextFactory.CreateDbContextAsync();
        var estadosDb = await db.TbEstadoSolicitud
            .Where(e => e.NombreEstado != "Archivada") // CORREGIDO: Cambié Descripcion por NombreEstado
            .ToListAsync();

        PanelEstados = estadosDb.Select(CrearEstadoPanel).ToList();

        // Cargar conteos por estado
        var counts = await SolicitudService.ObtenerConteoPorEstadoAsync();
        foreach (var panel in PanelEstados)
        {
            panel.Cantidad = counts.TryGetValue(panel.Titulo, out var c) ? c : 0;
        }

        // Mostrar mensaje si la contraseña fue cambiada
        var uri = new Uri(NavigationManager.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        if (query.Get("passwordChanged") == "true")
        {
            ModalForm.ShowSuccess("Tu contraseña ha sido cambiada con éxito.");
            NavigationManager.NavigateTo(uri.GetLeftPart(UriPartial.Path), forceLoad: false);
        }

        isLoading = false;
    }

    private void InicializarAcciones()
    {
        PanelAcciones = new List<AccionPanel>
        {
            new() { Titulo="Registrar Paciente", Icono="fa-user-plus", Color="primary", Borde="#0d6efd", Route="/registro-paciente" },
            new() { Titulo="Ver Solicitudes", Icono="fa-file-invoice", Color="success", Borde="#198754", Route="/admin/solicitudes_registros" },
        };
    }

    private class AccionPanel
    {
        public string Titulo { get; set; } = "";
        public string Icono { get; set; } = "";
        public string Color { get; set; } = "";
        public string Borde { get; set; } = "";
        public string Route { get; set; } = "";
    }

    private class EstadoPanel
    {
        public byte Clave { get; init; }
        public string Titulo { get; init; } = "";
        public string Subtitulo { get; init; } = "";
        public string Icono { get; init; } = "";
        public string Color { get; init; } = "";
        public string Fondo { get; init; } = "";
        public string Borde { get; init; } = "";
        public int Cantidad { get; set; }
    }

    private static readonly Dictionary<byte, (string Subtitulo, string Icono, string Color, string Fondo, string Borde)> EstadoConfig
        = new()
        {
            { 1, ("Pendientes", "fa-paper-plane", "info", "rgba(13,202,240,0.05)", "#0dcaf0") },
            { 2, ("Aprobadas", "fa-check-circle", "success", "rgba(25,135,84,0.05)", "#198754") },
            { 3, ("Rechazadas", "fa-times-circle", "danger", "rgba(220,53,69,0.05)", "#dc3545") },
            { 4, ("En Revisión", "fa-search", "warning", "rgba(255,193,7,0.05)", "#ffc107") }
        };

    private EstadoPanel CrearEstadoPanel(TbEstadoSolicitud estado) // CORREGIDO: Cambié el parámetro de 'e' a 'estado'
    {
        // Usar el IdEstado como clave en lugar de una propiedad 'Estado' que no existe
        var clave = (byte)estado.IdEstado;
        
        return EstadoConfig.TryGetValue(clave, out var cfg)
            ? new EstadoPanel
            {
                Clave = clave,
                Titulo = estado.NombreEstado, // CORREGIDO: Cambié Descripcion por NombreEstado
                Subtitulo = cfg.Subtitulo,
                Icono = cfg.Icono,
                Color = cfg.Color,
                Fondo = cfg.Fondo,
                Borde = cfg.Borde
            }
            : new EstadoPanel
            {
                Clave = clave,
                Titulo = estado.NombreEstado, // CORREGIDO: Cambié Descripcion por NombreEstado
                Subtitulo = "Solicitudes",
                Icono = "fa-question-circle",
                Color = "secondary",
                Fondo = "rgba(108,117,125,0.05)",
                Borde = "#6c757d"
            };
    }

    private void IrA(string route)
    {
        if (!string.IsNullOrWhiteSpace(route))
            NavigationManager.NavigateTo(route);
    }

    private void IrAPanelEstado(byte estado)
    {
        NavigationManager.NavigateTo($"/Admin/Solicitudes", forceLoad: true);
    }
}