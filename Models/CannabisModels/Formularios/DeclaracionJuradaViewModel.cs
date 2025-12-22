using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels.Formularios;

public class DeclaracionJuradaViewModel
{
    [Required]
    public bool AceptaDeclaracion { get; set; }

    [Required]
    [StringLength(150)]
    public string NombreDeclarante { get; set; }

    [StringLength(300)]
    public string Detalle { get; set; }

    // Solo para UI / control
    public DateTime FechaDeclaracion { get; set; } = DateTime.Now;
}