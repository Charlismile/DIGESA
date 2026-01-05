using DIGESA.Models.CannabisModels.Formularios;

namespace DIGESA.Models.CannabisModels.Validadores;

public class PacienteValidator
{
    public List<string> Validate(DatosPacienteVM paciente)
    {
        var errors = new List<string>();

        if (paciente == null)
        {
            errors.Add("Los datos del paciente son obligatorios.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(paciente.PrimerNombre))
            errors.Add("El primer nombre del paciente es obligatorio.");

        return errors;
    }
}
