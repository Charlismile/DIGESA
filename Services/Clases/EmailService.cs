using System.Net;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Services.Interfaces;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public async Task EnviarNotificacionNuevaSolicitud(Paciente paciente)
    {
        var mensaje = $"Se ha presentado una nueva solicitud de paciente:\n\n{paciente.NombreCompleto}\n{paciente.TipoDocumento} - {paciente.NumeroDocumento}";

        using var cliente = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port);
        cliente.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
        cliente.EnableSsl = true;

        var mail = new MailMessage
        {
            From = new MailAddress(_smtpSettings.From),
            Subject = "Nueva solicitud de paciente",
            Body = mensaje,
            IsBodyHtml = false
        };

        mail.To.Add("admin@example.com"); // Puedes traer esto desde DB

        await cliente.SendMailAsync(mail);
    }
}