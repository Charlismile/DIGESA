using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IReporteService
{
    // Generación de reportes
    Task<ResultModel<ReporteGeneradoModel>> GenerarReporteSolicitudesAsync(ReporteFiltrosModel filtros, TipoExportacion formato);
    Task<ResultModel<ReporteGeneradoModel>> GenerarReportePacientesAsync(ReporteFiltrosModel filtros, TipoExportacion formato);
    Task<ResultModel<ReporteGeneradoModel>> GenerarReporteCarnetsAsync(ReporteFiltrosModel filtros, TipoExportacion formato);
    
    // Consultas
    Task<List<ReporteGeneradoModel>> ObtenerReportesGeneradosAsync(string usuarioId, int dias = 30);
    Task<ResultModel<bool>> EliminarReporteAsync(int reporteId);
    
    // Exportación
    Task<byte[]> ExportarSolicitudesExcelAsync(ReporteFiltrosModel filtros);
    Task<byte[]> ExportarPacientesPDFAsync(ReporteFiltrosModel filtros);
    Task<byte[]> ExportarCarnetsCSVAsync(ReporteFiltrosModel filtros);
    
    // API para sistemas externos
    Task<List<ApiSolicitudModel>> ObtenerSolicitudesAPIAsync(ReporteFiltrosModel filtros, PaginationModel pagination);
}