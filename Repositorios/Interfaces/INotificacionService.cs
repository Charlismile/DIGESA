using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface INotificacionService
{
    /// <summary>
    /// Programa una notificación de vencimiento
    /// </summary>
    Task<ResultModel<bool>> ProgramarNotificacionVencimientoAsync(int solicitudId, int diasAntelacion);
    
    /// <summary>
    /// Envía notificaciones pendientes
    /// </summary>
    Task<ResultModel<bool>> EnviarNotificacionesPendientesAsync();
    
    /// <summary>
    /// Obtiene dashboard de vencimientos
    /// </summary>
    Task<DashboardVencimientosModel> ObtenerDashboardVencimientosAsync();
    
    /// <summary>
    /// Obtiene notificaciones por solicitud
    /// </summary>
    Task<List<NotificacionVencimientoModel>> ObtenerNotificacionesPorSolicitudAsync(int solicitudId);
    
    /// <summary>
    /// Obtiene notificaciones pendientes de envío
    /// </summary>
    Task<List<NotificacionVencimientoModel>> ObtenerNotificacionesPendientesAsync();
    
    /// <summary>
    /// Obtiene plantilla de email por tipo
    /// </summary>
    Task<ResultModel<PlantillaEmailModel>> ObtenerPlantillaAsync(string tipoNotificacion);
    
    /// <summary>
    /// Actualiza una plantilla de email
    /// </summary>
    Task<ResultModel<PlantillaEmailModel>> ActualizarPlantillaAsync(PlantillaEmailModel plantilla);
    
    /// <summary>
    /// Envía notificación de cambio de estado
    /// </summary>
    Task<bool> EnviarNotificacionEstadoAsync(string destinatario, string numeroSolicitud, string estado, string comentario);
}