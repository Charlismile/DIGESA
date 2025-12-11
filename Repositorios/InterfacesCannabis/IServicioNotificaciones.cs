namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioNotificaciones
{
    Task<bool> EnviarNotificacionVencimiento(int solicitudId, string emailDestino, int diasRestantes);
    Task<bool> EnviarNotificacionRenovacionIniciada(int solicitudId, string emailDestino);
    Task<bool> EnviarNotificacionRenovacionCompletada(int solicitudId, string emailDestino);
    Task<bool> EnviarNotificacionCarnetInactivado(int solicitudId, string emailDestino, string razon);
    Task<bool> EnviarNotificacionGeneral(string emailDestino, string asunto, string cuerpo);
    
    // Para SMS
    Task<bool> EnviarSMSVencimiento(int solicitudId, string telefono, int diasRestantes);
    Task<bool> EnviarSMSRenovacion(int solicitudId, string telefono);
    
    // Plantillas
    Task<string> ObtenerPlantillaEmail(string tipo);
    Task<bool> GuardarPlantillaEmail(string tipo, string asunto, string cuerpo);
}