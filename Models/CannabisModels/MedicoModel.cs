using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class MedicoModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El primer nombre es obligatorio.")]
    public string? PrimerNombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio.")]
    public string? PrimerApellido { get; set; }

    [Required(ErrorMessage = "La disciplina es obligatoria.")]
    public MedicoDisciplina MedicoDisciplina { get; set; }

    [Required(ErrorMessage = "El número de idoneidad es obligatorio.")]
    public string? MedicoIdoneidad { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public string? TelefonoMovil { get; set; } // Nombre consistente
    
    public string? DetalleEspecialidad { get; set; } // Renombrado para mayor claridad
    
    public int? RegionSaludId { get; set; } // Renombrado
    public int? InstalacionSaludId { get; set; } // Renombrado

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MedicoDisciplina == MedicoDisciplina.Especialista && 
            string.IsNullOrWhiteSpace(DetalleEspecialidad))
        {
            yield return new ValidationResult("Especifique la especialidad.",
                new[] { nameof(DetalleEspecialidad) });
        }
    }
}