using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class UnidadViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre de la unidad es requerido")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
    public string NombreUnidad { get; set; }
    
    public bool IsActivo { get; set; } = true;
    
    // Validación personalizada
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var unidadesPermitidas = new[] { "mg", "ml", "g", "gotas", "unidades" };
        
        if (!unidadesPermitidas.Contains(NombreUnidad.ToLower()))
        {
            yield return new ValidationResult(
                $"La unidad '{NombreUnidad}' no es válida. Unidades permitidas: {string.Join(", ", unidadesPermitidas)}",
                new[] { nameof(NombreUnidad) });
        }
    }
}