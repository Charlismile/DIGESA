using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class ViaAdministracion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<TratamientoViaAdministracion> TratamientoViaAdministracion { get; set; } = new List<TratamientoViaAdministracion>();
}
