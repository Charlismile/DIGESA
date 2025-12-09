namespace DIGESA.Models.CannabisModels;

public class HistoricoInscripcionesModel
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int TotalInscripciones { get; set; }
    public int TotalRenovaciones { get; set; }
    public Dictionary<string, int> InscripcionesPorMes { get; set; } = new();
    public Dictionary<string, int> RenovacionesPorMes { get; set; } = new();
    public List<RegionHistoricoModel> DistribucionPorRegion { get; set; } = new();
}