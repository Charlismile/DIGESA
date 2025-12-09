namespace DIGESA.Models.CannabisModels;

public class CarnetProximoInactivacionModel
{
    public int Id { get; set; }
    public string NumeroCarnet { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public DateTime ProximaInactivacion { get; set; }
    public int DiasParaInactivacion { get; set; }
    public bool Notificado { get; set; }
}