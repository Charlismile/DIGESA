using DIGESA.Models.CannabisModels.Formularios;
using DIGESA.Models.CannabisModels.Listados;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface ISolicitudCannabisService
{
    Task<int> CrearSolicitudAsync(SolicitudCannabisFormViewModel model);
    Task<SolicitudCannabisFormViewModel?> ObtenerPorIdAsync(int id);
    Task<List<PacienteListadoViewModel>> ObtenerSolicitudesAsync();
    Task<bool> AprobarSolicitudAsync(int id, string usuario);
    Task<bool> RechazarSolicitudAsync(int id, string comentario, string usuario);
    Task<bool> InactivarCarnetAsync(int id, string razon, string usuario);
    
    Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync();
    
    Task<bool> AprobarSolicitudAsync(int id, string usuario, string? comentario = null);
}
