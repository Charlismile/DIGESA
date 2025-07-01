using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Revision
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public int RevisorId { get; set; }

    public string TipoRevision { get; set; } = null!;

    public DateTime? FechaRevision { get; set; }

    public int DecisionRevisionId { get; set; }

    public string? Observaciones { get; set; }

    public virtual DecisionRevision DecisionRevision { get; set; } = null!;

    public virtual Usuario Revisor { get; set; } = null!;

    public virtual Solicitud Solicitud { get; set; } = null!;
}
