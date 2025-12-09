namespace DIGESA.Models.CannabisModels;

public class PacienteEstadoDetalleModel
{
    public string PacienteNombre { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string NumeroCarnet { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public int DiasDesdeVencimiento { get; set; }
}