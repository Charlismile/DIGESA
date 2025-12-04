using DIGESA.Components.Pages.Public;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.AspNetCore.Components.Forms;

namespace DIGESA.Repositorios.Interfaces;

public interface ISolicitudService
{
    // Métodos principales
    Task<ResultModel<int>> CrearSolicitudAsync(RegistroCannabisModel registro, List<IBrowserFile>? documentos = null);
    Task<ResultModel<bool>> ActualizarEstadoSolicitudAsync(EvaluacionSolicitudModel evaluacion, string usuarioRevisor);
    Task<ResultModel<SolicitudDetalleModel>> ObtenerSolicitudDetalleAsync(int solicitudId);
    
    // Consultas
    Task<List<SolicitudListModel>> ObtenerSolicitudesAsync(SolicitudesFiltroModel? filtros = null);
    Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync();
    Task<ResultModel<bool>> ValidarSolicitudCompletaAsync(RegistroCannabisModel registro);
    
    // Carnets
    Task<ResultModel<bool>> GenerarCarnetAsync(int solicitudId);
    Task<ResultModel<bool>> InactivarCarnetAsync(int solicitudId, string motivo);
}