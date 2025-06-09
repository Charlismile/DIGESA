using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Medico
{
    public int Id { get; set; }

    public string? UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Disciplina { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string NumeroRegistroIdoneidad { get; set; } = null!;

    public string NumeroTelefono { get; set; } = null!;

    public string? CorreoElectronico { get; set; }

    public string InstalacionSalud { get; set; } = null!;

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();

    public virtual AspNetUser? Usuario { get; set; }
}
