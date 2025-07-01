using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] ContraseñaHash { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public int RolId { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public string? FirmaBase64 { get; set; }

    public virtual ICollection<AuditoriaAccion> AuditoriaAccion { get; set; } = new List<AuditoriaAccion>();

    public virtual ICollection<DocumentoAdjunto> DocumentoAdjunto { get; set; } = new List<DocumentoAdjunto>();

    public virtual Medico? Medico { get; set; }

    public virtual ICollection<Paciente> Paciente { get; set; } = new List<Paciente>();

    public virtual ICollection<Revision> Revision { get; set; } = new List<Revision>();

    public virtual Rol Rol { get; set; } = null!;

    public virtual ICollection<Solicitud> Solicitud { get; set; } = new List<Solicitud>();
}
