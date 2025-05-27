using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class RevisionMedica
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public int RevisorId { get; set; }

    public DateTime? FechaRevision { get; set; }

    public string? Observaciones { get; set; }

    public bool Aprobado { get; set; }

    public virtual Usuario Revisor { get; set; } = null!;

    public virtual Solicitud Solicitud { get; set; } = null!;
}
