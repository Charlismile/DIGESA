namespace DIGESA.Models.CannabisModels;

public class InscripcionDetalleModel
{
    public int Id { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public string Tipo { get; set; } = string.Empty; // "Primera Inscripción" o "Renovación"
    public string Estado { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public string PacienteCorreo { get; set; } = string.Empty;
    public string PacienteTelefono { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Instalacion { get; set; } = string.Empty;
    public DateTime? FechaAprobacion { get; set; }
    public string? NumeroCarnet { get; set; }
    public DateTime? FechaEmisionCarnet { get; set; }
    public DateTime? FechaVencimientoCarnet { get; set; }
    public bool CarnetActivo { get; set; }
    public int DiasParaVencimiento { get; set; }

}