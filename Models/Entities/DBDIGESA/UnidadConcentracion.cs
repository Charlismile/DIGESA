using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class UnidadConcentracion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Tratamiento> TratamientoUnidadCbdNavigation { get; set; } = new List<Tratamiento>();

    public virtual ICollection<Tratamiento> TratamientoUnidadOtroCannabinoide1 { get; set; } = new List<Tratamiento>();

    public virtual ICollection<Tratamiento> TratamientoUnidadOtroCannabinoide2 { get; set; } = new List<Tratamiento>();

    public virtual ICollection<Tratamiento> TratamientoUnidadThcNavigation { get; set; } = new List<Tratamiento>();
}
