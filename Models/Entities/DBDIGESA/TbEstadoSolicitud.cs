using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbEstadoSolicitud
{
    public int IdEstado { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<TbSolRegCannabis> TbSolRegCannabis { get; set; } = new List<TbSolRegCannabis>();

    public virtual ICollection<TbSolRegCannabisHistorial> TbSolRegCannabisHistorial { get; set; } = new List<TbSolRegCannabisHistorial>();
}
