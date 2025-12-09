namespace DIGESA.Models.CannabisModels;

public class PacienteActivoInactivoModel
{
    public int Id { get; set; }
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public string PacienteCorreo { get; set; } = string.Empty;
    public string PacienteTelefono { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string NumeroCarnet { get; set; } = string.Empty;
    public DateTime FechaEmisionCarnet { get; set; }
    public DateTime FechaVencimientoCarnet { get; set; }
    public string Estado { get; set; } = string.Empty; // "Activo", "Inactivo", "Vencido"
    public int DiasDesdeVencimiento { get; set; }
    public DateTime? FechaUltimaRenovacion { get; set; }
    public int SolicitudId { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
}