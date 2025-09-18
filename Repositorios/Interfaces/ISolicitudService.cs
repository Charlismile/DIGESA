using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface ISolicitudService
{
    Task<List<SolicitudModel>> ObtenerSolicitudesAsync();
    Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync();
}