using System.Net;
using System.Net.Mail;
using DIGESA.Models.CannabisModels.Configuracion;
using Microsoft.Extensions.Options;
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
                From = new MailAddress(
                    _settings.FromEmail,
                    _settings.FromName,
                    System.Text.Encoding.UTF8),

                Subject = subject,
                Body = bodyHtml,
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8
            };

            mail.To.Add(to);

            using var smtp = new SmtpClient
            {
                Host = _settings.SmtpServer,
                Port = _settings.SmtpPort,
                EnableSsl = _settings.EnableSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    _settings.SmtpUsername,
                    _settings.SmtpPassword),
                Timeout = 20000
            };

            await smtp.SendMailAsync(mail);
        }
    }
}