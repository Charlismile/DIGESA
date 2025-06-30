using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class AuditoriaAccion
{
    public int AuditoriaAccionId { get; set; }

    public string? AuditoriaAccionUsuarioId { get; set; }

    public string AuditoriaAccionAccionRealizada { get; set; } = null!;

    public string? AuditoriaAccionTablaAfectada { get; set; }

    public int? AuditoriaAccionRegistroAfectadoId { get; set; }

    public string? AuditoriaAccionDetallesAdicionales { get; set; }

    public DateTime? AuditoriaAccionFechaAccion { get; set; }

    public string? AuditoriaAccionIpaddress { get; set; }

    public string? AuditoriaAccionUserAgent { get; set; }
}
