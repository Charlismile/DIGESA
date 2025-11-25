namespace DIGESA.Models.CannabisModels;

public class EmailNotificacionModel
{
    public string Destinatario { get; set; } = string.Empty;
    public string Asunto { get; set; } = string.Empty;
    public string Cuerpo { get; set; } = string.Empty;
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string NombrePaciente { get; set; } = string.Empty;
    public string EstadoSolicitud { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
}