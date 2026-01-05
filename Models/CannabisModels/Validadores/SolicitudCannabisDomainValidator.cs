using DIGESA.Models.CannabisModels.Formularios;

namespace DIGESA.Models.CannabisModels.Validadores;

public class SolicitudCannabisDomainValidator
{
    private readonly PacienteValidator _pacienteValidator = new();
    private readonly MedicoValidator _medicoValidator = new();
    private readonly ProductoValidator _productoValidator = new();

    public List<string> Validate(SolicitudCannabisFormViewModel model)
    {
        var errors = new List<string>();

        if (model == null)
        {
            errors.Add("El formulario es inválido.");
            return errors;
        }

        errors.AddRange(_pacienteValidator.Validate(model.Paciente));
        errors.AddRange(_medicoValidator.Validate(model.Medico));
        errors.AddRange(_productoValidator.Validate(model.Producto));

        if (model.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si
            && model.Acompanante == null)
        {
            errors.Add("Debe registrar un acompañante autorizado.");
        }

        if (!model.Diagnostico.SelectedDiagnosticosIds.Any() &&
            string.IsNullOrWhiteSpace(model.Diagnostico.NombreOtroDiagnostico))
        {
            errors.Add("Debe indicar al menos un diagnóstico.");
        }

        return errors;
    }
}