using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Medico
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string? Disciplina { get; set; }

    public string? Especialidad { get; set; }

    public string NumeroRegistroIdoneidad { get; set; } = null!;

    public string? NumeroTelefono { get; set; }

    public string? InstalacionSalud { get; set; }

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
