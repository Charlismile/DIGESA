using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Services.Interfaces;

public interface IEmailService
{
    Task EnviarNotificacionNuevaSolicitud(Paciente paciente);
}