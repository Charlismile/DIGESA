using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteDiagnostico
{
    public int Id { get; set; }
    public int? PacienteId { get; set; }
    [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
    public string? NombreDiagnostico { get; set; }
}