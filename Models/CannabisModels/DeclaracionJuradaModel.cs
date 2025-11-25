using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DeclaracionJuradaModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El detalle es obligatorio")]
    public string Detalle { get; set; } = string.Empty;
    
    public DateTime Fecha { get; set; } = DateTime.Now;
    
    [Required(ErrorMessage = "El nombre del declarante es obligatorio")]
    public string NombreDeclarante { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Debe aceptar la declaración jurada")]
    public bool Aceptada { get; set; }
}