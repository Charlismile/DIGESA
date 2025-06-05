using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class TratamientoCannabinoide
{
    public int Id { get; set; }

    public int TratamientoId { get; set; }

    public string Tipo { get; set; } = null!;

    public string? Observacion { get; set; }

    public virtual Tratamiento Tratamientos { get; set; } = null!;
}
