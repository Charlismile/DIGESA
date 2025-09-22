using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Web;
using DIGESA.Data;

namespace DIGESA.Components.Pages.Admin;

public partial class Indexboard : ComponentBase
{
    private bool isLoading = true;

    private DynamicModal ModalForm = default!;
    private ApplicationUser LoggedUser = new();
    private List<AccionPanel> PanelAcciones { get; set; } = new();
    private List<EstadoPanel> PanelEstados { get; set; } = new();

    [Inject] private ISolicitudService SolicitudService { get; set; } = default!;
    [Inject] private IUserData _UserService { get; set; } = default!;
    [Inject] private IDbContextFactory<DbContextDigesa> ContextFactory { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

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
            { 0, ("Pendientes", "fa-paper-plane", "info",    "rgba(13,202,240,0.05)", "#0dcaf0") },
            { 1, ("Procesando", "fa-clock",       "warning", "rgba(255,193,7,0.05)",  "#ffc107") },
            { 2, ("Completadas","fa-check-circle","success", "rgba(25,135,84,0.05)",  "#198754") },
            { 3, ("Denegadas",  "fa-times-circle","danger",  "rgba(220,53,69,0.05)",  "#dc3545") }
        };

    private EstadoPanel CrearEstadoPanel(TbEstadoSolicitud e) =>
        EstadoConfig.TryGetValue((byte)e.Estado, out var cfg)
            ? new EstadoPanel
            {
                Clave = (byte)e.Estado,
                Titulo = e.Descripcion,
                Subtitulo = cfg.Subtitulo,
                Icono = cfg.Icono,
                Color = cfg.Color,
                Fondo = cfg.Fondo,
                Borde = cfg.Borde
            }
            : new EstadoPanel
            {
                Clave = (byte)e.Estado,
                Titulo = e.Descripcion,
                Subtitulo = "",
                Icono = "fa-question-circle",
                Color = "secondary",
                Fondo = "rgba(108,117,125,0.05)",
                Borde = "#6c757d"
            };

    protected override async Task OnParametersSetAsync()
    {
        isLoading = true;

        InicializarAcciones();

        var auth = await AuthStateProvider.GetAuthenticationStateAsync();
        var username = auth.User.Identity?.Name;
        if (!string.IsNullOrEmpty(username))
        {
            LoggedUser = await _UserService.GetUser(username);
        }

        await using var db = await ContextFactory.CreateDbContextAsync();
        var estadosDb = await db.TbEstadoSolicitud
            .Where(e => e.Descripcion != "Archivada")
            .ToListAsync();

        PanelEstados = estadosDb.Select(CrearEstadoPanel).ToList();

        var counts = await SolicitudService.ObtenerConteoPorEstadoAsync();
        foreach (var panel in PanelEstados)
        {
            panel.Cantidad = counts.TryGetValue(panel.Titulo, out var c) ? c : 0;
        }

        // Modal contraseña
        var uri = new Uri(NavigationManager.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        if (query.Get("passwordChanged") == "true")
        {
            ModalForm.ShowSuccess("Tu contraseña ha sido cambiada con éxito.");
            NavigationManager.NavigateTo(uri.GetLeftPart(UriPartial.Path), forceLoad: false);
        }

        isLoading = false;
    }

    private void IrA(string route)
    {
        if (!string.IsNullOrWhiteSpace(route))
            NavigationManager.NavigateTo(route);
    }

    private void IrAPanelEstado(byte estado)
    {
        NavigationManager.NavigateTo($"/admin/solicitudes_registros?estado={estado}", forceLoad: true);
    }
}
