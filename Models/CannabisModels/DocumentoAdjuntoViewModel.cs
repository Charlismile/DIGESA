using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DocumentoAdjuntoViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La solicitud es requerida")]
    public int SolRegCannabisId { get; set; }
    
    [Required(ErrorMessage = "El tipo de documento es requerido")]
    public int TipoDocumentoId { get; set; }
    
    [StringLength(200)]
    public string NombreOriginal { get; set; }
    
    [StringLength(200)]
    public string NombreGuardado { get; set; }
    
    [StringLength(300)]
    public string Url { get; set; }
    
    public DateTime? FechaSubidaUtc { get; set; }
    
    [StringLength(100)]
    public string SubidoPor { get; set; }
    
    public bool? IsValido { get; set; }
    
    public bool EsDocumentoMedico { get; set; }
    
    public int? MedicoId { get; set; }
    
    [StringLength(100)]
    public string Categoria { get; set; }
    
    public int? Version { get; set; }
    
    // Propiedades de navegación
    public TipoDocumentoAdjuntoViewModel TipoDocumento { get; set; }
    
    // Propiedades para UI
    public IFormFile Archivo { get; set; }
    public string Extension => Path.GetExtension(NombreOriginal);
    public string TamañoFormateado { get; set; }
}