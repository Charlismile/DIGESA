namespace DIGESA.Models.CannabisModels;

public class PacienteEstadoModel
{
    public string Documento { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public DateTime? FechaVencimiento { get; set; }
    public bool Activo { get; set; }
    public string EstadoSolicitud { get; set; } = string.Empty; // Agregado
    public DateTime? FechaAprobacion { get; set; } // Agregado
    
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
}