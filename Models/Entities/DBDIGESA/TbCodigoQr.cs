using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbCodigoQr
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string CodigoQr { get; set; } = null!;

    public DateTime FechaGeneracion { get; set; }

    public DateTime? FechaVencimiento { get; set; }

    public bool Activo { get; set; }

    public int VecesEscaneado { get; set; }

    public DateTime? UltimoEscaneo { get; set; }

    public string? UltimoEscaneadoPor { get; set; }

    public string? Comentarios { get; set; }

    public virtual TbSolRegCannabis Solicitud { get; set; } = null!;
}
