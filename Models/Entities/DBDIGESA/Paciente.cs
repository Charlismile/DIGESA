using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Paciente
{
    public int Id { get; set; }

    public string? UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Nacionalidad { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public string DireccionResidencia { get; set; } = null!;

    public string? TelefonoResidencial { get; set; }

    public string? TelefonoPersonal { get; set; }

    public string? TelefonoLaboral { get; set; }

    public string? CorreoElectronico { get; set; }

    public string InstalacionSalud { get; set; } = null!;

    public string RegionSalud { get; set; } = null!;

    public bool RequiereAcompanante { get; set; }

    public string? MotivoRequerimientoAcompanante { get; set; }

    public string? TipoDiscapacidad { get; set; }

    public virtual ICollection<Acompanante> Acompanante { get; set; } = new List<Acompanante>();

    public virtual ICollection<PacienteDiagnostico> PacienteDiagnostico { get; set; } = new List<PacienteDiagnostico>();

    public virtual ICollection<Solicitud> Solicitud { get; set; } = new List<Solicitud>();
}
