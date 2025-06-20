﻿using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TratamientoFormaFarmaceutica
{
    public int Id { get; set; }

    public int TratamientoId { get; set; }

    public int FormaFarmaceuticaId { get; set; }

    public virtual FormaFarmaceutica FormaFarmaceutica { get; set; } = null!;

    public virtual Tratamiento Tratamiento { get; set; } = null!;
}
