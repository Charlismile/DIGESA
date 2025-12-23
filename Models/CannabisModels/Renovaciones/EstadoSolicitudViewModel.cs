namespace DIGESA.Models.CannabisModels.Renovaciones;

public class EstadoSolicitudViewModel
{
    public int IdEstado { get; set; }
    public bool Activo { get; set; }

    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;

    public string Documento { get; set; } = string.Empty;

    public DateTime? FechaVencimiento { get; set; }
}