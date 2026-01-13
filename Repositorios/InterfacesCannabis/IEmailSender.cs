namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string bodyHtml);
}