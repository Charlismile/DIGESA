namespace DIGESA.Models.CannabisModels.Renovaciones;

public class ReporteRenovacionesViewModel
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }

    public int TotalRenovaciones { get; set; }
    public int RenovacionesExitosas { get; set; }
    public int RenovacionesFallidas { get; set; }
    public int RenovacionesPendientes { get; set; }

    public List<RenovacionMesViewModel> RenovacionesPorMes { get; set; } = new();
    public List<RenovacionDetalleViewModel> Detalles { get; set; } = new();
}