using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.BDUbicaciones;

public partial class Distrito
{
    public int DistritoId { get; set; }

    public int CodigoDistrito { get; set; }

    public string Nombre { get; set; } = null!;

    public int ProvinciaId { get; set; }

    public virtual ICollection<Corregimiento> Corregimiento { get; set; } = new List<Corregimiento>();

    public virtual Provincia Provincia { get; set; } = null!;
}
