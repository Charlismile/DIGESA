using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbNotificacionVencimiento
{
    public int Id { get; set; }

    public int SolRegCannabisId { get; set; }

    public int? DiasAntelacion { get; set; }

    public DateTime? FechaEnvio { get; set; }

    public bool? EmailEnviado { get; set; }

    public string? UsuarioNotificado { get; set; }

    public string? TipoNotificacion { get; set; }

    public virtual TbSolRegCannabis SolRegCannabis { get; set; } = null!;
}
