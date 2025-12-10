using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ExportacionViewModel
{
    [Required(ErrorMessage = "El formato de exportación es requerido")]
    public string Formato { get; set; } // "Excel", "PDF"
    
    [Required(ErrorMessage = "El tipo de reporte es requerido")]
    public string TipoReporte { get; set; } // "Pacientes", "Solicitudes", "Carnets", "Estadisticas"
    
    // Filtros
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string Estado { get; set; }
    public string TipoSolicitud { get; set; }
    public int? ProvinciaId { get; set; }
    public int? RegionSaludId { get; set; }
    
    // Columnas a incluir (para personalización)
    public List<string> ColumnasIncluidas { get; set; } = new List<string>();
    
    // Opciones de formato
    public bool IncluirEncabezado { get; set; } = true;
    public bool IncluirFirmas { get; set; } = false;
    public bool IncluirLogos { get; set; } = true;
    public string Orientacion { get; set; } = "Vertical"; // "Vertical" o "Horizontal"
    
    // Validación personalizada
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (FechaInicio.HasValue && FechaFin.HasValue && FechaInicio > FechaFin)
        {
            yield return new ValidationResult(
                "La fecha de inicio no puede ser mayor a la fecha fin",
                new[] { nameof(FechaInicio), nameof(FechaFin) });
        }
        
        if (FechaInicio.HasValue && FechaInicio.Value.Year < 2020)
        {
            yield return new ValidationResult(
                "No se pueden exportar registros anteriores al año 2020",
                new[] { nameof(FechaInicio) });
        }
    }
}