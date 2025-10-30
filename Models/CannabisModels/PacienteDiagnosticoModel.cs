using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteDiagnosticoModel : IValidatableObject // AGREGADO: Implementar validación
{
    public bool IsOtroDiagSelected { get; set; } = false;
    
    [Required(ErrorMessage = "Seleccione al menos un diagnóstico.")]
    [MinLength(1, ErrorMessage = "Seleccione al menos un diagnóstico.")]
    public List<int> SelectedDiagnosticosIds { get; set; } = new();
    
    public string? NombreOtroDiagnostico { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SelectedDiagnosticosIds.Count == 0 && !IsOtroDiagSelected)
        {
            yield return new ValidationResult("Seleccione al menos un diagnóstico o agregue uno nuevo.",
                new[] { nameof(SelectedDiagnosticosIds) });
        }

        if (IsOtroDiagSelected && string.IsNullOrWhiteSpace(NombreOtroDiagnostico))
        {
            yield return new ValidationResult("Especifique el nombre del diagnóstico.",
                new[] { nameof(NombreOtroDiagnostico) });
        }
    }
}