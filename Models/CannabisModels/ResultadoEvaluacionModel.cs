namespace DIGESA.Models.CannabisModels;

public class ResultadoEvaluacionModel
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string EmailEnviado { get; set; } = string.Empty;
    public DateTime FechaEvaluacion { get; set; }
}