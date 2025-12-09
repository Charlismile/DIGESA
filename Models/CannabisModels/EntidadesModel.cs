using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? PrimerNombre { get; set; }

    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? SegundoNombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? PrimerApellido { get; set; }

    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? SegundoApellido { get; set; }

    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    public TipoDocumento TipoDocumento { get; set; }

    [Required(ErrorMessage = "El número de documento es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? NumeroDocumento { get; set; }

    [Required(ErrorMessage = "La nacionalidad es obligatoria")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? Nacionalidad { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio")]
    public Sexo Sexo { get; set; }

    [Required(ErrorMessage = "Indique si requiere acompañante")]
    public RequiereAcompanante RequiereAcompanante { get; set; }

    public MotivoRequerimientoAcompananteE? MotivoRequerimientoAcompanante { get; set; }
    
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
    public string? TipoDiscapacidad { get; set; }

    [Required(ErrorMessage = "El teléfono personal es obligatorio")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido")]
    public string? TelefonoPersonal { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido")]
    public string? TelefonoLaboral { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? CorreoElectronico { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string? DireccionExacta { get; set; }

    // Ubicación geográfica
    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    public int? CorregimientoId { get; set; }

    // Información de salud
    public int? RegionSaludId { get; set; }
    public int? InstalacionSaludId { get; set; }

    // Instalación personalizada
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? InstalacionSaludPersonalizada { get; set; }

    // Propiedades de solo lectura
    public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}".Trim();
    public int? Edad => FechaNacimiento.HasValue ? 
        DateTime.Now.Year - FechaNacimiento.Value.Year : null;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (RequiereAcompanante == RequiereAcompanante.Si)
        {
            if (MotivoRequerimientoAcompanante == null)
            {
                yield return new ValidationResult("Especifique el motivo por el que se requiere acompañante",
                    new[] { nameof(MotivoRequerimientoAcompanante) });
            }
            else if (MotivoRequerimientoAcompanante == MotivoRequerimientoAcompananteE.PacienteDiscapacidad)
            {
                if (string.IsNullOrWhiteSpace(TipoDiscapacidad))
                    yield return new ValidationResult("Especifique la discapacidad",
                        new[] { nameof(TipoDiscapacidad) });
            }
        }

        if (FechaNacimiento.HasValue && FechaNacimiento.Value > DateTime.Now)
        {
            yield return new ValidationResult("La fecha de nacimiento no puede ser futura",
                new[] { nameof(FechaNacimiento) });
        }
    }
}

public class AcompananteModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? PrimerNombre { get; set; }

    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? SegundoNombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? PrimerApellido { get; set; }

    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? SegundoApellido { get; set; }

    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    public TipoDocumento TipoDocumento { get; set; }

    [Required(ErrorMessage = "El número de documento es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? NumeroDocumento { get; set; }

    [Required(ErrorMessage = "La nacionalidad es obligatoria")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? Nacionalidad { get; set; }
    
    [Required(ErrorMessage = "El parentesco es obligatorio")]
    public Parentesco Parentesco { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido")]
    [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
    public string? TelefonoMovil { get; set; }

    public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}".Trim();
}

public class MedicoModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? PrimerNombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? PrimerApellido { get; set; }

    [Required(ErrorMessage = "La disciplina es obligatoria")]
    public MedicoDisciplina MedicoDisciplina { get; set; }

    [Required(ErrorMessage = "El número de idoneidad es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? MedicoIdoneidad { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido")]
    [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
    public string? MedicoTelefono { get; set; }
    
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string? DetalleMedico { get; set; }
    
    public int? RegionSaludId { get; set; }
    public int? InstalacionSaludId { get; set; }

    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? InstalacionSaludPersonalizada { get; set; }

    public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}".Trim();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MedicoDisciplina == MedicoDisciplina.Especialista && 
            string.IsNullOrWhiteSpace(DetalleMedico))
        {
            yield return new ValidationResult("Especifique la especialidad",
                new[] { nameof(DetalleMedico) });
        }
    }
}

public class DiagnosticoModel
{
    public int? Id { get; set; }
    
    [Required(ErrorMessage = "Seleccione al menos un diagnóstico")]
    public List<int> DiagnosticosIds { get; set; } = new();
    
    public bool IncluyeOtroDiagnostico { get; set; }
    
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? DiagnosticoPersonalizado { get; set; }
    
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string? DetalleTratamiento { get; set; }
    
    public DateTime? FechaDiagnostico { get; set; }
}

public class ComorbilidadModel
{
    public int? Id { get; set; }
    
    public bool TieneComorbilidad { get; set; }
    
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? NombreComorbilidad { get; set; }
    
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string? DetalleTratamientoComorbilidad { get; set; }
    
    public DateTime? FechaDiagnosticoComorbilidad { get; set; }
}

public class ProductoPacienteModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre comercial es obligatorio")]
    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? NombreComercial { get; set; }

    public int? FormaFarmaceuticaId { get; set; }
    
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
    public string? FormaFarmaceuticaPersonalizada { get; set; }

    public int? ViaAdministracionId { get; set; }
    
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? ViaAdministracionPersonalizada { get; set; }

    [Required(ErrorMessage = "El tipo de concentración es obligatorio")]
    public TipoConcentracion TipoConcentracion { get; set; }

    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? ConcentracionPersonalizada { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public decimal? CantidadConcentracion { get; set; }

    public int? UnidadId { get; set; }

    [Required(ErrorMessage = "El detalle de la dosis es obligatorio")]
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string? DetalleDosis { get; set; }

    [Required(ErrorMessage = "La duración es obligatoria")]
    [Range(1, 36, ErrorMessage = "Indique un valor entre 1 y 36 meses")]
    public int? DuracionTratamiento { get; set; }

    [Required(ErrorMessage = "La frecuencia es obligatoria")]
    [Range(1, 365, ErrorMessage = "Indique un valor entre 1 y 365 días")]
    public int? FrecuenciaTratamiento { get; set; }

    public bool UsaDosisRescate { get; set; }
    
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string? DetalleDosisRescate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TipoConcentracion == TipoConcentracion.OTRO && 
            string.IsNullOrWhiteSpace(ConcentracionPersonalizada))
        {
            yield return new ValidationResult("Especifique la concentración personalizada",
                new[] { nameof(ConcentracionPersonalizada) });
        }

        if (UsaDosisRescate && string.IsNullOrWhiteSpace(DetalleDosisRescate))
        {
            yield return new ValidationResult("Especifique los detalles de la dosis de rescate",
                new[] { nameof(DetalleDosisRescate) });
        }
    }
}
