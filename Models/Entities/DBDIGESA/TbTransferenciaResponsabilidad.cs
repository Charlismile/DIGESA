using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

[Index("Estado", Name = "IX_TbTransferenciaResponsabilidad_Estado")]
public partial class TbTransferenciaResponsabilidad
{
    [Key]
    public int Id { get; set; }

    public int SolRegCannabisId { get; set; }

    [StringLength(450)]
    public string UsuarioOrigenId { get; set; } = null!;

    [StringLength(450)]
    public string UsuarioDestinoId { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? FechaSolicitud { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaAprobacion { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Estado { get; set; }

    [StringLength(500)]
    public string? Comentario { get; set; }

    [StringLength(450)]
    public string? AprobadoPor { get; set; }

    [ForeignKey("SolRegCannabisId")]
    [InverseProperty("TbTransferenciaResponsabilidad")]
    public virtual TbSolRegCannabis SolRegCannabis { get; set; } = null!;

    [InverseProperty("Transferencia")]
    public virtual ICollection<TbAprobacionTransferencia> TbAprobacionTransferencia { get; set; } = new List<TbAprobacionTransferencia>();
}
