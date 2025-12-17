namespace DIGESA.Models.CannabisModels;

public class DispensacionDiariaViewModel
{
    public DateTime Fecha { get; set; }
    public int CantidadDispensaciones { get; set; }
    public decimal CantidadTotal { get; set; }
    public int PacientesUnicos { get; set; }
}