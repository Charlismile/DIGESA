namespace DIGESA.Models.CannabisModels;

public class ReporteRenovacionesViewModel
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    
    // Estadísticas
    public int TotalRenovaciones { get; set; }
    public int RenovacionesExitosas { get; set; }
    public int RenovacionesFallidas { get; set; }
    public int RenovacionesPendientes { get; set; }
    
    // Por mes
    public List<RenovacionMesViewModel> RenovacionesPorMes { get; set; } = new();
    
    // Detalles
    public List<RenovacionDetalleViewModel> Detalles { get; set; } = new();
    
    // Por provincia
    public List<RenovacionProvinciaViewModel> RenovacionesPorProvincia { get; set; } = new();
}