using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class HistorialRenovacionViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La solicitud anterior es requerida")]
    public int SolicitudAnteriorId { get; set; }
    
    [Required(ErrorMessage = "La nueva solicitud es requerida")]
    public int SolicitudNuevaId { get; set; }
    
    [Required(ErrorMessage = "La fecha de renovación es requerida")]
    public DateTime FechaRenovacion { get; set; }
    
    [StringLength(200, ErrorMessage = "La razón no puede exceder 200 caracteres")]
    public string RazonRenovacion { get; set; }
    
    [StringLength(450)]
    public string UsuarioRenovador { get; set; }
    
    [StringLength(500, ErrorMessage = "Los comentarios no pueden exceder 500 caracteres")]
    public string Comentarios { get; set; }
    
    // Propiedades de navegación
    public SolicitudCannabisViewModel SolicitudAnterior { get; set; }
    public SolicitudCannabisViewModel SolicitudNueva { get; set; }
    
    // Propiedades calculadas
    public string NumeroCarnetAnterior => SolicitudAnterior?.NumeroCarnet;
    public string NumeroCarnetNuevo => SolicitudNueva?.NumeroCarnet;
    public string NombrePaciente => SolicitudAnterior?.Paciente?.NombreCompleto;
}