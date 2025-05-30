using DIGESA.Models.Entities.DIGESA;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

public class CorreoService
{
    private readonly IConfiguration _config;
    private readonly PdfCarnetService _pdfService;
    private readonly DbContextDigesa _db;

    public CorreoService(IConfiguration config, PdfCarnetService pdfService, DbContextDigesa db)
    {
        _config = config;
        _pdfService = pdfService;
        _db = db;
    }

    public async Task EnviarCarnetAsync(string correoPaciente, byte[] pdfBytes)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("DIGESA Panamá", _config["Correo:Remitente"]));
        message.To.Add(MailboxAddress.Parse(correoPaciente));
        message.Subject = "Carnet de Autorización - Uso de Cannabis Medicinal";

        var builder = new BodyBuilder
        {
            HtmlBody = @"
                <p>Estimado/a paciente,</p>
                <p>Adjunto encontrará su carnet digital para el uso de cannabis medicinal autorizado por el Ministerio de Salud.</p>
                <p>Por favor, conserve este documento. En caso de tener acompañante, también se incluye su carnet.</p>
                <p>Saludos cordiales,<br/>DIGESA Panamá</p>"
        };

        builder.Attachments.Add("CarnetDIGESA.pdf", pdfBytes, new ContentType("application", "pdf"));
        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["Correo:Servidor"], int.Parse(_config["Correo:Puerto"]), true);
        await smtp.AuthenticateAsync(_config["Correo:Usuario"], _config["Correo:Clave"]);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}