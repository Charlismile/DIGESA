namespace DIGESA.Models.CannabisModels;

public class ProductoDispensacionViewModel
{
    public string Producto { get; set; }
    public int CantidadDispensaciones { get; set; }
    public decimal CantidadTotal { get; set; }
    public string UnidadMedida { get; set; }
    public decimal Porcentaje { get; set; }
}