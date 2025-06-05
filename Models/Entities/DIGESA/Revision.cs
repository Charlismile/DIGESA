using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Revision
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public int RevisorId { get; set; }

    public string TipoRevision { get; set; } = null!;

    public DateTime? FechaRevision { get; set; }

    public string Decision { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual Usuario Revisor { get; set; } = null!;

    public virtual Solicitud Solicitudes { get; set; } = null!;
}
