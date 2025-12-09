using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class UnidadModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(10)]
    public string? Simbolo { get; set; }
    
    [StringLength(100)]
    public string? Descripcion { get; set; }
    
    public bool IsActivo { get; set; } = true;
    public string? TipoUnidad { get; set; } // "Volumen", "Peso", "Unidad"
    
    // Unidades predefinidas para productos de cannabis
    public static List<UnidadModel> UnidadesPredefinidas()
    {
        return new List<UnidadModel>
        {
            new() { Id = 1, Nombre = "Miligramos", Simbolo = "mg", TipoUnidad = "Peso" },
            new() { Id = 2, Nombre = "Gramos", Simbolo = "g", TipoUnidad = "Peso" },
            new() { Id = 3, Nombre = "Mililitros", Simbolo = "ml", TipoUnidad = "Volumen" },
            new() { Id = 4, Nombre = "Gotas", Simbolo = "gts", TipoUnidad = "Unidad" },
            new() { Id = 5, Nombre = "Cápsulas", Simbolo = "caps", TipoUnidad = "Unidad" },
            new() { Id = 6, Nombre = "Porcentaje", Simbolo = "%", TipoUnidad = "Concentración" },
            new() { Id = 7, Nombre = "Miligramos por mililitro", Simbolo = "mg/ml", TipoUnidad = "Concentración" },
            new() { Id = 8, Nombre = "Unidades Internacionales", Simbolo = "UI", TipoUnidad = "Concentración" }
        };
    }
}