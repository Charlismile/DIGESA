using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.BDUbicaciones;

public partial class RegionSalud
{
    public int RegionSaludId { get; set; }

    public int CodigoRegion { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
