using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El primer nombre es obligatorio.")]
    public string? PrimerNombre { get; set; }

    public string? SegundoNombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio.")]
    public string? PrimerApellido { get; set; }

    public string? SegundoApellido { get; set; }

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public string? TipoDocumento { get; set; }

    [Required(ErrorMessage = "El número de cedula es obligatorio.")]
    [RegularExpression(@"^(\d{1,2}-\d{1,8}-\d{1,8}|PE-\d{1,8}-\d{1,8}|E-\d{1,8}-\d{1,8}|N-\d{1,8}-\d{1,8}|\d{1,8}AV-\d{1,8}-\d{1,8}|\d{1,8}PI-\d{1,8}-\d{1,8})$", 
        ErrorMessage = "La cédula no tiene un formato válido. Ejemplos válidos: 1-123456-7, PE-123456-7, E-123456-7, N-123456-7, 1AV-123456-7, 1PI-123456-7")]
    public string? NumDocCedula { get; set; }
    
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El número de pasaporte es obligatorio.")]
    public string NumDocPasaporte { get; set; } = "";

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "La nacionalidad solo debe contener letras.")]
    public string? Nacionalidad { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    public string? Sexo { get; set; }

    public bool? RequiereAcompanante { get; set; }

    [Required(ErrorMessage = "El motivo es obligatorio.")]
    public string? MotivoRequerimientoAcompanante { get; set; }

    [Required(ErrorMessage = "Indique la discapacidad.")]
    public string? TipoDiscapacidad { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public long? TelefonoResidencial { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public long? TelefonoPersonal { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public long? TelefonoLaboral { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
    public string? CorreoElectronico { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string? DireccionExacta { get; set; }

    [Required(ErrorMessage = "Seleccione una via de consumo.")]
    public string? ViaConsumoProducto { get; set; }

    [Required(ErrorMessage = "El detalle de la dosis del tratamiento es obligatoria.")]
    public string? DetDosisPaciente { get; set; }

    [Required(ErrorMessage = "El tiempo de tratamiento es obligatoria.")]
    public string? DuracionTratamiento { get; set; }
    
}