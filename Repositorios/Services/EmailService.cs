using System.Net;
using System.Net.Mail;
using DIGESA.Repositorios.Interfaces;

namespace DIGESA.Repositorios.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(destinatario))
            {
                _logger.LogWarning("No se puede enviar correo: destinatario vacío");
                return false;
            }

            using var smtp = new SmtpClient(_config["SMTP:Host"])
            {
                Port = int.Parse(_config["SMTP:Port"] ?? "587"),
                Credentials = new NetworkCredential(
                    _config["SMTP:User"],
                    _config["SMTP:Pass"]
                ),
                EnableSsl = true,
                Timeout = 30000
            };

            using var mail = new MailMessage
            {
                From = new MailAddress(_config["SMTP:User"] ?? "noreply@digesa.gob.pa", 
                    "DIGESA - Cannabis Medicinal"),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };

            mail.To.Add(destinatario);

            await smtp.SendMailAsync(mail);
            _logger.LogInformation($"Correo enviado exitosamente a: {destinatario}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error enviando correo a: {destinatario}");
            return false;
        }
    }
}