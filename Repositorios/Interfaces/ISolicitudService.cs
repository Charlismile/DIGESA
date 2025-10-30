using DIGESA.Components.Pages.Public;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositorios.Interfaces;

public interface ISolicitudService
{
    Task<int> CrearSolicitudCompletaAsync(RegistroCanabisUnionModel registro, Solicitud.DocumentosModel documentos);
    Task<bool> ValidarSolicitudCompletaAsync(RegistroCanabisUnionModel registro);
    Task<int?> CrearOGuardarInstalacionPersonalizadaAsync(string nombreInstalacion);
    Task<Dictionary<string, int>> ObtenerTiposDocumentoAsync();
    
    // Métodos para la lista de solicitudes
    Task<List<SolicitudModel>> ObtenerSolicitudesAsync();
    Task<bool> ActualizarEstadoSolicitudAsync(int solicitudId, string nuevoEstado, string usuarioRevisor, string comentario);
    
    // NUEVO MÉTODO PARA EL INDEXBOARD
    Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync();
}