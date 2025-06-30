using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class UnidadMedida
{
    public int UnidadMedidaId { get; set; }

    public string UnidadMedidaNombre { get; set; } = null!;

    public string UnidadMedidaSimbolo { get; set; } = null!;
}
