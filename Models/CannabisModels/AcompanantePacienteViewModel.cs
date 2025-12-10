using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class AcompanantePacienteViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El paciente es requerido")]
    public int? PacienteId { get; set; }
    
    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    [StringLength(100)]
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
    
    [StringLength(100)]
    public string Nacionalidad { get; set; }
    
    [Required(ErrorMessage = "El parentesco es obligatorio")]
    [StringLength(100)]
    public string Parentesco { get; set; }
    
    [Required(ErrorMessage = "El teléfono móvil es obligatorio")]
    [StringLength(15)]
    public string TelefonoMovil { get; set; }
    
    // Propiedades calculadas
    public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}";
}