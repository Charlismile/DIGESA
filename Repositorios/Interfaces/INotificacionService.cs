using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface INotificacionService
{
    // Notificaciones de vencimiento
    Task<ResultModel<bool>> ProgramarNotificacionVencimientoAsync(int solicitudId, int diasAntelacion);
    Task<ResultModel<bool>> EnviarNotificacionesPendientesAsync();
    Task<DashboardVencimientosModel> ObtenerDashboardVencimientosAsync();
    
    // Consultas
    Task<List<NotificacionVencimientoModel>> ObtenerNotificacionesPorSolicitudAsync(int solicitudId);
    Task<List<NotificacionVencimientoModel>> ObtenerNotificacionesPendientesAsync();
    
    // Plantillas de email
    Task<ResultModel<PlantillaEmailModel>> ObtenerPlantillaAsync(string tipoNotificacion);
    Task<ResultModel<PlantillaEmailModel>> ActualizarPlantillaAsync(PlantillaEmailModel plantilla);
}