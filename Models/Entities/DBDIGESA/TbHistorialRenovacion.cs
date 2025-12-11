using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbHistorialRenovacion
{
    public int Id { get; set; }

    public int SolicitudAnteriorId { get; set; }

    public int SolicitudNuevaId { get; set; }

    public DateTime FechaRenovacion { get; set; }

    public string? RazonRenovacion { get; set; }

    public string? UsuarioRenovador { get; set; }

    public string? Comentarios { get; set; }

    public virtual TbSolRegCannabis SolicitudAnterior { get; set; } = null!;

    public virtual TbSolRegCannabis SolicitudNueva { get; set; } = null!;
}
