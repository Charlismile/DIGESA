using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ValidacionPacienteViewModel : IValidatableObject
{
    public PacienteViewModel Paciente { get; set; }
    public List<AcompanantePacienteViewModel> Acompanantes { get; set; }
    public List<PacienteDiagnosticoViewModel> Diagnosticos { get; set; }
    public bool EsRenovacion { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Validar edad mínima
        if (Paciente.Edad < 1)
        {
            yield return new ValidationResult("El paciente debe tener al menos 1 año", 
                new[] { nameof(Paciente.FechaNacimiento) });
        }
        
        // Validar que pacientes menores de 18 tengan acompañante
        if (Paciente.Edad < 18 && (!Paciente.RequiereAcompanante || Acompanantes.Count == 0))
        {
            yield return new ValidationResult(
                "Los pacientes menores de 18 años deben tener al menos un acompañante autorizado",
                new[] { nameof(Paciente.RequiereAcompanante) });
        }
        
        // Validar diagnósticos aprobados para cannabis
        var diagnosticosNoAprobados = Diagnosticos
            .Where(d => !d.Diagnostico?.EsAprobadoParaCannabis ?? false)
            .ToList();
            
        if (diagnosticosNoAprobados.Any())
        {
            yield return new ValidationResult(
                $"Los siguientes diagnósticos no están aprobados para cannabis medicinal: " +
                $"{string.Join(", ", diagnosticosNoAprobados.Select(d => d.NombreDiagnostico))}",
                new[] { nameof(Diagnosticos) });
        }
        
        // Validar renovaciones (no puede renovar antes de tiempo)
        if (EsRenovacion)
        {
            // Aquí iría la lógica para validar si puede renovar
            // Por ejemplo, no puede renovar si el carnet actual tiene más de 6 meses de vigencia restante
        }
    }
}