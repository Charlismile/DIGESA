using DIGESA.Models.Entities.DIGESA;

namespace DIGESA.Services.Interfaces
{
    public interface ISolicitudService
    {
        Task<List<Solicitud>> ObtenerSolicitudesPendientes();
        Task<Solicitud?> ObtenerSolicitudPorId(int id);
        Task ActualizarSolicitud(Solicitud solicitud);
        Task<Usuario?> ObtenerUsuarioPorEmail(string email);
    }
}