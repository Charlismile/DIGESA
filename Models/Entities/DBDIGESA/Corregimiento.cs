using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Corregimiento
{
    public int Id { get; set; }

    public string NombreCorregimiento { get; set; } = null!;

    public int DistritoId { get; set; }

    public virtual Distrito Distrito { get; set; } = null!;

    public virtual ICollection<Paciente> Paciente { get; set; } = new List<Paciente>();
}
