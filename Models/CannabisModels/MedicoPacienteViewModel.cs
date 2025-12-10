using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class MedicoPacienteViewModel
{
    public int Id { get; set; }
    
    [StringLength(100)]
    public string PrimerNombre { get; set; }
    
    [StringLength(100)]
    public string PrimerApellido { get; set; }
    
    [StringLength(150)]
    public string MedicoDisciplina { get; set; }
    
    [StringLength(100)]
    public string MedicoIdoneidad { get; set; }
    
    [StringLength(15)]
    public string MedicoTelefono { get; set; }
    
    public int? RegionId { get; set; }
    public int? InstalacionId { get; set; }
    
    [Required(ErrorMessage = "El detalle médico es obligatorio")]
    [StringLength(500)]
    public string DetalleMedico { get; set; }
    
    public int? PacienteId { get; set; }
    
    [StringLength(200)]
    public string InstalacionPersonalizada { get; set; }
    
    // Propiedades de navegación
    public RegionSaludViewModel Region { get; set; }
    public InstalacionSaludViewModel Instalacion { get; set; }
    
    // Propiedades calculadas
    public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}";
}