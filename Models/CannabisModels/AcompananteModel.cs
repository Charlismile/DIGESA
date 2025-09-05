using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum TipoDocumentoAcompañante { Cedula, Pasaporte }

public enum Parentesco { Padre, Madre, Tutor }
public class AcompananteModel
{
        public int Id { get; set; }

        [Required(ErrorMessage = "El primer nombre es obligatorio.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo letras y espacios.")]
        public string? PrimerNombre { get; set; }

        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]*$", ErrorMessage = "Solo letras y espacios.")]
        public string? SegundoNombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]+$", ErrorMessage = "Solo letras y espacios.")]
        public string? PrimerApellido { get; set; }

        [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñ\s]*$", ErrorMessage = "Solo letras y espacios.")]
        public string? SegundoApellido { get; set; }

        public TipoDocumentoAcompañante TipoDocumentoAcompañanteEnum { get; set; }

        [Required(ErrorMessage = "El número de cedula es obligatorio.")]
        [RegularExpression(@"^(\d{1,2}-\d{1,8}-\d{1,8}|PE-\d{1,8}-\d{1,8}|E-\d{1,8}-\d{1,8}|N-\d{1,8}-\d{1,8}|\d{1,8}AV-\d{1,8}-\d{1,8}|\d{1,8}PI-\d{1,8}-\d{1,8})$", 
                ErrorMessage = "La cédula no tiene un formato válido. Ejemplos válidos: 1-123456-7, PE-123456-7, E-123456-7, N-123456-7, 1AV-123456-7, 1PI-123456-7")]
        public string? NumDocCedula { get; set; }
    
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El número de pasaporte es obligatorio.")]
        public string NumDocPasaporte { get; set; } = "";

        [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "La nacionalidad solo debe contener letras.")]
        public string? Nacionalidad { get; set; }
        
        public Parentesco? ParentescoEnum { get; set; }

        [Required(ErrorMessage = "El parentesco es obligatorio.")]
        public string? Parentesco { get; set; }
        
        [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
        public string? TelefonoPersonal { get; set; }
}

