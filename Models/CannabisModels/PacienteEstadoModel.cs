namespace DIGESA.Models.CannabisModels;

public class PacienteEstadoModel
{
    public bool Activo { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Documento { get; set; }
}