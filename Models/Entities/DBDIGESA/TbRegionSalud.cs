using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbRegionSalud
{
    public int RegionSaludId { get; set; }

    public string Nombre { get; set; } = null!;
}
