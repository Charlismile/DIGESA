using DIGESA.Models.CannabisModels.Formularios;
using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Models.CannabisModels.Validadores;

public class MedicoValidator : IValidator<DatosMedicosVM>
{
    public List<string> Validate(DatosMedicosVM medico)
    {
        var errors = new List<string>();

        if (medico == null)
        {
            errors.Add("Los datos del médico son obligatorios.");
            return errors;
        }

        if (string.IsNullOrWhiteSpace(medico.PrimerNombre))
            errors.Add("El nombre del médico es obligatorio.");

        if (string.IsNullOrWhiteSpace(medico.PrimerApellido))
            errors.Add("El apellido del médico es obligatorio.");

        if (string.IsNullOrWhiteSpace(medico.MedicoIdoneidad))
            errors.Add("La idoneidad del médico es obligatoria.");

        if (medico.RegionSaludId == null)
            errors.Add("La región de salud del médico es obligatoria.");

        if (medico.InstalacionSaludId == null &&
            string.IsNullOrWhiteSpace(medico.InstalacionSaludPersonalizada))
        {
            errors.Add("Debe indicar la instalación de salud del médico.");
        }

        return errors;
    }
}