using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteDiagnosticoViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El paciente es requerido")]
    public int? PacienteId { get; set; }
    
    [StringLength(200)]
    public string NombreDiagnostico { get; set; }
    
    public int? DiagnosticoId { get; set; }
    
    [StringLength(20)]
    public string Tipo { get; set; }
    
    [StringLength(300)]
    public string DetalleTratamiento { get; set; }
    
    public DateTime? FechaDiagnostico { get; set; }
    
    // Propiedades de navegación
    public DiagnosticoViewModel Diagnostico { get; set; }
}