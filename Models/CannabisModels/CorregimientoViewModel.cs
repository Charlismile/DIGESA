using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class CorregimientoViewModel
{
    public int Id { get; set; }
    
    [StringLength(150)]
    public string NombreCorregimiento { get; set; }
    
    public int? DistritoId { get; set; }
}