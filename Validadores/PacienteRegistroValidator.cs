// using DIGESA.Models.DTOs;
// using FluentValidation;
//
// public class PacienteRegistroValidator : AbstractValidator<PacienteRegistroDTO>
// {
//     public PacienteRegistroValidator()
//     {
//         // Validaciones básicas
//         RuleFor(x => x.NombreCompleto).NotEmpty();
//         RuleFor(x => x.TipoDocumento).NotEmpty();
//         RuleFor(x => x.NumeroDocumento).NotEmpty();
//
//         // Validación condicional del acompañante
//         When(x => x.RequiereAcompanante, () =>
//         {
//             RuleFor(x => x.Acompanante.NombreCompleto)
//                 .NotEmpty().WithMessage("Nombre del acompañante requerido.");
//
//             RuleFor(x => x.Acompanante.TipoDocumento)
//                 .NotEmpty().WithMessage("Tipo de documento del acompañante requerido.");
//
//             RuleFor(x => x.Acompanante.NumeroDocumento)
//                 .NotEmpty().WithMessage("Número de documento del acompañante requerido.");
//
//             RuleFor(x => x.Acompanante.Parentesco)
//                 .NotEmpty().WithMessage("Parentesco del acompañante requerido.");
//         });
//     }
// }