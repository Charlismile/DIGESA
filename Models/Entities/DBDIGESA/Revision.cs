using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Revision
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string RevisorId { get; set; } = null!;

    public string TipoRevision { get; set; } = null!;

    public DateTime? FechaRevision { get; set; }

    public string Decision { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual Solicitud Solicitud { get; set; } = null!;
}
