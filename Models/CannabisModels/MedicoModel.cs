using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum MedicoDisciplina { General, Odontologo, Especialista }
public class MedicoModel : IValidatableObject
{
    [Required(ErrorMessage = "La disciplina es obligatoria.")]
    public MedicoDisciplina? MedicoDisciplinaEnum { get; set; }
    
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string? PrimerNombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string? PrimerApellido { get; set; }
    public string? DetalleMedico { get; set; }

    [Required(ErrorMessage = "El número de idoneidad es obligatorio.")]
    public string? MedicoIdoneidad { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public string? MedicoTelefono { get; set; }
    
    public string? MedicoInstalacion { get; set; }
    
    public int? medicoInstalacionId { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MedicoDisciplinaEnum == MedicoDisciplina.Especialista)
        {
            if (string.IsNullOrWhiteSpace(DetalleMedico))
                yield return new ValidationResult("Especifique la especialidad.",
                    new[] { nameof(DetalleMedico) });
        }
    }
}



