using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class FormaFarmaceuticaModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(300)]
    public string? Descripcion { get; set; }
    
    public bool IsActivo { get; set; } = true;
    public int OrdenVisualizacion { get; set; } = 0;
    public string? Codigo { get; set; }
    
    // Formas predefinidas comunes
    public static List<FormaFarmaceuticaModel> FormasPredefinidas()
    {
        return new List<FormaFarmaceuticaModel>
        {
            new() { Id = 1, Nombre = "Aceite", Descripcion = "Aceite de cannabis medicinal", OrdenVisualizacion = 1 },
            new() { Id = 2, Nombre = "Cápsulas", Descripcion = "Cápsulas orales", OrdenVisualizacion = 2 },
            new() { Id = 3, Nombre = "Crema/Tópico", Descripcion = "Crema de aplicación tópica", OrdenVisualizacion = 3 },
            new() { Id = 4, Nombre = "Spray", Descripcion = "Spray oral o sublingual", OrdenVisualizacion = 4 },
            new() { Id = 5, Nombre = "Flores", Descripcion = "Flores para vaporización", OrdenVisualizacion = 5 },
            new() { Id = 6, Nombre = "Comestible", Descripcion = "Productos comestibles", OrdenVisualizacion = 6 },
            new() { Id = 7, Nombre = "Otro", Descripcion = "Otra forma farmacéutica", OrdenVisualizacion = 7 }
        };
    }
}