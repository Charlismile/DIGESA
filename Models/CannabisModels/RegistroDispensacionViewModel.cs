using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class RegistroDispensacionViewModel
{
    public int Id { get; set; }
    
    [Required]
    public int SolicitudId { get; set; }
    
    [Required]
    public int FarmaciaId { get; set; }
    
    [Required]
    public DateTime FechaDispensacion { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Producto { get; set; }
    
    [Required]
    [Range(0.01, 1000, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public decimal Cantidad { get; set; }
    
    [Required]
    [StringLength(50)]
    public string UnidadMedida { get; set; }
    
    [StringLength(100)]
    public string LoteProducto { get; set; }
    
    public DateTime? FechaVencimientoProducto { get; set; }
    
    [StringLength(200)]
    public string FarmaceuticoResponsable { get; set; }
    
    [StringLength(100)]
    public string NumeroFactura { get; set; }
    
    [StringLength(500)]
    public string Comentarios { get; set; }
    
    [StringLength(450)]
    public string UsuarioRegistro { get; set; }
    
    public DateTime FechaRegistro { get; set; }
    
    // Propiedades de navegación
    public SolicitudCannabisViewModel Solicitud { get; set; }
    public FarmaciaAutorizadaViewModel Farmacia { get; set; }
    
    // Propiedades calculadas
    public string NombrePaciente => Solicitud?.Paciente?.NombreCompleto;
    public string NombreFarmacia => Farmacia?.NombreFarmacia;
    public string ProductoCantidad => $"{Cantidad} {UnidadMedida} de {Producto}";
    
    // Validaciones
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaVencimientoProducto.HasValue && FechaVencimientoProducto.Value < DateTime.Now)
        {
            yield return new ValidationResult(
                "El producto está vencido",
                new[] { nameof(FechaVencimientoProducto) });
        }
        
        if (FechaDispensacion > DateTime.Now.AddDays(1))
        {
            yield return new ValidationResult(
                "La fecha de dispensación no puede ser futura",
                new[] { nameof(FechaDispensacion) });
        }
    }
}