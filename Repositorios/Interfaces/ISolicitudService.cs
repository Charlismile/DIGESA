using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface ISolicitudService
{
    Task<List<SolicitudModel>> ObtenerSolicitudesAsync();
    Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync();
    Task<List<SolicitudModel>> ObtenerSolicitudesPendientesORevisionAsync();
    Task<bool> ActualizarEstadoSolicitudAsync(int id, string nuevoEstado);
}