using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class InstalacionSaludModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(300)]
    public string? Direccion { get; set; }
    
    public int? RegionSaludId { get; set; }
    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    
    [StringLength(50)]
    public string? Telefono { get; set; }
    
    [StringLength(100)]
    public string? TipoInstalacion { get; set; } // "Hospital", "Centro de Salud", "Clínica"
    
    public bool IsActivo { get; set; } = true;
    public string? CodigoMinsa { get; set; }
    
    // Propiedades de navegación
    public RegionSaludModel? RegionSalud { get; set; }
    public string? RegionNombre => RegionSalud?.Nombre;
}