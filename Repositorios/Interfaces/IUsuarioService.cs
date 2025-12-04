using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IUsuarioService
{
    // Gestión de usuarios
    Task<List<UsuarioAprobacionModel>> ObtenerUsuariosAsync(FiltroUsuariosModel filtros);
    Task<ResultModel<bool>> CambiarEstadoUsuarioAsync(CambioEstadoUsuarioModel cambio, string usuarioModificador);
    Task<ResultModel<UsuarioModel>> ObtenerUsuarioPorIdAsync(string usuarioId);
    Task<ResultModel<bool>> ReasignarRolAsync(string usuarioId, string nuevoRol, string motivo);
    
    // Historial
    Task<List<HistorialUsuarioModel>> ObtenerHistorialUsuarioAsync(string usuarioId);
    Task<List<HistorialUsuarioModel>> ObtenerHistorialPorFechasAsync(DateTime fechaInicio, DateTime fechaFin);
}