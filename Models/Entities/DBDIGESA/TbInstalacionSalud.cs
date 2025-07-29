using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbInstalacionSalud
{
    public int InstalacionId { get; set; }

    public string Nombre { get; set; } = null!;
}
