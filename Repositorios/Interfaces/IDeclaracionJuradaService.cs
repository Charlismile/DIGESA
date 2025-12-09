using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IDeclaracionJuradaService
{
    /// <summary>
    /// Genera una nueva declaración jurada para una solicitud
    /// </summary>
    Task<ResultModel<DeclaracionJuradaModel>> GenerarDeclaracionAsync(int solicitudId);
    
    /// <summary>
    /// Acepta la declaración jurada por parte del paciente
    /// </summary>
    Task<ResultModel<bool>> AceptarDeclaracionAsync(int declaracionId, string ipAceptacion = null);
    
    /// <summary>
    /// Obtiene la declaración jurada asociada a una solicitud
    /// </summary>
    Task<ResultModel<DeclaracionJuradaModel>> ObtenerDeclaracionPorSolicitudAsync(int solicitudId);
    
    /// <summary>
    /// Verifica si una declaración jurada ha sido aceptada
    /// </summary>
    Task<ResultModel<bool>> VerificarDeclaracionAceptadaAsync(int solicitudId);
    
    /// <summary>
    /// Obtiene el texto completo de la declaración jurada
    /// </summary>
    Task<string> ObtenerTextoDeclaracionJuradaAsync();
}