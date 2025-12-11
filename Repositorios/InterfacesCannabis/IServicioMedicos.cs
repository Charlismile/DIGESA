using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioMedicos
{
    // CRUD Médicos
    Task<MedicoViewModel> CrearMedico(MedicoViewModel medico, string usuarioId);
    Task<MedicoViewModel> ActualizarMedico(int medicoId, MedicoViewModel medico, string usuarioId);
    Task<bool> EliminarMedico(int medicoId, string usuarioId, string motivo);
    Task<MedicoViewModel> ObtenerMedicoPorId(int medicoId);
    Task<MedicoViewModel> ObtenerMedicoPorCodigo(string codigoMedico);
    Task<MedicoViewModel> ObtenerMedicoPorDocumento(string tipoDocumento, string numeroDocumento);
    Task<List<MedicoViewModel>> BuscarMedicos(string criterio, bool soloActivos = true);
    
    // Verificación por Ministerio
    Task<bool> VerificarMedico(int medicoId, string usuarioVerificador, string observaciones);
    Task<bool> RevocarVerificacion(int medicoId, string usuario, string motivo);
    
    // Reportes y estadísticas
    Task<ReporteMedicosViewModel> GenerarReporteMedicos(DateTime? fechaInicio, DateTime? fechaFin);
    Task<EstadisticasMedicosViewModel> ObtenerEstadisticasMedicos();
    
    // Auditoría
    Task<List<AuditoriaMedicoViewModel>> ObtenerAuditoriaMedico(int medicoId);
    Task<List<AuditoriaMedicoViewModel>> ObtenerAuditoriaPorUsuario(string usuarioId);
}