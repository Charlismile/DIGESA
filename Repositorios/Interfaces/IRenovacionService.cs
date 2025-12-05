using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IRenovacionService
{
    Task<ResultModel<bool>> VerificarDisponibilidadRenovacionAsync(int pacienteId);
    Task<ResultModel<int>> IniciarRenovacionAsync(int solicitudOriginalId, List<int> documentosMedicosIds);
    Task ProcesarRecordatoriosVencimientoAsync();
    Task InactivarCarnetsVencidosAsync();
    Task<List<RenovacionPendienteModel>> ObtenerRenovacionesPendientesAsync();
    Task<ResultModel<bool>> CompletarRenovacionAsync(int solicitudRenovacionId, bool aprobada, string comentario);
}