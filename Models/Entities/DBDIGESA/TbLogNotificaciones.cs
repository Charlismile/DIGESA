using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbLogNotificaciones
{
    public int Id { get; set; }

    public int? SolicitudId { get; set; }

    public string TipoNotificacion { get; set; } = null!;

    public DateTime FechaEnvio { get; set; }

    public string? MetodoEnvio { get; set; }

    public string? Destinatario { get; set; }

    public string? Estado { get; set; }

    public string? Error { get; set; }

    public virtual TbSolRegCannabis? Solicitud { get; set; }
}
