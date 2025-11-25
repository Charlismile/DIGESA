using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class EvaluacionSolicitudModel
{
    public int SolicitudId { get; set; }
    
    [Required(ErrorMessage = "La acción es requerida")]
    public string Accion { get; set; } = string.Empty; 
    
    [Required(ErrorMessage = "El motivo es obligatorio")]
    public string Motivo { get; set; } = string.Empty;
    
    public string? ComentarioAdicional { get; set; }
    
    
}