using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface ITransferenciaService
{
    // Gestión de transferencias
    Task<ResultModel<int>> SolicitarTransferenciaAsync(SolicitudTransferenciaModel solicitud, string usuarioOrigenId);
    Task<ResultModel<bool>> AprobarTransferenciaAsync(AprobacionTransferenciaInputModel aprobacion, string usuarioAprobador);
    Task<ResultModel<bool>> RechazarTransferenciaAsync(int transferenciaId, string motivo, string usuarioAprobador);
    
    // Consultas
    Task<List<TransferenciaModel>> ObtenerTransferenciasPendientesAsync(string? usuarioId = null);
    Task<List<TransferenciaModel>> ObtenerTransferenciasPorSolicitudAsync(int solicitudId);
    Task<TransferenciaModel?> ObtenerTransferenciaDetalleAsync(int transferenciaId);
    
    // Aprobaciones en múltiples niveles
    Task<ResultModel<bool>> ProcesarNivelAprobacionAsync(int transferenciaId, int nivel, bool aprobada, string comentario, string usuarioAprobador);
}