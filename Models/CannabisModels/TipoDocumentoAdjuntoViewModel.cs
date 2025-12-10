using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class TipoDocumentoAdjuntoViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(200)]
    public string Nombre { get; set; }
    
    [StringLength(300)]
    public string Descripcion { get; set; }
    
    public bool? EsRequisitoParaPaciente { get; set; }
    public bool? EsRequisitoParaAcompanante { get; set; }
    public bool? EsParaMenorEdad { get; set; }
    public bool? EsParaMayorEdad { get; set; }
    public bool? EsParaRenovacion { get; set; }
    public bool? EsObligatorio { get; set; }
    public bool? EsSoloMedico { get; set; }
    public bool? IsActivo { get; set; }
}