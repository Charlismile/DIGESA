using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

[Index("NombreEstado", Name = "UQ__TbEstado__6CE50615E2886F9E", IsUnique = true)]
public partial class TbEstadoSolicitud
{
    [Key]
    public int IdEstado { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string NombreEstado { get; set; } = null!;

    [InverseProperty("EstadoSolicitud")]
    public virtual ICollection<TbSolRegCannabis> TbSolRegCannabis { get; set; } = new List<TbSolRegCannabis>();

    [InverseProperty("EstadoSolicitudIdHistorialNavigation")]
    public virtual ICollection<TbSolRegCannabisHistorial> TbSolRegCannabisHistorial { get; set; } = new List<TbSolRegCannabisHistorial>();
}
