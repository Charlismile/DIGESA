using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Provincia
{
    public int Id { get; set; }

    public string NombreProvincia { get; set; } = null!;

    public virtual ICollection<Paciente> Paciente { get; set; } = new List<Paciente>();
}
