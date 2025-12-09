using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ProvinciaModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(10)]
    public string? Codigo { get; set; }
    
    public bool IsActivo { get; set; } = true;
    
    // Propiedades de navegación
    public List<DistritoModel> Distritos { get; set; } = new();
}

public class DistritoModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    public int ProvinciaId { get; set; }
    
    [StringLength(10)]
    public string? Codigo { get; set; }
    
    public bool IsActivo { get; set; } = true;
    
    // Propiedades de navegación
    public ProvinciaModel? Provincia { get; set; }
    public List<CorregimientoModel> Corregimientos { get; set; } = new();
}

public class CorregimientoModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    public int DistritoId { get; set; }
    
    [StringLength(10)]
    public string? Codigo { get; set; }
    
    public bool IsActivo { get; set; } = true;
    
    // Propiedades de navegación
    public DistritoModel? Distrito { get; set; }
    public string ProvinciaNombre => Distrito?.Provincia?.Nombre;
}