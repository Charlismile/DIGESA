using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class MedicoViewModel : IValidatableObject
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El código de médico es requerido")]
    [StringLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
    public string CodigoMedico { get; set; }
    
    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El primer nombre no puede exceder 100 caracteres")]
    public string PrimerNombre { get; set; }
    
    [StringLength(100)]
    public string SegundoNombre { get; set; }
    
    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    [StringLength(100)]
    public string PrimerApellido { get; set; }
    
    [StringLength(100)]
    public string SegundoApellido { get; set; }
    
    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    [StringLength(50)]
    public string TipoDocumento { get; set; }
    
    [Required(ErrorMessage = "El número de documento es obligatorio")]
    [StringLength(100)]
    public string NumeroDocumento { get; set; }
    
    [Required(ErrorMessage = "La especialidad es obligatoria")]
    [StringLength(150)]
    public string Especialidad { get; set; }
    
    [StringLength(150)]
    public string Subespecialidad { get; set; }
    
    [Required(ErrorMessage = "El número de colegiatura es obligatorio")]
    [StringLength(100)]
    public string NumeroColegiatura { get; set; }
    
    [Phone(ErrorMessage = "Formato de teléfono inválido")]
    [StringLength(15)]
    public string TelefonoConsultorio { get; set; }
    
    [Phone(ErrorMessage = "Formato de teléfono inválido")]
    [StringLength(15)]
    public string TelefonoMovil { get; set; }
    
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [StringLength(200)]
    public string Email { get; set; }
    
    [StringLength(300)]
    public string DireccionConsultorio { get; set; }
    
    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    public int? RegionSaludId { get; set; }
    public int? InstalacionSaludId { get; set; }
    
    [StringLength(200)]
    public string InstalacionPersonalizada { get; set; }
    
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaActualizacion { get; set; }
    
    [StringLength(450)]
    public string UsuarioRegistro { get; set; }
    
    public bool Activo { get; set; } = true;
    public bool Verificado { get; set; } = false;
    
    public DateTime? FechaVerificacion { get; set; }
    
    [StringLength(450)]
    public string UsuarioVerificador { get; set; }
    
    [StringLength(500)]
    public string Observaciones { get; set; }
    
    // Propiedades de navegación
    public ProvinciaViewModel Provincia { get; set; }
    public DistritoViewModel Distrito { get; set; }
    public RegionSaludViewModel RegionSalud { get; set; }
    public InstalacionSaludViewModel InstalacionSalud { get; set; }
    
    // Propiedades calculadas
    public string NombreCompletoConTitulo => $"Dr. {PrimerNombre} {PrimerApellido}";
    public string EspecialidadCompleta => string.IsNullOrEmpty(Subespecialidad) 
        ? Especialidad 
        : $"{Especialidad} - {Subespecialidad}";
    
    public int TotalPacientesAtendidos { get; set; } // Se calcula desde servicio
    
    // Validaciones personalizadas
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Validar que el número de colegiatura tenga formato válido
        if (!string.IsNullOrEmpty(NumeroColegiatura) && !NumeroColegiatura.All(char.IsLetterOrDigit))
        {
            yield return new ValidationResult(
                "El número de colegiatura solo puede contener letras y números",
                new[] { nameof(NumeroColegiatura) });
        }
        
        // Validar que si es especialista en cannabis, tenga subespecialidad
        if (Especialidad.Contains("Cannabis", StringComparison.OrdinalIgnoreCase) && 
            string.IsNullOrEmpty(Subespecialidad))
        {
            yield return new ValidationResult(
                "Los médicos especialistas en cannabis deben especificar su subespecialidad",
                new[] { nameof(Subespecialidad) });
        }
        
        // Validar fecha de verificación si está verificado
        if (Verificado && !FechaVerificacion.HasValue)
        {
            yield return new ValidationResult(
                "Debe especificar la fecha de verificación para médicos verificados",
                new[] { nameof(FechaVerificacion) });
        }
    }
    public string ProvinciaNombre { get; set; }
    public string DistritoNombre { get; set; }
    public string RegionSaludNombre { get; set; }
    public string InstalacionSaludNombre { get; set; }

    // Actualiza la propiedad calculada:
    public string NombreCompleto 
    { 
        get 
        {
            var nombres = $"{PrimerNombre} {SegundoNombre}".Trim();
            var apellidos = $"{PrimerApellido} {SegundoApellido}".Trim();
            return $"{nombres} {apellidos}".Trim();
        }
    }
}