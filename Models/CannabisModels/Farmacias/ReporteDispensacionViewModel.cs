namespace DIGESA.Models.CannabisModels.Farmacias;

public class ReporteDispensacionViewModel
{
    public int FarmaciaId { get; set; }
    public string? NombreFarmacia { get; set; }

    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaGeneracion { get; set; }

    public int TotalDispensaciones { get; set; }
    public int TotalPacientesAtendidos { get; set; }
    public decimal CantidadTotalProducto { get; set; }

    public List<ProductoDispensacionViewModel> Productos { get; set; } = new();
    public List<RegistroDispensacionViewModel> Detalles { get; set; } = new();
}