using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.Entities.DIGESA;

public class Paciente
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public string TipoDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    public string NumeroDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    public string Nacionalidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    public DateOnly FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    public string Sexo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección de residencia es obligatoria.")]
    public string DireccionResidencia { get; set; } = string.Empty;

    public string? TelefonoResidencial { get; set; }

    public string? TelefonoPersonal { get; set; }

    public string? TelefonoLaboral { get; set; }

    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    public string? CorreoElectronico { get; set; }

    [Required(ErrorMessage = "La instalación de salud es obligatoria.")]
    public string InstalacionSalud { get; set; } = string.Empty;

    [Required(ErrorMessage = "La región de salud es obligatoria.")]
    public string RegionSalud { get; set; } = string.Empty;

    public bool RequiereAcompanante { get; set; }

    public string? MotivoRequerimientoAcompanante { get; set; }

    public string? TipoDiscapacidad { get; set; }

    public virtual ICollection<Acompanante> Acompanantes { get; set; } = new List<Acompanante>();

    public virtual ICollection<PacienteDiagnostico> PacienteDiagnosticos { get; set; } = new List<PacienteDiagnostico>();

    public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();

    public virtual Usuario? Usuario { get; set; }
}
