using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class LogNotificacionViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La solicitud es requerida")]
    public int SolicitudId { get; set; }
    
    [Required(ErrorMessage = "El tipo de notificación es requerido")]
    [StringLength(50)]
    public string TipoNotificacion { get; set; }
    
    [Required(ErrorMessage = "La fecha de envío es requerida")]
    public DateTime FechaEnvio { get; set; }
    
    [StringLength(50)]
    public string MetodoEnvio { get; set; }
    
    [StringLength(200)]
    public string Destinatario { get; set; }
    
    [StringLength(50)]
    public string Estado { get; set; }
    
    [StringLength(500)]
    public string Error { get; set; }
    
    // Propiedades calculadas
    public bool FueExitosa => Estado == "Enviado";
    public string IconoEstado => FueExitosa ? "✓" : "✗";
    public string ColorEstado => Estado switch
    {
        "Enviado" => "success",
        "Fallido" => "danger",
        "Pendiente" => "warning",
        _ => "secondary"
    };
}