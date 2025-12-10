using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class InstalacionSaludViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre de la instalación es requerido")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
    public string Nombre { get; set; }
    
    // Información adicional
    public string TipoInstalacion { get; set; } // "Hospital", "Centro de Salud", "Clínica"
    public string Direccion { get; set; }
    public string Telefono { get; set; }
    public int? RegionId { get; set; }
    
    public RegionSaludViewModel Region { get; set; }
}