using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El primer nombre es obligatorio.")]
    public string? PrimerNombre { get; set; }

    public string? SegundoNombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio.")]
    public string? PrimerApellido { get; set; }

    public string? SegundoApellido { get; set; }

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public TipoDocumento TipoDocumento { get; set; }

    // Campo unificado para número de documento
    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    public string? NumeroDocumento { get; set; }

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    public string? Nacionalidad { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    public Sexo Sexo { get; set; }

    [Required(ErrorMessage = "Indique si requiere acompañante.")]
    public RequiereAcompanante RequiereAcompanante { get; set; }

    public MotivoRequerimientoAcompanante? MotivoRequerimientoAcompanante { get; set; }
    public string? TipoDiscapacidad { get; set; }

    [Required(ErrorMessage = "El teléfono personal es obligatorio.")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public string? TelefonoPersonal { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public string? TelefonoLaboral { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido.")]
    public string? CorreoElectronico { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string? DireccionExacta { get; set; }

    // Ubicación geográfica
    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    public int? CorregimientoId { get; set; }

    // Información de salud
    public int? RegionSaludId { get; set; }
    public int? InstalacionSaludId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RequiereAcompanante == RequiereAcompanante.Si)
        {
            if (MotivoRequerimientoAcompanante == null)
            {
                yield return new ValidationResult("Especifique el motivo por el que se requiere acompañante.",
                    new[] { nameof(MotivoRequerimientoAcompanante) });
            }
            else if (MotivoRequerimientoAcompanante == Models.CannabisModels.MotivoRequerimientoAcompanante.PacienteDiscapacidad)
            {
                if (string.IsNullOrWhiteSpace(TipoDiscapacidad))
                    yield return new ValidationResult("Especifique la discapacidad.",
                        new[] { nameof(TipoDiscapacidad) });
            }
        }
    }
}