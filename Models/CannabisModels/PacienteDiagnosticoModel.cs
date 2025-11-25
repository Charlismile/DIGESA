using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteDiagnosticoModel : IValidatableObject
{
    public bool TieneComorbilidades { get; set; }
    
    [Required(ErrorMessage = "Seleccione al menos un diagnóstico.")]
    [MinLength(1, ErrorMessage = "Seleccione al menos un diagnóstico.")]
    public List<int> DiagnosticosIds { get; set; } = new(); // Renombrado
    
    public string? DiagnosticoPersonalizado { get; set; } // Renombrado
    public string? DetalleTratamiento { get; set; }
    public DateTime? FechaDiagnostico { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DiagnosticosIds.Count == 0 && string.IsNullOrWhiteSpace(DiagnosticoPersonalizado))
        {
            yield return new ValidationResult("Seleccione al menos un diagnóstico o especifique uno personalizado.",
                new[] { nameof(DiagnosticosIds) });
        }

        if (!string.IsNullOrWhiteSpace(DiagnosticoPersonalizado) && DiagnosticosIds.Count == 0)
        {
            // Validación adicional para diagnóstico personalizado
        }
    }
}