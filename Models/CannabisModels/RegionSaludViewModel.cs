using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class RegionSaludViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre de la región de salud es requerido")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
    public string Nombre { get; set; }
    
    // Información adicional
    public string CodigoRegion { get; set; }
    public string TelefonoContacto { get; set; }
    public string EmailContacto { get; set; }
    
    // Para cascada en UI
    public List<InstalacionSaludViewModel> Instalaciones { get; set; }
}