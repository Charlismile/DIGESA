using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbDocumentoAdjunto
{
    [Key]
    public int Id { get; set; }

    public int SolRegCannabisId { get; set; }

    public int TipoDocumentoId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? NombreOriginal { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? NombreGuardado { get; set; }

    [StringLength(300)]
    public string? Url { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaSubidaUtc { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? SubidoPor { get; set; }

    public bool? IsValido { get; set; }

    public bool EsDocumentoMedico { get; set; }

    public int? MedicoId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Categoria { get; set; }

    public int? Version { get; set; }

    [ForeignKey("SolRegCannabisId")]
    [InverseProperty("TbDocumentoAdjunto")]
    public virtual TbSolRegCannabis SolRegCannabis { get; set; } = null!;

    [ForeignKey("TipoDocumentoId")]
    [InverseProperty("TbDocumentoAdjunto")]
    public virtual TbTipoDocumentoAdjunto TipoDocumento { get; set; } = null!;
}
