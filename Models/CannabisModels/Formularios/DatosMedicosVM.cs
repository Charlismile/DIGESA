using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosMedicosVM
{
    [Required]
    public int MedicoId { get; set; }

    [Required]
    public int DiagnosticoId { get; set; }

    [StringLength(300)]
    public string DetalleTratamiento { get; set; }

    public DateTime FechaDiagnostico { get; set; } = DateTime.Now;
}
