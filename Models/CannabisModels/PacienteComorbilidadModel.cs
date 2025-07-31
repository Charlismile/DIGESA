using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteComorbilidadModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
    public string? NombreDiagnostico { get; set; }
    public string? DetalleTratamiento { get; set; }
}