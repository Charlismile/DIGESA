using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class AprobacionTransferenciaViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La transferencia es requerida")]
    public int TransferenciaId { get; set; }
    
    [Required(ErrorMessage = "El usuario es requerido")]
    [StringLength(450)]
    public string UsuarioId { get; set; }
    
    public bool? Aprobada { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    
    [StringLength(500)]
    public string Comentario { get; set; }
    
    public int? NivelAprobacion { get; set; }
    
    // Propiedades de navegación
    public TransferenciaViewModel Transferencia { get; set; }
    public string UsuarioNombre { get; set; }
}