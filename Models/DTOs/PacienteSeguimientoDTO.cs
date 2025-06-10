namespace DIGESA.Models.DTOs;

public class PacienteSeguimientoDTO
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = "";
    public string NumeroDocumento { get; set; } = "";
    public string EstadoSolicitud { get; set; } = "Pendiente";
    public DateTime FechaRegistro { get; set; }
    public bool Aprobado { get; set; }
}