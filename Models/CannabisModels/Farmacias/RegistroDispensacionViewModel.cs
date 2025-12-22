using DIGESA.Models.CannabisModels.Renovaciones;

namespace DIGESA.Models.CannabisModels.Farmacias;

public class RegistroDispensacionViewModel
{
    public int Id { get; set; }

    public int? SolicitudId { get; set; }
    public int? FarmaciaId { get; set; }

    public DateTime? FechaDispensacion { get; set; }

    public string? Producto { get; set; }
    public decimal Cantidad { get; set; }
    public string? UnidadMedida { get; set; }

    public string? LoteProducto { get; set; }
    public DateTime? FechaVencimientoProducto { get; set; }

    public string? FarmaceuticoResponsable { get; set; }
    public string? NumeroFactura { get; set; }
    public string? Comentarios { get; set; }

    public DateTime? FechaRegistro { get; set; }
    public string? UsuarioRegistro { get; set; }

    // Navegación
    public SolicitudCannabisViewModel? Solicitud { get; set; }
    public FarmaciaAutorizadaViewModel? Farmacia { get; set; }
}