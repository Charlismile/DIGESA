using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using DIGESA.Models.CannabisModels.Configuracion;
using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class EmailSenderService : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailSenderService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string bodyHtml)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(_settings.FromEmail, _settings.FromName),
                Subject = subject,
                Body = bodyHtml,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            using var smtp = new SmtpClient(_settings.SmtpServer, _settings.SmtpPort)
            {
                EnableSsl = _settings.EnableSsl,
                Credentials = new NetworkCredential(
                    _settings.SmtpUsername,
                    _settings.SmtpPassword)
            };

            await smtp.SendMailAsync(mail);
        }
    }
}