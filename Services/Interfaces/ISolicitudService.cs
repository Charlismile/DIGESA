using DIGESA.Models.Entities.DIGESA;

namespace DIGESA.Services.Interfaces;

public interface ISolicitudService
{
    Task GuardarSolicitud(Solicitud solicitud);
    Task<Solicitud?> ObtenerSolicitudPorId(int id);
    Task<List<Solicitud>> ObtenerSolicitudesPendientes();
    Task ActualizarSolicitud(Solicitud solicitud);
}
