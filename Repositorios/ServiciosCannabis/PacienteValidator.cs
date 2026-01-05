using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Formularios;
using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Repositorios.ServiciosCannabis;

public class PacienteValidator : IValidator<DatosPacienteVM>
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

        if (string.IsNullOrWhiteSpace(paciente.PrimerApellido))
            errors.Add("El primer apellido del paciente es obligatorio.");

        if (string.IsNullOrWhiteSpace(paciente.NumeroDocumento))
            errors.Add("El número de documento del paciente es obligatorio.");

        if (paciente.ProvinciaId == null)
            errors.Add("La provincia del paciente es obligatoria.");

        if (paciente.DistritoId == null)
            errors.Add("El distrito del paciente es obligatorio.");

        if (paciente.RegionSaludId == null)
            errors.Add("La región de salud del paciente es obligatoria.");

        if (paciente.InstalacionSaludId == null &&
            string.IsNullOrWhiteSpace(paciente.InstalacionSaludPersonalizada))
        {
            errors.Add("Debe indicar la instalación de salud del paciente.");
        }

        if (paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si &&
            paciente.MotivoRequerimientoAcompanante == null)
        {
            errors.Add("Debe indicar el motivo del acompañante.");
        }

        return errors;
    }
}