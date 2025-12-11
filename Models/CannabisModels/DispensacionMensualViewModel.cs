namespace DIGESA.Models.CannabisModels;

public class DispensacionMensualViewModel
{
    public string Mes { get; set; }
    public int CantidadDispensaciones { get; set; }
    public int PacientesAtendidos { get; set; }
    public decimal CantidadProducto { get; set; }
}