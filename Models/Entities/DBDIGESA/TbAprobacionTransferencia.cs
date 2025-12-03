using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbAprobacionTransferencia
{
    [Key]
    public int Id { get; set; }

    public int TransferenciaId { get; set; }

    [StringLength(450)]
    public string UsuarioId { get; set; } = null!;

    public bool? Aprobada { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaAprobacion { get; set; }

    [StringLength(500)]
    public string? Comentario { get; set; }

    public int? NivelAprobacion { get; set; }

    [ForeignKey("TransferenciaId")]
    [InverseProperty("TbAprobacionTransferencia")]
    public virtual TbTransferenciaResponsabilidad Transferencia { get; set; } = null!;
}
