using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IInscripcionesReporteService
{
    /// <summary>
    /// Obtiene estadísticas detalladas de inscripciones y renovaciones
    /// </summary>
    Task<ResultModel<InscripcionesEstadisticasModel>> ObtenerEstadisticasInscripcionesAsync(
        ReporteFiltrosModel filtros);
    
    /// <summary>
    /// Obtiene lista detallada de inscripciones con filtros aplicables
    /// </summary>
    Task<ResultModel<List<InscripcionDetalleModel>>> ObtenerInscripcionesDetalladasAsync(
        ReporteFiltrosModel filtros);
    
    /// <summary>
    /// Obtiene lista de pacientes con estado activo/inactivo
    /// </summary>
    Task<ResultModel<List<PacienteActivoInactivoModel>>> ObtenerPacientesActivosInactivosAsync(
        ReporteFiltrosModel filtros);
    
    /// <summary>
    /// Obtiene reporte de renovaciones pendientes
    /// </summary>
    Task<ResultModel<List<RenovacionPendienteReporteModel>>> ObtenerRenovacionesPendientesReporteAsync(
        ReporteFiltrosModel filtros);
    
    /// <summary>
    /// Obtiene reporte de carnets próximos a vencer
    /// </summary>
    Task<ResultModel<List<CarnetProximoVencerModel>>> ObtenerCarnetsProximosVencerReporteAsync(
        ReporteFiltrosModel filtros);
    
    /// <summary>
    /// Obtiene reporte histórico de inscripciones por período
    /// </summary>
    Task<ResultModel<HistoricoInscripcionesModel>> ObtenerHistoricoInscripcionesAsync(
        DateTime fechaInicio, DateTime fechaFin);
}