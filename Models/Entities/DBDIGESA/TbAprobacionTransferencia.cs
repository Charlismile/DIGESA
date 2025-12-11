using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbAprobacionTransferencia
{
    public int Id { get; set; }

    public int TransferenciaId { get; set; }

    public string UsuarioId { get; set; } = null!;

    public bool? Aprobada { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public string? Comentario { get; set; }

    public int? NivelAprobacion { get; set; }

    public virtual TbTransferenciaResponsabilidad Transferencia { get; set; } = null!;
}
