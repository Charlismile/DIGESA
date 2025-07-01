using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.BDUbicaciones;

public partial class Corregimiento
{
    public int CorregimientoId { get; set; }

    public int CodigoCorregimiento { get; set; }

    public string Nombre { get; set; } = null!;

    public int DistritoId { get; set; }

    public virtual Distrito Distrito { get; set; } = null!;
}
