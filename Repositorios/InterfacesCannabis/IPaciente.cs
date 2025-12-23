using DIGESA.Models.CannabisModels.Renovaciones;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IPaciente
{
    Task<EstadoSolicitudViewModel?> GetEstadoPacienteAsync(string documento);
    Task<PacienteViewModel?> BuscarPorDocumentoAsync(string documento);
}