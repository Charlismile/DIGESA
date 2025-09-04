using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IPaciente
{
    Task<PacienteModel?> BuscarPorDocumentoAsync(string documento);
    Task<PacienteEstadoModel> GetEstadoPacienteAsync(string documento);
}