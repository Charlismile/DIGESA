using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class FarmaciaAutorizadaViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El código de farmacia es requerido")]
    [StringLength(50)]
    public string CodigoFarmacia { get; set; }
    
    [Required(ErrorMessage = "El nombre de la farmacia es requerido")]
    [StringLength(200)]
    public string NombreFarmacia { get; set; }
    
    [Required(ErrorMessage = "El RUC es requerido")]
    [StringLength(50)]
    public string RUC { get; set; }
    
    [Required(ErrorMessage = "La dirección es requerida")]
    [StringLength(300)]
    public string Direccion { get; set; }
    
    [Required(ErrorMessage = "La provincia es requerida")]
    public int ProvinciaId { get; set; }
    
    [Required(ErrorMessage = "El distrito es requerido")]
    public int DistritoId { get; set; }
    
    [Phone(ErrorMessage = "Formato de teléfono inválido")]
    [StringLength(15)]
    public string Telefono { get; set; }
    
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [StringLength(200)]
    public string Email { get; set; }
    
    [StringLength(200)]
    public string Responsable { get; set; }
    
    [Required(ErrorMessage = "La fecha de autorización es requerida")]
    public DateTime FechaAutorizacion { get; set; }
    
    public DateTime? FechaVencimientoAutorizacion { get; set; }
    
    public bool Activo { get; set; } = true;
    
    [StringLength(450)]
    public string UsuarioRegistro { get; set; }
    
    public DateTime FechaRegistro { get; set; }
    
    // Propiedades de navegación
    public ProvinciaViewModel Provincia { get; set; }
    public DistritoViewModel Distrito { get; set; }
    
    // Propiedades calculadas
    public bool AutorizacionVigente => Activo && 
        (!FechaVencimientoAutorizacion.HasValue || 
         FechaVencimientoAutorizacion.Value > DateTime.Now);
    
    public string DireccionCompleta => $"{Direccion}, {Distrito?.NombreDistrito}, {Provincia?.NombreProvincia}";
    
    // Para auditoría
    public int TotalDispensaciones { get; set; }
    public DateTime? UltimaDispensacion { get; set; }
}