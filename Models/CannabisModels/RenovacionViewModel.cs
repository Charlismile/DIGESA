using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class RenovacionViewModel
{
    public int SolicitudId { get; set; }
    public int PacienteId { get; set; }
    
    [Required(ErrorMessage = "La fecha de renovación es requerida")]
    public DateTime FechaRenovacion { get; set; }
    
    public bool RequiereNuevaDeclaracionJurada { get; set; }
    public bool RequiereNuevosDocumentos { get; set; }
    
    // Información del paciente para mostrar
    public string NombrePaciente { get; set; }
    public string NumeroCarnetActual { get; set; }
    public DateTime FechaVencimientoActual { get; set; }
    
    // Nueva información (si aplica)
    public IFormFile NuevaFoto { get; set; }
    public IFormFile NuevaFirma { get; set; }
    
    // Lista de documentos requeridos para renovación
    public List<TipoDocumentoAdjuntoViewModel> DocumentosRequeridos { get; set; }
}