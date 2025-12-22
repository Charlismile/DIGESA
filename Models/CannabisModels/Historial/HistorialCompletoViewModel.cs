using DIGESA.Models.CannabisModels.Renovaciones;

namespace DIGESA.Models.CannabisModels.Historial;

public class HistorialCompletoViewModel
{
    public int PacienteId { get; set; }

    public List<SolicitudCannabisViewModel> Solicitudes { get; set; } = new();
    public List<HistorialRenovacionViewModel> Renovaciones { get; set; } = new();
    public List<SolicitudHistorialViewModel> CambiosEstado { get; set; } = new();
    public List<LogNotificacionViewModel> Notificaciones { get; set; } = new();
    public List<EventoHistorialViewModel> LineaTiempo { get; set; } = new();

    public int TotalSolicitudes { get; set; }
    public int TotalRenovaciones { get; set; }
    public SolicitudCannabisViewModel? CarnetActual { get; set; }
}