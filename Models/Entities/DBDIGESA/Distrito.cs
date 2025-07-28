using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Distrito
{
    public int Id { get; set; }

    public string NombreDistrito { get; set; } = null!;

    public int ProvinciaId { get; set; }

    public virtual ICollection<Corregimiento> Corregimiento { get; set; } = new List<Corregimiento>();

    public virtual ICollection<Paciente> Paciente { get; set; } = new List<Paciente>();
}
