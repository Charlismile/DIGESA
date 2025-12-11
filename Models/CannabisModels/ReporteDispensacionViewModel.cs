namespace DIGESA.Models.CannabisModels;

public class ReporteDispensacionViewModel
{
    public int FarmaciaId { get; set; }
    public string NombreFarmacia { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    
    // Estadísticas
    public int TotalDispensaciones { get; set; }
    public int TotalPacientesAtendidos { get; set; }
    public decimal CantidadTotalProducto { get; set; }
    public decimal ValorTotalEstimado { get; set; }
    
    // Por producto
    public List<ProductoDispensacionViewModel> Productos { get; set; } = new();
    
    // Por mes
    public List<DispensacionMensualViewModel> DispensacionMensual { get; set; } = new();
    
    // Detalles
    public List<RegistroDispensacionViewModel> Detalles { get; set; } = new();
}