using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DiagnosticoViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre del diagnóstico es requerido")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
    public string Nombre { get; set; }
    
    public bool IsActivo { get; set; } = true;
    
    // Para diagnósticos específicos de cannabis medicinal
    public string Categoria { get; set; } // Ej: "Oncología", "Neurología", "Dolor Crónico"
    public string CodigoCIE10 { get; set; }
    
    public bool EsAprobadoParaCannabis { get; set; } = true;
}