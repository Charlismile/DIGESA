using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DatosAcompananteVM : PersonaBaseViewModel
{
    [Required]
    public string Parentesco { get; set; }

    [Required]
    public string TelefonoMovil { get; set; }
}
