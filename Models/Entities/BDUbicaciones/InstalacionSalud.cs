using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.BDUbicaciones;

public partial class InstalacionSalud
{
    public int InstalacionId { get; set; }

    public string Instalacion { get; set; } = null!;
}
