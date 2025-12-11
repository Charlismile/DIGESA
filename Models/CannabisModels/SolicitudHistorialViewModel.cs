using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class SolicitudHistorialViewModel
{
    public int Id { get; set; }
    
    public int? SolRegCannabisId { get; set; }
    
    [StringLength(300, ErrorMessage = "El comentario no puede exceder 300 caracteres")]
    public string Comentario { get; set; }
    
    [StringLength(100)]
    public string UsuarioRevisor { get; set; }
    
    public DateTime? FechaCambio { get; set; }
    
    public int? EstadoSolicitudIdHistorial { get; set; }
    
    // Propiedades de navegación
    public SolicitudCannabisViewModel Solicitud { get; set; }
    public EstadoSolicitudViewModel EstadoSolicitud { get; set; }
    
    // Propiedades calculadas
    public string EstadoNombre => EstadoSolicitud?.NombreEstado;
    public string FechaFormateada => FechaCambio?.ToString("dd/MM/yyyy HH:mm");
    
    // Para la línea de tiempo
    public string TipoEvento => "Cambio de Estado";
    public string Icono => EstadoNombre switch
    {
        "Aprobado" => "check-circle",
        "Rechazado" => "x-circle",
        "Pendiente" => "clock",
        "En Revisión" => "search",
        _ => "file-text"
    };
    
    public string Color => EstadoNombre switch
    {
        "Aprobado" => "success",
        "Rechazado" => "danger",
        "Pendiente" => "warning",
        "En Revisión" => "info",
        _ => "secondary"
    };
}