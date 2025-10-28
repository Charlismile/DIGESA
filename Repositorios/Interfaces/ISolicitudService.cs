using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositorios.Interfaces;

public interface ISolicitudService
{
    Task<List<SolicitudModel>> ObtenerSolicitudesAsync();
    Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync();
    Task<List<SolicitudModel>> ObtenerSolicitudesPendientesORevisionAsync();
    Task<SolicitudDetalleModel?> ObtenerDetalleSolicitudAsync(int id);
    Task<bool> ActualizarEstadoSolicitudAsync(int id, string nuevoEstado, string usuarioRevisor, string? comentario = null);
    Task<bool> RegistrarHistorialCambioAsync(int solicitudId, string nombreEstado, string usuarioRevisor, string? comentario);
    Task<List<TbSolRegCannabisHistorial>> ObtenerHistorialSolicitudAsync(int solicitudId);
    Task<List<SolicitudModel>> BuscarSolicitudesPorCedulaAsync(string cedula);
}