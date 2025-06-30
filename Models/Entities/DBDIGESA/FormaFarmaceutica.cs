using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class FormaFarmaceutica
{
    public int FormaFarmaceuticaId { get; set; }

    public string FormaFarmaceuticaNombre { get; set; } = null!;

    public virtual ICollection<TratamientoFormaFarmaceutica> TratamientoFormaFarmaceutica { get; set; } = new List<TratamientoFormaFarmaceutica>();
}
