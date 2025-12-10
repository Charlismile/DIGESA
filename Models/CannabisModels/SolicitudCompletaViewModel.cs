using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class SolicitudCompletaViewModel : IValidatableObject
{
    // Datos del paciente
    [Required(ErrorMessage = "Los datos del paciente son requeridos")]
    public PacienteViewModel Paciente { get; set; }
    
    // Acompañante (si aplica)
    public AcompanantePacienteViewModel Acompanante { get; set; }
    
    // Médico tratante
    [Required(ErrorMessage = "Los datos del médico tratante son requeridos")]
    public MedicoPacienteViewModel Medico { get; set; }
    
    // Diagnósticos
    [Required(ErrorMessage = "Al menos un diagnóstico es requerido")]
    [MinLength(1, ErrorMessage = "Debe registrar al menos un diagnóstico")]
    public List<PacienteDiagnosticoViewModel> Diagnosticos { get; set; } = new List<PacienteDiagnosticoViewModel>();
    
    // Productos de cannabis
    [Required(ErrorMessage = "Debe especificar al menos un producto de cannabis")]
    [MinLength(1, ErrorMessage = "Debe registrar al menos un producto")]
    public List<ProductoPacienteViewModel> Productos { get; set; } = new List<ProductoPacienteViewModel>();
    
    // Declaración jurada
    [Required(ErrorMessage = "La declaración jurada es obligatoria")]
    public DeclaracionJuradaViewModel DeclaracionJurada { get; set; }
    
    // Documentos adjuntos
    [Required(ErrorMessage = "Debe adjuntar los documentos requeridos")]
    public List<DocumentoAdjuntoViewModel> Documentos { get; set; } = new List<DocumentoAdjuntoViewModel>();
    
    // Información de la solicitud
    public bool EsRenovacion { get; set; }
    public int? SolicitudAnteriorId { get; set; } // Para renovaciones
    
    // Validaciones personalizadas complejas
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Validar que el paciente sea mayor de edad o tenga acompañante
        if (Paciente.Edad < 18 && !Paciente.RequiereAcompanante)
        {
            yield return new ValidationResult(
                "Los pacientes menores de edad deben tener un acompañante autorizado",
                new[] { nameof(Paciente.RequiereAcompanante) });
        }
        
        // Validar que si requiere acompañante, los datos estén completos
        if (Paciente.RequiereAcompanante && (Acompanante == null || string.IsNullOrEmpty(Acompanante.NumeroDocumento)))
        {
            yield return new ValidationResult(
                "Debe proporcionar la información del acompañante",
                new[] { nameof(Acompanante) });
        }
        
        // Validar que la declaración jurada esté aceptada
        if (DeclaracionJurada != null && !DeclaracionJurada.Aceptada.GetValueOrDefault())
        {
            yield return new ValidationResult(
                "Debe aceptar la declaración jurada para continuar",
                new[] { nameof(DeclaracionJurada.Aceptada) });
        }
        
        // Validar documentos requeridos según tipo de solicitud
        var documentosRequeridos = EsRenovacion ? 3 : 5; // Ejemplo
        if (Documentos.Count < documentosRequeridos)
        {
            yield return new ValidationResult(
                $"Debe adjuntar al menos {documentosRequeridos} documentos",
                new[] { nameof(Documentos) });
        }
        
        // Validar que los productos tengan información completa
        foreach (var producto in Productos)
        {
            if (string.IsNullOrEmpty(producto.NombreProducto) || 
                string.IsNullOrEmpty(producto.DetDosisPaciente))
            {
                yield return new ValidationResult(
                    "Todos los productos deben tener nombre y dosis especificados",
                    new[] { nameof(Productos) });
            }
        }
    }
}