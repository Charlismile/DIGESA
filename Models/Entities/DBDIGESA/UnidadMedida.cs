using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class UnidadMedida
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Simbolo { get; set; } = null!;

    public virtual ICollection<Tratamiento> TratamientoUnidadCbd { get; set; } = new List<Tratamiento>();

    public virtual ICollection<Tratamiento> TratamientoUnidadThc { get; set; } = new List<Tratamiento>();
}
