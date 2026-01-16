using DIGESA.Data;
using DIGESA.Models.CannabisModels.Listados;
using DIGESA.Models.CannabisModels.Reportes;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IExcelExportService
{
    Task<byte[]> ExportSolicitudesAsync(
        List<PacienteListadoViewModel> solicitudes,
        string? estadoFiltro,
        string? documentoBusqueda);
    
    Task<byte[]> ExportUsuariosAsync(List<ApplicationUser> usuarios);
    
    Task<byte[]> ExportReporteEstadisticoAsync(Dictionary<string, int> estadisticas);
    
    // Nuevo método
    Task<byte[]> ExportEstadisticasPacientesProductosAsync(
        List<EstadisticaPacienteProducto> estadisticas);
}