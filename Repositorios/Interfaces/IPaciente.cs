using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IPaciente
{
    Task<PacienteModel?> BuscarPorDocumentoAsync(string documento);
    Task<PacienteEstadoModel> GetEstadoPacienteAsync(string documento);
    Task<ResultModel<PacienteModel>> CrearPacienteAsync(PacienteModel paciente);
    Task<ResultModel<PacienteModel>> ActualizarPacienteAsync(PacienteModel paciente);
    Task<List<InscripcionPacienteModel>> ObtenerInscripcionesAsync(FiltroInscripcionesModel? filtros = null);
    Task<EstadisticasInscripcionesModel> ObtenerEstadisticasInscripcionesAsync();
}