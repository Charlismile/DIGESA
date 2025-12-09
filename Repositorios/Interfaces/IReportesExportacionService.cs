using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IReportesExportacionService
{
    /// <summary>
    /// Exporta lista de pacientes activos e inactivos en formato especificado
    /// </summary>
    Task<ResultModel<byte[]>> ExportarPacientesActivosInactivosAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato);
    
    /// <summary>
    /// Exporta lista de inscripciones vs renovaciones
    /// </summary>
    Task<ResultModel<byte[]>> ExportarInscripcionesRenovacionesAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato);
    
    /// <summary>
    /// Exporta dashboard de estadísticas
    /// </summary>
    Task<ResultModel<byte[]>> ExportarDashboardEstadisticasAsync(
        DashboardEstadisticasModel dashboard, TipoExportacion formato);
    
    /// <summary>
    /// Exporta reporte de carnets próximos a vencer
    /// </summary>
    Task<ResultModel<byte[]>> ExportarCarnetsProximosVencerAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato);
    
    /// <summary>
    /// Exporta reporte de renovaciones pendientes
    /// </summary>
    Task<ResultModel<byte[]>> ExportarRenovacionesPendientesAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato);
    
    /// <summary>
    /// Exporta reporte de declaraciones juradas
    /// </summary>
    Task<ResultModel<byte[]>> ExportarDeclaracionesJuradasAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato);
}