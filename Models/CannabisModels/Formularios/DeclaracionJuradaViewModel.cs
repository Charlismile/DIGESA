using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels.Formularios;

public class DeclaracionJuradaViewModel
{
    [Required(ErrorMessage = "Debe aceptar la declaración jurada")]
    public bool Aceptada { get; set; }  // Cambiar de AceptaDeclaracion a Aceptada

    [Required(ErrorMessage = "El nombre del declarante es requerido")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
    public string NombreDeclarante { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "El detalle no puede exceder 300 caracteres")]
    public string Detalle { get; set; } = "Declaración jurada de veracidad de información";

    // Solo para UI / control
    public DateTime FechaDeclaracion { get; set; } = DateTime.Now;
}