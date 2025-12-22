namespace DIGESA.Models.CannabisModels.Historial;

public class HistorialRenovacionViewModel
{
    public int Id { get; set; }
    public int SolicitudAnteriorId { get; set; }
    public int SolicitudNuevaId { get; set; }
    public DateTime FechaRenovacion { get; set; }
    public string? RazonRenovacion { get; set; }
    public string? UsuarioRenovador { get; set; }
    public string? Comentarios { get; set; }
}