namespace DIGESA.Models.CannabisModels;

public class RegionHistoricoModel
{
    public string Region { get; set; } = string.Empty;
    public int Inscripciones { get; set; }
    public int Renovaciones { get; set; }
    public decimal Porcentaje { get; set; }
}