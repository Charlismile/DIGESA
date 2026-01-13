using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Renovaciones;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioRenovaciones
{
    Task<bool> IniciarRenovacionAsync(int solicitudId, string usuarioId);
    Task<bool> AprobarRenovacionAsync(int solicitudId, string usuarioId);
    Task ProcesarRenovacionesAutomaticasAsync();

    Task<bool> InactivarCarnetsVencidos();
    Task<bool> ProcesarRenovacionesAutomaticas();
}
