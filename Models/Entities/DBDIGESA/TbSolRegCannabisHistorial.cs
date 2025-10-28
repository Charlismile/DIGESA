using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbSolRegCannabisHistorial
{
    public int Id { get; set; }

    public int? SolRegCannabisId { get; set; }

    public string? Comentario { get; set; }

    public string? UsuarioRevisor { get; set; }

    public DateOnly? FechaCambio { get; set; }

    public int? EstadoSolicitudIdHistorial { get; set; }

    public virtual TbEstadoSolicitud? EstadoSolicitudIdHistorialNavigation { get; set; }

    public virtual TbSolRegCannabis? SolRegCannabis { get; set; }
}
