using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum TipoDocumentoPaciente { Cedula, Pasaporte }
public enum RequiereAcompanante { Si, No }
public enum MotivoRequerimientoAcompanante { PacienteMenorEdad, PacienteDiscapacidad }
public enum Sexo {Masculino, Femenino}
public class PacienteModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El primer nombre es obligatorio.")]
    public string? PrimerNombre { get; set; }

    public string? SegundoNombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio.")]
    public string? PrimerApellido { get; set; }

    public string? SegundoApellido { get; set; }
    
    public TipoDocumentoPaciente TipoDocumentoPacienteEnum { get; set; }

    [Required(ErrorMessage = "El número de cedula es obligatorio.")]
    [RegularExpression(@"^(\d{1,2}-\d{1,8}-\d{1,8}|PE-\d{1,8}-\d{1,8}|E-\d{1,8}-\d{1,8}|N-\d{1,8}-\d{1,8}|\d{1,8}AV-\d{1,8}-\d{1,8}|\d{1,8}PI-\d{1,8}-\d{1,8})$", 
        ErrorMessage = "La cédula no tiene un formato válido. Ejemplos válidos: 1-123456-7, PE-123456-7, E-123456-7, N-123456-7, 1AV-123456-7, 1PI-123456-7")]
    public string? NumDocCedula { get; set; }
    
    [Required(ErrorMessage = "El número de pasaporte es obligatorio.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El pasaporte no tiene un formato válido. Ejemplos válidos:")]
    public string NumDocPasaporte { get; set; } = "";

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "La nacionalidad solo debe contener letras.")]
    public string? Nacionalidad { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    public Sexo? SexoEnum { get; set; }
    [Required(ErrorMessage = "Indique si requiere acompañante.")]
    public RequiereAcompanante? RequiereAcompanante { get; set; }
    public MotivoRequerimientoAcompanante? MotivoRequerimientoAcompanante { get; set; }

    [Required(ErrorMessage = "Indique la discapacidad.")]
    public string? TipoDiscapacidad { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public string? TelefonoResidencial { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public string? TelefonoPersonal { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public string? TelefonoLaboral { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Correo electrónico inválido.")]
    public string? CorreoElectronico { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string? DireccionExacta { get; set; }
    
    public int? pacienteInstalacionId { get; set; }
    public int? pacienteRegionId { get; set; }
    public int? pacienteProvinciaId { get; set; }
    public int? pacienteDistritoId { get; set; }
    public int? pacienteCorregimientoId { get; set; }
    
    public string? pacienteRegion { get; set; }
    public string? pacienteProvincia { get; set; }
    public string? pacienteDistrito { get; set; }
    public string? pacienteCorregimiento { get; set; }
    public string? pacienteInstalacion { get; set; }
}