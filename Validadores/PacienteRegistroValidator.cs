using DIGESA.Models.DTOs;
using FluentValidation;

namespace DIGESA.Validators
{
    public class PacienteRegistroValidator : AbstractValidator<PacienteRegistroDTO>
    {
        public PacienteRegistroValidator()
        {
            RuleFor(x => x.NombreCompleto).NotEmpty().WithMessage("El nombre completo es requerido.");
            RuleFor(x => x.TipoDocumento).NotEmpty().WithMessage("Selecciona tipo de documento.");
            RuleFor(x => x.NumeroDocumento).NotEmpty().When(x => !string.IsNullOrEmpty(x.TipoDocumento))
                .WithMessage("Número de documento es obligatorio.");

            RuleFor(x => x.Nacionalidad).NotEmpty().WithMessage("La nacionalidad es requerida.");
            RuleFor(x => x.FechaNacimiento).NotEmpty().WithMessage("Fecha de nacimiento es requerida.");
            RuleFor(x => x.Sexo).NotEmpty().WithMessage("El sexo es requerido.");
            RuleFor(x => x.DireccionResidencia).NotEmpty().WithMessage("Dirección residencia es obligatoria.");
            RuleFor(x => x.InstalacionSalud).NotEmpty().WithMessage("Instalación de salud es requerida.");
            RuleFor(x => x.RegionSalud).NotEmpty().WithMessage("Región de salud es requerida.");

            When(x => x.RequiereAcompanante, () =>
            {
                RuleFor(x => x.MotivoRequerimientoAcompanante)
                    .NotEmpty().WithMessage("Motivo de acompañante es obligatorio.");

                RuleFor(x => x.Acompanante.NombreCompleto)
                    .NotEmpty().When(x => x.RequiereAcompanante)
                    .WithMessage("Nombre del acompañante es obligatorio.");

                RuleFor(x => x.Acompanante.Parentesco)
                    .NotEmpty().When(x => x.RequiereAcompanante)
                    .WithMessage("Parentesco es obligatorio.");
            });
        }
    }
}