using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ReporteViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre del archivo es requerido")]
    [StringLength(200)]
    public string NombreArchivo { get; set; }
    
    [Required(ErrorMessage = "El tipo de reporte es requerido")]
    [StringLength(50)]
    public string TipoReporte { get; set; }
    
    public DateTime? FechaGeneracion { get; set; }
    
    [StringLength(450)]
    public string GeneradoPor { get; set; }
    
    public string FiltrosAplicados { get; set; }
    
    [StringLength(500)]
    public string RutaArchivo { get; set; }
    
    public long? TamanoBytes { get; set; }
    public bool? Descargado { get; set; }
    
    // Propiedades para filtros
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string Estado { get; set; }
    public string TipoSolicitud { get; set; }
    public int? ProvinciaId { get; set; }
    
    // Formatos de exportación
    public string FormatoExportacion { get; set; } // "Excel" o "PDF"
}