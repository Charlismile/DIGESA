using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioHistorial
{
    // Registros
    Task RegistrarCambioEstado(int solicitudId, string estadoAnterior, string estadoNuevo, 
        string usuario, string comentario);
    Task RegistrarRenovacion(int solicitudAnteriorId, int solicitudNuevaId, 
        string usuario, string razon);
    Task RegistrarInactivacion(int solicitudId, string usuario, string razon);
    Task RegistrarNotificacion(int solicitudId, string tipo, string metodo, 
        string destinatario, bool exitosa, string error = null);
    
    // Consultas
    Task<HistorialCompletoViewModel> ObtenerHistorialCompleto(int pacienteId);
    Task<HistorialCompletoViewModel> ObtenerHistorialCompletoPorCarnet(string numeroCarnet);
    Task<List<EventoHistorialViewModel>> ObtenerLineaTiempo(int pacienteId);
    Task<List<SolicitudHistorialViewModel>> ObtenerCambiosEstado(int solicitudId);
    Task<List<HistorialRenovacionViewModel>> ObtenerRenovaciones(int pacienteId);
    
    // Reportes
    Task<ReporteHistorialViewModel> GenerarReporteHistorial(DateTime fechaInicio, DateTime fechaFin);
    
    //--------------------------------------------
    Task RegistrarEvento(string tipo, string descripcion, string usuario, string entidadId);
    Task RegistrarError(string origen, string mensaje, string usuario);
}