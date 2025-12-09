using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ViaAdministracionModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? Descripcion { get; set; }
    
    public bool IsActivo { get; set; } = true;
    public string? Codigo { get; set; }
    public bool RequiereFormaFarmaceuticaEspecifica { get; set; } = false;
    
    // Vías de administración predefinidas
    public static List<ViaAdministracionModel> ViasPredefinidas()
    {
        return new List<ViaAdministracionModel>
        {
            new() { 
                Id = 1, 
                Nombre = "Oral", 
                Descripcion = "Administración por vía oral (tragar)",
                Codigo = "ORAL"
            },
            new() { 
                Id = 2, 
                Nombre = "Sublingual", 
                Descripcion = "Administración debajo de la lengua",
                Codigo = "SUBLING"
            },
            new() { 
                Id = 3, 
                Nombre = "Tópica", 
                Descripcion = "Aplicación sobre la piel",
                Codigo = "TOPICO"
            },
            new() { 
                Id = 4, 
                Nombre = "Inhalación", 
                Descripcion = "Inhalación (vaporización)",
                Codigo = "INHAL"
            },
            new() { 
                Id = 5, 
                Nombre = "Rectal", 
                Descripcion = "Administración por vía rectal",
                Codigo = "RECTAL"
            },
            new() { 
                Id = 6, 
                Nombre = "Transdérmica", 
                Descripcion = "A través de la piel (parches)",
                Codigo = "TRANSD"
            }
        };
    }
}