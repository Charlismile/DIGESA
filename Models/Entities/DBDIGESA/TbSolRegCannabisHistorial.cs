using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbSolRegCannabisHistorial
{
    [Key]
    public int Id { get; set; }

    public int? SolRegCannabisId { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Comentario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? UsuarioRevisor { get; set; }

    public DateOnly? FechaCambio { get; set; }

    public int? EstadoSolicitudIdHistorial { get; set; }

    [ForeignKey("EstadoSolicitudIdHistorial")]
    [InverseProperty("TbSolRegCannabisHistorial")]
    public virtual TbEstadoSolicitud? EstadoSolicitudIdHistorialNavigation { get; set; }

    [ForeignKey("SolRegCannabisId")]
    [InverseProperty("TbSolRegCannabisHistorial")]
    public virtual TbSolRegCannabis? SolRegCannabis { get; set; }
}
