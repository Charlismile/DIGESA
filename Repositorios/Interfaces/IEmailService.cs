using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IEmailService
{
    Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml);
    Task<bool> EnviarNotificacionVencimientoAsync(NotificacionVencimientoModel notificacion);
    Task<bool> EnviarNotificacionEstadoAsync(string destinatario, string numeroSolicitud, string estado, string comentario);
}