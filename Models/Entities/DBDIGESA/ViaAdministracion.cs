using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class ViaAdministracion
{
    public int ViaAdministracionId { get; set; }

    public string ViaAdministracionNombre { get; set; } = null!;

    public virtual ICollection<TratamientoViaAdministracion> TratamientoViaAdministracion { get; set; } = new List<TratamientoViaAdministracion>();
}
