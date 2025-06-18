namespace DIGESA.Models.DTOs;

public class PacienteSeguimientoDTO
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public DateTime? FechaRegistro { get; set; }
    public string EstadoSolicitud { get; set; } = string.Empty;
    public bool Aprobado { get; set; }
}