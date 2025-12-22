namespace DIGESA.Models.CannabisModels.Renovaciones;

public class RenovacionDetalleViewModel
{
    public int SolicitudId { get; set; }
    public string? NumeroCarnet { get; set; }
    public string? Paciente { get; set; }
    public DateTime FechaRenovacion { get; set; }
    public string? Estado { get; set; }
    public string? Usuario { get; set; }
    public string? Comentarios { get; set; }
}