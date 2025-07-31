using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbDeclaracionJurada
{
    public int Id { get; set; }

    public int? SolRegCannabisId { get; set; }

    public string? Detalle { get; set; }

    public DateOnly? Fecha { get; set; }

    public string? NombreDeclarante { get; set; }

    public bool Aceptada { get; set; }

    public virtual TbSolRegCannabis? SolRegCannabis { get; set; }
}
