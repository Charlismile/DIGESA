namespace DIGESA.Models.CannabisModels;

public class EstadisticasPorRegion
{
    public string Region { get; set; } = string.Empty;
    public int TotalSolicitudes { get; set; }
    public int Aprobadas { get; set; }
    public int Pendientes { get; set; }
}