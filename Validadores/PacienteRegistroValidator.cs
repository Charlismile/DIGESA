using DIGESA.Models.DTOs;
using FluentValidation;

public class PacienteRegistroValidator : AbstractValidator<PacienteRegistroDTO>
{
    public PacienteRegistroValidator()
    {
        RuleFor(x => x.NombreCompleto).NotEmpty().WithMessage("El nombre es requerido.");
        RuleFor(x => x.TipoDocumento).NotEmpty().WithMessage("Tipo de documento es obligatorio.");
        RuleFor(x => x.NumeroDocumento).NotEmpty().When(x => !string.IsNullOrEmpty(x.TipoDocumento))
            .WithMessage("Número de documento es obligatorio.");

        When(x => x.RequiereAcompanante, () =>
        {
            RuleFor(x => x.Acompanante.NombreCompleto)
                .NotEmpty().WithMessage("Nombre del acompañante es obligatorio.");

            RuleFor(x => x.Acompanante.Parentesco)
                .NotEmpty().WithMessage("Parentesco es obligatorio.");
        });

        RuleFor(x => x.DiagnosticosSeleccionados)
            .Must(list => list != null && list.Count > 0)
            .WithMessage("Debe seleccionar al menos un diagnóstico.");
    }
}