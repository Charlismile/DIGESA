using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DeclaracionJuradaModel
{
    public int Id { get; set; }
    
    [Required]
    public int SolicitudId { get; set; }
    
    [Required]
    [StringLength(300)]
    public string Detalle { get; set; } = string.Empty;
    
    [Required]
    public DateTime Fecha { get; set; } = DateTime.Now;
    
    [Required]
    [StringLength(150)]
    public string NombreDeclarante { get; set; } = string.Empty;
    
    [Required]
    public bool Aceptada { get; set; } = false;
    
    [Required]
    public string DocumentoIdentidad { get; set; } = string.Empty;
    
    public string? IpAceptacion { get; set; }
    public DateTime? FechaAceptacion { get; set; }
    
    // Propiedades de navegación
    public SolicitudBaseModel? Solicitud { get; set; }
}