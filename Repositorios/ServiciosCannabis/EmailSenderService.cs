using System.Net;
using System.Net.Mail;
using DIGESA.Models.CannabisModels;
using Microsoft.Extensions.Options;
using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class EmailSenderService : IEmailSender
    { 
        private readonly MailSettings _settings;
        
        public EmailSenderService(IOptions<MailSettings> options)
        {
            _settings = options.Value;
        }
        
        public async Task SendEmailAsync(string to, string subject, string bodyHtml)
        {
            var mail = new MailMessage()
            {
                From = new MailAddress(
                    _settings.UserName,
                    "DIGESA - Sistema de Cannabis Medicinal",
                    System.Text.Encoding.UTF8),
                Subject = subject,
                Body = bodyHtml,
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8,
            };

            mail.To.Add(to);

            using var smtp = new SmtpClient
            {
                Host = _settings.Host,
                Port = _settings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    _settings.UserName,
                    _settings.Password),
                Timeout = 20000
            };

            try
            {
                await smtp.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error enviando email: {ex.Message}", ex);
            }
        }
    }
}