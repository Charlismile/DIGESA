namespace DIGESA.Models.CannabisModels.Reportes;

public class NotificationRequest
{
    public EnumViewModel.NotificationType Tipo { get; set; }
    public int SolicitudId { get; set; }
    public string EmailDestino { get; set; } = string.Empty;

    // Datos dinámicos
    public int? DiasRestantes { get; set; }
    public string? Razon { get; set; }
    public string? Asunto { get; set; }
    public string? Cuerpo { get; set; }
}