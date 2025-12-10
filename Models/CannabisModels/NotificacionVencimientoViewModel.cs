using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class NotificacionVencimientoViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La solicitud es requerida")]
    public int SolRegCannabisId { get; set; }
    
    public int? DiasAntelacion { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public bool? EmailEnviado { get; set; }
    
    [StringLength(450)]
    public string UsuarioNotificado { get; set; }
    
    [StringLength(50)]
    public string TipoNotificacion { get; set; }
    
    // Mensaje recordatorio
    public string MensajeRecordatorio => "Su identificación está próximo a vencer, favor iniciar proceso de renovación, en caso que sea necesario";
    
    // Propiedades de navegación
    public SolicitudCannabisViewModel Solicitud { get; set; }
}