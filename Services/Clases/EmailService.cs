namespace DIGESA.Services.Clases;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task EnviarCorreoConQRAsync(string destinatario, string asunto, string mensaje, byte[] qrBytes)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Portal Médico", _config["SmtpSettings:FromEmail"]));
        email.To.Add(new MailboxAddress("", destinatario));
        email.Subject = asunto;

        var body = new TextPart("html") { Text = mensaje };
        var attachment = new MimePart("image", "png")
        {
            Content = new MimeContent(new MemoryStream(qrBytes)),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            FileName = "CertificacionQR.png"
        };

        var multipart = new Multipart("mixed") { body, attachment };
        email.Body = multipart;

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["SmtpSettings:Server"], int.Parse(_config["SmtpSettings:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
