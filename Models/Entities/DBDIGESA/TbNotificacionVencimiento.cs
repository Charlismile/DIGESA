using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

[Index("SolRegCannabisId", Name = "IX_TbNotificacionVencimiento_Solicitud")]
public partial class TbNotificacionVencimiento
{
    [Key]
    public int Id { get; set; }

    public int SolRegCannabisId { get; set; }

    public int? DiasAntelacion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaEnvio { get; set; }

    public bool? EmailEnviado { get; set; }

    [StringLength(450)]
    public string? UsuarioNotificado { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TipoNotificacion { get; set; }

    [ForeignKey("SolRegCannabisId")]
    [InverseProperty("TbNotificacionVencimiento")]
    public virtual TbSolRegCannabis SolRegCannabis { get; set; } = null!;
}
