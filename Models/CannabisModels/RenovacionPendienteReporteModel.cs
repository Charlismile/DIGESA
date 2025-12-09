namespace DIGESA.Models.CannabisModels;

public class RenovacionPendienteReporteModel
{
    public int SolicitudId { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public DateTime FechaSolicitudRenovacion { get; set; }
    public int DiasDesdeVencimiento { get; set; }
    public DateTime? FechaVencimientoOriginal { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Comentario { get; set; } = string.Empty;
}