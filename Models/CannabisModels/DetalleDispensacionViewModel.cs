namespace DIGESA.Models.CannabisModels;

public class DetalleDispensacionViewModel
{
    public int Id { get; set; }
    public DateTime FechaDispensacion { get; set; }
    public string PacienteNombre { get; set; }
    public string PacienteCedula { get; set; }
    public string Producto { get; set; }
    public decimal Cantidad { get; set; }
    public string UnidadMedida { get; set; }
    public string LoteProducto { get; set; }
    public DateOnly? FechaVencimientoProducto { get; set; }
    public string FarmaceuticoResponsable { get; set; }
    public string NumeroFactura { get; set; }
    public string Comentarios { get; set; }
}