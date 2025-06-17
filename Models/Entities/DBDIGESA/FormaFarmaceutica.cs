using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class FormaFarmaceutica
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<TratamientoFormaFarmaceutica> TratamientoFormaFarmaceutica { get; set; } = new List<TratamientoFormaFarmaceutica>();
}
