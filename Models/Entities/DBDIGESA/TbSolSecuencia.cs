using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbSolSecuencia
{
    public int Id { get; set; }

    public int? IdEntidad { get; set; }

    public int? Anio { get; set; }

    public int? Numeracion { get; set; }

    public bool? IsActivo { get; set; }
}
