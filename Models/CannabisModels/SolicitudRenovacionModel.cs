using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class SolicitudRenovacionModel
{
    public int Id { get; set; }
    
    [Required]
    public int SolicitudOriginalId { get; set; }
    
    [Required]
    public int PacienteId { get; set; }
    
    [Required]
    public DateTime FechaSolicitud { get; set; } = DateTime.Now;
    
    [Required]
    public EstadoSolicitud Estado { get; set; } = EstadoSolicitud.Pendiente;
    
    public string? NumeroCarnetAnterior { get; set; }
    public DateTime? FechaVencimientoAnterior { get; set; }
    
    public bool DatosPersonalesCambiados { get; set; } = false;
    public bool DiagnosticoCambiado { get; set; } = false;
    public bool ProductoCambiado { get; set; } = false;
    
    public string? ComentarioRenovacion { get; set; }
    public DateTime? FechaAprobacionRenovacion { get; set; }
    
    // Propiedades de navegación
    public SolicitudBaseModel? SolicitudOriginal { get; set; }
    public PacienteModel? Paciente { get; set; }
    
    // Método para determinar si necesita nueva declaración
    public bool RequiereNuevaDeclaracion()
    {
        return DatosPersonalesCambiados || DiagnosticoCambiado || ProductoCambiado;
    }
}