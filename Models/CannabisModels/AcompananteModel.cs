using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

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

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public TipoDocumento TipoDocumento { get; set; }

    // Campo unificado para número de documento
    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    public string? NumeroDocumento { get; set; }

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "La nacionalidad solo debe contener letras.")]
    public string? Nacionalidad { get; set; }
    
    [Required(ErrorMessage = "El parentesco es obligatorio.")]
    public Parentesco Parentesco { get; set; }

    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public string? TelefonoMovil { get; set; }  
}