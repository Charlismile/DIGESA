using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Paciente
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Nacionalidad { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public string InstalacionSalud { get; set; } = null!;

    public string RegionSalud { get; set; } = null!;

    public bool RequiereAcompanante { get; set; }

    public string? MotivoRequerimientoAcompanante { get; set; }

    public string? TipoDiscapacidad { get; set; }

    public string? FirmaBase64 { get; set; }

    public string? TelefonoResidencial { get; set; }

    public string? TelefonoPersonal { get; set; }

    public string? TelefonoLaboral { get; set; }

    public string? CorreoElectronico { get; set; }

    public int? ProvinciaId { get; set; }

    public int? DistritoId { get; set; }

    public int? CorregimientoId { get; set; }

    public string? DireccionExacta { get; set; }

    public virtual ICollection<Acompanante> Acompanante { get; set; } = new List<Acompanante>();

    public virtual Corregimiento? Corregimiento { get; set; }

    public virtual Distrito? Distrito { get; set; }

    public virtual ICollection<PacienteDiagnostico> PacienteDiagnostico { get; set; } = new List<PacienteDiagnostico>();

    public virtual Provincia? Provincia { get; set; }

    public virtual ICollection<Solicitud> Solicitud { get; set; } = new List<Solicitud>();

    public virtual Usuario? Usuario { get; set; }
}
