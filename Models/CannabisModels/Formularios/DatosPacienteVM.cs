using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosPacienteVM : PersonaBaseViewModel
{
    [Required]
    public DateTime? FechaNacimiento { get; set; }

    [Required]
    public string Nacionalidad { get; set; }

    public bool RequiereAcompanante { get; set; }

    [StringLength(300)]
    public string MotivoAcompanante { get; set; }

    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    public int? CorregimientoId { get; set; }

    public string DireccionExacta { get; set; }
}
