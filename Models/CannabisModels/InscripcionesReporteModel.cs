namespace DIGESA.Models.CannabisModels;

public class InscripcionesReporteModel
{
    public DateTime FechaGeneracion { get; set; }
    public int PrimerasInscripciones { get; set; }
    public int Renovaciones { get; set; }
    public decimal PorcentajeRenovaciones { get; set; }
    public Dictionary<string, int> PorMes { get; set; } = new();
    public List<RegionInscripcionModel> PorRegion { get; set; } = new();
}