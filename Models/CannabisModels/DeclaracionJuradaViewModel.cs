using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DeclaracionJuradaViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La solicitud es requerida")]
    public int? SolRegCannabisId { get; set; }
    
    [Required(ErrorMessage = "El detalle es obligatorio")]
    [StringLength(300)]
    public string Detalle { get; set; }
    
    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime? Fecha { get; set; }
    
    [Required(ErrorMessage = "El nombre del declarante es obligatorio")]
    [StringLength(150)]
    public string NombreDeclarante { get; set; }
    
    [Required(ErrorMessage = "Debe aceptar la declaración jurada")]
    public bool? Aceptada { get; set; }
    
    // Texto fijo de la declaración jurada
    public string TextoDeclaracionJurada => "El ministerio de salud no se hace responsable del uso que el paciente le dé al producto medicinal de cannabis ni de los efectos que estos puedan provocar.";
}