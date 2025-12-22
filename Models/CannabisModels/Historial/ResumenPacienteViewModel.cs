namespace DIGESA.Models.CannabisModels.Historial;

public class ResumenPacienteViewModel
{
    public int PacienteId { get; set; }
    public string NombreCompleto { get; set; }
    public string DocumentoIdentidad { get; set; }
    public DateTime FechaPrimeraSolicitud { get; set; }
    public int TotalSolicitudes { get; set; }
    public int TotalRenovaciones { get; set; }
    public string? CarnetActual { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Estado { get; set; }
}