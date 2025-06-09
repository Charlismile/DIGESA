using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TratamientoViaAdministracion
{
    public int Id { get; set; }

    public int TratamientoId { get; set; }

    public int ViaAdministracionId { get; set; }

    public virtual Tratamiento Tratamiento { get; set; } = null!;

    public virtual ViaAdministracion ViaAdministracion { get; set; } = null!;
}
