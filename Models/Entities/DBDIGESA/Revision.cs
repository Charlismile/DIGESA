using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Revision
{
    public int RevisionId { get; set; }

    public int RevisionSolicitudId { get; set; }

    public string RevisionRevisorId { get; set; } = null!;

    public string RevisionTipoRevision { get; set; } = null!;

    public DateTime? RevisionFechaRevision { get; set; }

    public string RevisionDecision { get; set; } = null!;

    public string? RevisionObservaciones { get; set; }

    public string RevisionEstado { get; set; } = null!;

    public virtual Solicitud RevisionSolicitud { get; set; } = null!;
}
