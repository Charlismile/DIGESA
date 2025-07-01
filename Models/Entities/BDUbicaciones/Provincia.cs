using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.BDUbicaciones;

public partial class Provincia
{
    public int ProvinciaId { get; set; }

    public int CodigoProvincia { get; set; }

    public string Nombre { get; set; } = null!;

    public int RegionSaludId { get; set; }

    public virtual ICollection<Distrito> Distrito { get; set; } = new List<Distrito>();

    public virtual RegionSalud RegionSalud { get; set; } = null!;
}
