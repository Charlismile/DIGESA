using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioRenovaciones
{
    // Métodos principales
    Task<ResultadoRenovacionViewModel> IniciarRenovacion(int solicitudId, string usuarioId);
    Task<ResultadoRenovacionViewModel> CompletarRenovacion(int solicitudNuevaId, string usuarioId);
    Task<bool> CancelarRenovacion(int solicitudId, string usuarioId, string motivo);
    
    // Consultas
    Task<List<SolicitudCannabisViewModel>> ObtenerSolicitudesPorVencer(int dias = 30);
    Task<List<SolicitudCannabisViewModel>> ObtenerSolicitudesVencidas();
    Task<List<SolicitudCannabisViewModel>> ObtenerSolicitudesParaInactivar();
    Task<SolicitudConHistorialViewModel> ObtenerSolicitudConHistorial(int solicitudId);
    
    // Procesos automáticos
    Task<bool> EnviarNotificacionesVencimiento();
    Task<bool> InactivarCarnetsVencidos();
    Task<bool> ProcesarRenovacionesAutomaticas();
    
    // Reportes
    Task<ReporteRenovacionesViewModel> GenerarReporteRenovaciones(DateTime fechaInicio, DateTime fechaFin);
    Task<byte[]> ExportarReporteRenovaciones(DateTime fechaInicio, DateTime fechaFin, string formato);

}