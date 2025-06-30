using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Paciente
{
    public int PacienteId { get; set; }

    public string? UsuarioId { get; set; }

    public string PacienteNombreCompleto { get; set; } = null!;

    public string PacienteTipoDocumento { get; set; } = null!;

    public string PacienteNumeroDocumento { get; set; } = null!;

    public string PacienteNacionalidad { get; set; } = null!;

    public DateTime PacienteFechaNacimiento { get; set; }

    public string PacienteSexo { get; set; } = null!;

    public string PacienteDireccionResidencia { get; set; } = null!;

    public string? PacienteTelefonoResidencial { get; set; }

    public string? PacienteTelefonoPersonal { get; set; }

    public string? PacienteTelefonoLaboral { get; set; }

    public string? PacienteCorreoElectronico { get; set; }

    public string PacienteInstalacionSalud { get; set; } = null!;

    public string PacienteRegionSalud { get; set; } = null!;

    public bool PacienteRequiereAcompanante { get; set; }

    public string? PacienteMotivoRequerimientoAcompanante { get; set; }

    public string? PacienteTipoDiscapacidad { get; set; }

    public DateTime PacienteFechaRegistro { get; set; }

    public string PacienteEstado { get; set; } = null!;

    public virtual ICollection<Acompanante> Acompanante { get; set; } = new List<Acompanante>();

    public virtual ICollection<Solicitud> Solicitud { get; set; } = new List<Solicitud>();
}
