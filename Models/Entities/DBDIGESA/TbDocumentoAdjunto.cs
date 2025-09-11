using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbDocumentoAdjunto
{
    public int Id { get; set; }

    public int SolRegCannabisId { get; set; }

    public int TipoDocumentoId { get; set; }

    public string? NombreOriginal { get; set; }

    public string? NombreGuardado { get; set; }

    public string? Url { get; set; }

    public DateTime? FechaSubidaUtc { get; set; }

    public string? SubidoPor { get; set; }

    public bool? IsValido { get; set; }

    public virtual TbSolRegCannabis SolRegCannabis { get; set; } = null!;

    public virtual TbTipoDocumentoAdjunto TipoDocumento { get; set; } = null!;
}
