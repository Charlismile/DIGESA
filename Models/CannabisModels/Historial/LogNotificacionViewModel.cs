namespace DIGESA.Models.CannabisModels.Historial;

public class LogNotificacionViewModel
{
    public int Id { get; set; }
    public int SolicitudId { get; set; }
    public string TipoNotificacion { get; set; }
    public DateTime FechaEnvio { get; set; }
    public string MetodoEnvio { get; set; }
    public string Destinatario { get; set; }
    public string Estado { get; set; }
    public string Error { get; set; }
}