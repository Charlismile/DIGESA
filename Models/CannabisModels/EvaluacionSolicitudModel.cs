using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class EvaluacionSolicitudModel
{
    public int SolicitudId { get; set; }
    
    [Required(ErrorMessage = "La acción es requerida")]
    public string Accion { get; set; } = "Aprobar"; // "Aprobar" o "Rechazar"
    
    [Required(ErrorMessage = "El motivo es obligatorio")]
    public string Motivo { get; set; } = string.Empty;
    
    public string? ComentarioAdicional { get; set; }
    public string UsuarioRevisor { get; set; } = string.Empty;
}