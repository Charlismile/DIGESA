using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class SolicitudDiagnostico
{
    public int SolicitudId { get; set; }

    public int DiagnosticoId { get; set; }

    public bool? EsPrimario { get; set; }

    public string? Observaciones { get; set; }

    public virtual Diagnostico Diagnosticos { get; set; } = null!;

    public virtual Solicitud Solicitudes { get; set; } = null!;
}
