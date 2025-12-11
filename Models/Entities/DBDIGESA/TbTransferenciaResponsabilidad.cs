using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbTransferenciaResponsabilidad
{
    public int Id { get; set; }

    public int SolRegCannabisId { get; set; }

    public string UsuarioOrigenId { get; set; } = null!;

    public string UsuarioDestinoId { get; set; } = null!;

    public DateTime? FechaSolicitud { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string? Estado { get; set; }

    public string? Comentario { get; set; }

    public string? AprobadoPor { get; set; }

    public virtual TbSolRegCannabis SolRegCannabis { get; set; } = null!;

    public virtual ICollection<TbAprobacionTransferencia> TbAprobacionTransferencia { get; set; } = new List<TbAprobacionTransferencia>();
}
