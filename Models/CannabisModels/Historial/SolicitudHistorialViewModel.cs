using DIGESA.Models.CannabisModels.Renovaciones;

namespace DIGESA.Models.CannabisModels.Historial;

public class SolicitudHistorialViewModel
{
    public int Id { get; set; }
    public int? SolRegCannabisId { get; set; }
    public string? Comentario { get; set; }
    public string? UsuarioRevisor { get; set; }
    public DateTime? FechaCambio { get; set; }
    public int EstadoSolicitudIdHistorial { get; set; }
    public EstadoSolicitudViewModel? EstadoSolicitud { get; set; }
}