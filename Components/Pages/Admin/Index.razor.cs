using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using DIGESA.Repositorios.Services;

namespace DIGESA.Components.Pages.Admin;

public partial class Index : ComponentBase
{
    [Inject] 
    private ISolicitudService SolicitudService { get; set; } = default!;

    private DynamicModal ModalForm = default!;
    private ApplicationUser LoggedUser = new();
    private List<AccionPanel> PanelAcciones { get; set; } = new();
    private List<EstadoPanel> PanelEstados { get; set; } = new();

    // Configuración de panel de acciones
    private void InicializarAcciones()
    {
        PanelAcciones = new List<AccionPanel>
        {
            new() { Titulo="Registrar Paciente", Icono="fa-user-plus", Color="primary", Borde="#0d6efd", Route="/registropaciente" },
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
        EstadoConfig.TryGetValue((byte)e.Estado, out var cfg) // 👈 conversión explícita
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
        InicializarAcciones();

        var auth = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var uid = auth.User.FindFirst(c => c.Type.EndsWith("nameidentifier"))?.Value;

        if (string.IsNullOrEmpty(uid))
            return;

        LoggedUser = await _UserService.GetUser(auth.User.Identity?.Name ?? "");

        await using var db = await ContextFactory.CreateDbContextAsync();

        var estadosDb = await db.TbEstadoSolicitud
            .Where(e => e.Descripcion != "Archivada")
            .ToListAsync();

        PanelEstados = estadosDb.Select(CrearEstadoPanel).ToList();

        // --- Conteo desde SolicitudService
        var counts = await SolicitudService.ObtenerConteoPorEstadoAsync();

        foreach (var panel in PanelEstados)
        {
            if (counts.TryGetValue(panel.Titulo, out int cantidad))
                panel.Cantidad = cantidad;
            else
                panel.Cantidad = 0;
        }


        // --- Modal si cambió contraseña
        var uri = Navigation.ToAbsoluteUri(Navigation.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        if (query.Get("passwordChanged") == "true")
        {
            ModalForm.ShowSuccess("Tu contraseña ha sido cambiada con éxito.");
            Navigation.NavigateTo(uri.GetLeftPart(UriPartial.Path), forceLoad: false);
        }
    }

    private void IrA(string route)
    {
        if (!string.IsNullOrWhiteSpace(route))
            Navigation.NavigateTo(route);
    }

    private void IrAPanelEstado(byte estado)
    {
        Navigation.NavigateTo($"/admin/solicitudes_registros?estado={estado}", forceLoad: true);
    }
}
