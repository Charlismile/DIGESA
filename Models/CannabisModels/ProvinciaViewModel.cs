using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ProvinciaViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre de la provincia es requerido")]
    [StringLength(150)]
    public string NombreProvincia { get; set; }
    
    // Para cascada en UI
    public List<DistritoViewModel> Distritos { get; set; }
}