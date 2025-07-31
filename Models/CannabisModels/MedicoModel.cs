using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class MedicoModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string? PrimerNombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string? PrimerApellido { get; set; }

    [Required(ErrorMessage = "La disciplina es obligatoria.")]
    public string? MedicoDisciplina { get; set; }

    [Required(ErrorMessage = "El número de idoneidad es obligatorio.")]
    public string? MedicoIdoneidad { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "Número de teléfono inválido.")]
    public long? MedicoTelefono { get; set; }

    [Required(ErrorMessage = "La Instalacion es obligatorio.")]
    public string? MedicoInstalacion { get; set; }
    
}



