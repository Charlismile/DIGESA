using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IAdminService
{
    /// <summary>
    /// Obtiene estadísticas para el dashboard
    /// </summary>
    Task<EstadisticasDashboardModel> ObtenerEstadisticasAsync();
    
    /// <summary>
    /// Obtiene solicitudes recientes
    /// </summary>
    Task<List<SolicitudListModel>> ObtenerSolicitudesRecientesAsync(int count = 10);
    
    /// <summary>
    /// Obtiene estadísticas por región
    /// </summary>
    Task<List<EstadisticasPorRegion>> ObtenerEstadisticasPorRegionAsync();
    
    /// <summary>
    /// Obtiene dashboard de vencimientos
    /// </summary>
    Task<DashboardVencimientosModel> ObtenerDashboardVencimientosAsync();
    
    /// <summary>
    /// Obtiene reporte de inscripciones vs renovaciones
    /// </summary>
    Task<InscripcionesReporteModel> ObtenerReporteInscripcionesAsync(ReporteFiltrosModel filtros);
    
    /// <summary>
    /// Obtiene reporte de pacientes activos/inactivos
    /// </summary>
    Task<PacientesActivosInactivosModel> ObtenerReportePacientesActivosInactivosAsync(ReporteFiltrosModel filtros);
}