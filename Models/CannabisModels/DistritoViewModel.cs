using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DistritoViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre del distrito es requerido")]
    [StringLength(150)]
    public string NombreDistrito { get; set; }
    
    public int? ProvinciaId { get; set; }
    
    // Para cascada en UI
    public List<CorregimientoViewModel> Corregimientos { get; set; }
}