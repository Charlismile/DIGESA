namespace DIGESA.Models.CannabisModels.Renovaciones;

public class SolicitudCannabisViewModel
{
    public int Id { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public int? PacienteId { get; set; }

    public DateTime? FechaRevision { get; set; }
    public string? UsuarioRevisor { get; set; }
    public string? ComentarioRevision { get; set; }

    public string? NumSolCompleta { get; set; }
    public string? CreadaPor { get; set; }
    public DateTime? FechaAprobacion { get; set; }

    public int EstadoSolicitudId { get; set; }
    public bool EsRenovacion { get; set; }

    public string? FotoCarnetUrl { get; set; }
    public string? FirmaDigitalUrl { get; set; }

    public bool CarnetActivo { get; set; }
    public string? NumeroCarnet { get; set; }

    public DateTime? FechaEmisionCarnet { get; set; }
    public DateTime? FechaVencimientoCarnet { get; set; }
    public DateTime? FechaUltimaRenovacion { get; set; }
}