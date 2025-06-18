// using DIGESA.Models.DTOs;
// using FluentValidation;
//
// namespace DIGESA.Validadores
// {
//     public class AcompananteRegistroValidator : AbstractValidator<AcompananteRegistroDTO>
//     {
//         public AcompananteRegistroValidator()
//         {
//             RuleFor(x => x.NombreCompleto)
//                 .NotEmpty()
//                 .WithMessage("El nombre completo del acompañante es requerido.")
//                 .When(x => !string.IsNullOrEmpty(x.NombreCompleto) == false && x.Parentesco != null);
//
//             RuleFor(x => x.TipoDocumento)
//                 .NotEmpty()
//                 .WithMessage("Debe seleccionar un tipo de documento para el acompañante.")
//                 .When(x => !string.IsNullOrEmpty(x.NombreCompleto));
//
//             RuleFor(x => x.NumeroDocumento)
//                 .NotEmpty()
//                 .WithMessage("El número de documento del acompañante es requerido.")
//                 .When(x => !string.IsNullOrEmpty(x.NombreCompleto));
//
//             RuleFor(x => x.Nacionalidad)
//                 .NotEmpty()
//                 .WithMessage("La nacionalidad del acompañante es requerida.")
//                 .When(x => !string.IsNullOrEmpty(x.NombreCompleto));
//
//             RuleFor(x => x.Parentesco)
//                 .NotEmpty()
//                 .WithMessage("El parentesco del acompañante es requerido.")
//                 .When(x => !string.IsNullOrEmpty(x.NombreCompleto));
//         }
//     }
// }