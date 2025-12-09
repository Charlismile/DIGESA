namespace DIGESA.Models.CannabisModels;

public class RegionInscripcionModel
{
    public string Region { get; set; } = string.Empty;
    public int PrimerasInscripciones { get; set; }
    public int Renovaciones { get; set; }
    public int Total { get; set; }
}