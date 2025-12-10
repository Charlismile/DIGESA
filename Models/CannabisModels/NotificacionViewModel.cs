using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class NotificacionViewModel : IValidatableObject
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La solicitud es requerida")]
    public int SolRegCannabisId { get; set; }
    
    [Required(ErrorMessage = "Los días de antelación son requeridos")]
    [Range(1, 90, ErrorMessage = "Los días de antelación deben estar entre 1 y 90")]
    public int? DiasAntelacion { get; set; }
    
    public DateTime? FechaEnvio { get; set; }
    public bool? EmailEnviado { get; set; }
    
    [StringLength(450)]
    public string UsuarioNotificado { get; set; }
    
    [Required(ErrorMessage = "El tipo de notificación es requerido")]
    [StringLength(50)]
    public string TipoNotificacion { get; set; } // "Vencimiento", "Renovacion", "Recordatorio"
    
    // Contenido de la notificación
    public string Asunto { get; set; }
    public string Mensaje { get; set; }
    public string DestinatarioEmail { get; set; }
    public string DestinatarioTelefono { get; set; }
    
    // Plantilla
    public int? PlantillaEmailId { get; set; }
    
    // Propiedades calculadas
    public string MensajeRecordatorio => 
        $"Su identificación está próxima a vencer en {DiasAntelacion} días. " +
        "Favor iniciar proceso de renovación, en caso que sea necesario.";
    
    // Validaciones personalizadas
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TipoNotificacion == "Vencimiento" && (!DiasAntelacion.HasValue || DiasAntelacion > 60))
        {
            yield return new ValidationResult(
                "Para notificaciones de vencimiento, los días de antelación deben ser máximo 60",
                new[] { nameof(DiasAntelacion) });
        }
        
        if (string.IsNullOrEmpty(DestinatarioEmail) && string.IsNullOrEmpty(DestinatarioTelefono))
        {
            yield return new ValidationResult(
                "Debe especificar al menos un medio de contacto (email o teléfono)",
                new[] { nameof(DestinatarioEmail), nameof(DestinatarioTelefono) });
        }
    }
}