using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Models.CannabisModels;

public class HistorialCompletoViewModel
{
    public int PacienteId { get; set; }
    public string NombrePaciente { get; set; }
    public List<SolicitudCannabisViewModel> Solicitudes { get; set; }
    public List<HistorialRenovacionViewModel> Renovaciones { get; set; }
    public List<SolicitudHistorialViewModel> CambiosEstado { get; set; }
    public List<LogNotificacionViewModel> Notificaciones { get; set; }
    public List<EventoHistorialViewModel> LineaTiempo { get; set; }
    public int TotalSolicitudes { get; set; }
    public int TotalRenovaciones { get; set; }
    public TbSolRegCannabis CarnetActual { get; set; }
    
    // Estadísticas
    public int DiasComoPaciente => CalcularDiasComoPaciente();
    public int RenovacionesExitosas => Renovaciones.Count(r => 
        Solicitudes.Any(s => s.Id == r.SolicitudNuevaId && s.CarnetActivo));
    public DateTime? FechaPrimeraSolicitud => Solicitudes.LastOrDefault()?.FechaSolicitud;
    
    private int CalcularDiasComoPaciente()
    {
        if (!FechaPrimeraSolicitud.HasValue) return 0;
        return (int)(DateTime.Now - FechaPrimeraSolicitud.Value).TotalDays;
    }
}