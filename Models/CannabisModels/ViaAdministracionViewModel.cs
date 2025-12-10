using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ViaAdministracionViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; }
    
    public bool IsActivo { get; set; } = true;
    
    // Validación personalizada
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var viasPermitidas = new[] 
        { 
            "Oral", "Sublingual", "Tópica", "Inhalación", 
            "Transdérmica", "Rectal", "Nasal" 
        };
        
        if (!viasPermitidas.Contains(Nombre, StringComparer.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                $"La vía de administración '{Nombre}' no es válida",
                new[] { nameof(Nombre) });
        }
    }
}