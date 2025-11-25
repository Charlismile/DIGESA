namespace DIGESA.Models.CannabisModels;

// Modelo base para resultados
public class ResultModel
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errores { get; set; } = new();
}

// Modelo genérico para resultados con datos
public class ResultModel<T> : ResultModel
{
    public T? Data { get; set; }
}

// Modelo específico para evaluación
public class ResultadoEvaluacionModel : ResultModel
{
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string? EmailDestinatario { get; set; } // Renombrado
    public DateTime FechaEvaluacion { get; set; } = DateTime.Now;
}