using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class SolicitudCannabisViewModel
{
    public int Id { get; set; }
    
    public DateTime FechaSolicitud { get; set; }
    
    [Required(ErrorMessage = "El paciente es requerido")]
    public int PacienteId { get; set; }
    
    public DateTime? FechaRevision { get; set; }
    
    [StringLength(100)]
    public string UsuarioRevisor { get; set; }
    
    [StringLength(300)]
    public string ComentarioRevision { get; set; }
    
    public int? NumSolSecuencia { get; set; }
    public int? NumSolAnio { get; set; }
    public int? NumSolMes { get; set; }
    
    [StringLength(100)]
    public string NumSolCompleta { get; set; }
    
    [StringLength(100)]
    public string CreadaPor { get; set; }
    
    public DateTime? ModificadaEn { get; set; }
    
    [StringLength(100)]
    public string ModificadaPor { get; set; }
    
    public DateTime? FechaAprobacion { get; set; }
    
    [Required(ErrorMessage = "El estado de solicitud es requerido")]
    public int EstadoSolicitudId { get; set; }
    
    public bool EsRenovacion { get; set; }
    
    [StringLength(500)]
    public string FotoCarnetUrl { get; set; }
    
    [StringLength(500)]
    public string FirmaDigitalUrl { get; set; }
    
    public bool CarnetActivo { get; set; }
    
    [StringLength(50)]
    public string NumeroCarnet { get; set; }
    
    public DateTime? FechaEmisionCarnet { get; set; }
    public DateTime? FechaVencimientoCarnet { get; set; }
    public DateTime? FechaUltimaRenovacion { get; set; }
    
    // Propiedades de navegación
    public PacienteViewModel Paciente { get; set; }
    public EstadoSolicitudViewModel EstadoSolicitud { get; set; }
    
    // Propiedades calculadas
    public string EstadoNombre => EstadoSolicitud?.NombreEstado;
    public string TipoSolicitud => EsRenovacion ? "Renovación" : "Primera Vez";
    
    // Para la declaración jurada
    public DeclaracionJuradaViewModel DeclaracionJurada { get; set; }
    
    // Para documentos adjuntos
    public List<DocumentoAdjuntoViewModel> DocumentosAdjuntos { get; set; }
    
    // Para notificaciones de vencimiento
    public bool EstaPorVencer => FechaVencimientoCarnet.HasValue && 
        (FechaVencimientoCarnet.Value - DateTime.Now).TotalDays <= 30;
    
    public int DiasParaVencimiento => FechaVencimientoCarnet.HasValue ? 
        (int)(FechaVencimientoCarnet.Value - DateTime.Now).TotalDays : 0;
}