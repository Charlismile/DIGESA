using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Diagnostico
{
    public int Id { get; set; }

    public string? CodigoCie10 { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool EsOtro { get; set; }

    public virtual ICollection<PacienteDiagnostico> PacienteDiagnostico { get; set; } = new List<PacienteDiagnostico>();

    public virtual ICollection<SolicitudDiagnostico> SolicitudDiagnostico { get; set; } = new List<SolicitudDiagnostico>();
}
