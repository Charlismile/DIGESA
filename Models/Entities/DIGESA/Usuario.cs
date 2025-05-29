using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public byte[] ContraseñaHash { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<AuditoriaAccion> AuditoriaAcciones { get; set; } = new List<AuditoriaAccion>();

    public virtual ICollection<DocumentoAdjunto> DocumentoAdjuntos { get; set; } = new List<DocumentoAdjunto>();

    public virtual Medico? Medico { get; set; }

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();

    public virtual ICollection<Revision> Revisiones { get; set; } = new List<Revision>();

    public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
}
