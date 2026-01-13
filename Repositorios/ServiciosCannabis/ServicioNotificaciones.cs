using System.Net;
using System.Net.Mail;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Configuracion;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.Extensions.Options;

namespace DIGESA.Repositorios.ServiciosCannabis;

public class ServicioNotificaciones : IServicioNotificaciones
{
    private readonly EmailSettings _email;

    public ServicioNotificaciones(IOptions<EmailSettings> email)
    {
        _email = email.Value;
    }

    public async Task EnviarAsync(
        EnumViewModel.NotificationType tipo,
        string emailDestino,
        object datos)
    {
        var (asunto, cuerpo) = ConstruirCorreo(tipo, datos);

        using var smtp = new SmtpClient(_email.SmtpServer, _email.SmtpPort)
        {
            Credentials = new NetworkCredential(
                _email.SmtpUsername,
                _email.SmtpPassword),
            EnableSsl = _email.EnableSsl
        };

        var mensaje = new MailMessage(
            _email.FromEmail,
            emailDestino,
            asunto,
            cuerpo)
        {
            IsBodyHtml = true
        };

        await smtp.SendMailAsync(mensaje);
    }

    private static (string asunto, string cuerpo) ConstruirCorreo(
        EnumViewModel.NotificationType tipo,
        object datos)
    {
        return tipo switch
        {
            EnumViewModel.NotificationType.RenovacionAprobada =>
            (
                "Solicitud aprobada",
                "<p>Su solicitud fue <strong>APROBADA</strong>.</p>"
            ),

            EnumViewModel.NotificationType.RenovacionRechazada =>
            (
                "Solicitud rechazada",
                "<p>Su solicitud fue <strong>RECHAZADA</strong>.</p>"
            ),

            EnumViewModel.NotificationType.VencimientoCarnet =>
            (
                "Carnet próximo a vencer",
                "<p>Su carnet está próximo a vencer. Inicie la renovación.</p>"
            ),

            EnumViewModel.NotificationType.CarnetInactivado =>
            (
                "Carnet inactivado",
                "<p>Su carnet ha sido inactivado por vencimiento.</p>"
            ),

            _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, null)
        };
    }
}
