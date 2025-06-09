using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class UnidadMedidum
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Simbolo { get; set; } = null!;

    public virtual ICollection<Tratamiento> TratamientoUnidadCbds { get; set; } = new List<Tratamiento>();

    public virtual ICollection<Tratamiento> TratamientoUnidadThcs { get; set; } = new List<Tratamiento>();
}
