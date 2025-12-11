namespace DIGESA.Repositorios.InterfacesCannabis;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string bodyHtml);
}