using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IServicioNotificaciones
{
    Task EnviarAsync(
        EnumViewModel.NotificationType tipo,
        string emailDestino,
        object datos
    );
}
