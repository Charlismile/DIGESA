namespace DIGESA.Models.CannabisModels;

public class CarnetProximoVencerModel
{
    public string NumeroCarnet { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public string PacienteCorreo { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public int DiasRestantes { get; set; }
    public bool Notificado30Dias { get; set; }
    public bool Notificado15Dias { get; set; }
    public bool Notificado7Dias { get; set; }
    public DateTime? UltimaNotificacion { get; set; }
}