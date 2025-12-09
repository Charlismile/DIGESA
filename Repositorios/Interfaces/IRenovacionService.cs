using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IRenovacionService
{
    /// <summary>
    /// Verifica si un paciente puede renovar su carnet
    /// </summary>
    Task<ResultModel<bool>> VerificarDisponibilidadRenovacionAsync(int pacienteId);
    
    /// <summary>
    /// Inicia el proceso de renovación de un carnet
    /// </summary>
    Task<ResultModel<int>> IniciarRenovacionAsync(int solicitudOriginalId, List<int> documentosMedicosIds);
    
    /// <summary>
    /// Procesa recordatorios de vencimiento (30, 15, 7 días antes)
    /// </summary>
    Task ProcesarRecordatoriosVencimientoAsync();
    
    /// <summary>
    /// Inactiva carnets vencidos automáticamente
    /// </summary>
    Task InactivarCarnetsVencidosAsync();
    
    /// <summary>
    /// Obtiene renovaciones pendientes de revisión
    /// </summary>
    Task<List<RenovacionPendienteModel>> ObtenerRenovacionesPendientesAsync();
    
    /// <summary>
    /// Completa el proceso de renovación (aprobación/rechazo)
    /// </summary>
    Task<ResultModel<bool>> CompletarRenovacionAsync(int solicitudRenovacionId, bool aprobada, string comentario);
    
    /// <summary>
    /// Obtiene carnets próximos a vencer para inactivación automática
    /// </summary>
    Task<List<CarnetProximoInactivacionModel>> ObtenerCarnetsProximosInactivacionAsync();
    
    /// <summary>
    /// Inactiva carnets vencidos por más de 2 años
    /// </summary>
    Task<ResultModel<int>> InactivarCarnetsVencidos2AniosAsync();
}