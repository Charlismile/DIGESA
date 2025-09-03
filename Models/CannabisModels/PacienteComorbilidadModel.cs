using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum TieneComorbilidad { Si, No }
public class PacienteComorbilidadModel : IValidatableObject
{
    public int Id { get; set; }
    public string? NombreDiagnostico { get; set; }
    public string? DetalleTratamiento { get; set; }
    
    public TieneComorbilidad TieneComorbilidadEnum { get ; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TieneComorbilidadEnum == TieneComorbilidad.Si)
        {
            if (string.IsNullOrWhiteSpace(NombreDiagnostico))
                yield return new ValidationResult("Especifique el diagnostico.",
                    new[] { nameof(NombreDiagnostico) });
        }
    }
}