using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class TransferenciaViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La solicitud es requerida")]
    public int SolRegCannabisId { get; set; }
    
    [Required(ErrorMessage = "El usuario origen es requerido")]
    [StringLength(450)]
    public string UsuarioOrigenId { get; set; }
    
    [Required(ErrorMessage = "El usuario destino es requerido")]
    [StringLength(450)]
    public string UsuarioDestinoId { get; set; }
    
    public DateTime? FechaSolicitud { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    
    [StringLength(50)]
    public string Estado { get; set; }
    
    [StringLength(500)]
    public string Comentario { get; set; }
    
    [StringLength(450)]
    public string AprobadoPor { get; set; }
    
    // Propiedades de navegación
    public SolicitudCannabisViewModel Solicitud { get; set; }
    public string UsuarioOrigenNombre { get; set; }
    public string UsuarioDestinoNombre { get; set; }
}