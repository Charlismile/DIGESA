using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Contacto
{
    public int Id { get; set; }

    public string PropietarioTipo { get; set; } = null!;

    public int PropietarioId { get; set; }

    public int TipoContactoId { get; set; }

    public string Valor { get; set; } = null!;

    public virtual TipoContacto TipoContacto { get; set; } = null!;
}
