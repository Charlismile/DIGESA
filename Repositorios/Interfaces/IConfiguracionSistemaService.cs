using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IConfiguracionSistemaService
{
    /// <summary>
    /// Obtiene la configuración de notificaciones del sistema
    /// </summary>
    Task<ResultModel<ConfiguracionNotificacionModel>> ObtenerConfiguracionNotificacionesAsync();
    
    /// <summary>
    /// Actualiza la configuración de notificaciones
    /// </summary>
    Task<ResultModel<ConfiguracionNotificacionModel>> ActualizarConfiguracionNotificacionesAsync(
        ConfiguracionNotificacionModel configuracion);
    
    /// <summary>
    /// Obtiene el texto de la declaración jurada de responsabilidad
    /// </summary>
    Task<ResultModel<string>> ObtenerDeclaracionJuradaTextoAsync();
    
    /// <summary>
    /// Obtiene todos los parámetros configurables del sistema
    /// </summary>
    Task<ResultModel<List<ParametroSistemaModel>>> ObtenerParametrosSistemaAsync();
    
    /// <summary>
    /// Actualiza un parámetro del sistema
    /// </summary>
    Task<ResultModel<ParametroSistemaModel>> ActualizarParametroSistemaAsync(
        ParametroSistemaModel parametro);
    
    /// <summary>
    /// Obtiene la vigencia configurada para los carnets (en años)
    /// </summary>
    Task<int> ObtenerVigenciaCarnetAniosAsync();
    
    /// <summary>
    /// Obtiene los días antes del vencimiento para permitir renovación
    /// </summary>
    Task<int> ObtenerDiasRenovacionAnticipadaAsync();
    
    /// <summary>
    /// Obtiene la configuración de inactivación automática
    /// </summary>
    Task<ConfiguracionInactivacionModel> ObtenerConfiguracionInactivacionAsync();
}