using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Medico
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string Especialidad { get; set; } = null!;

    public string NumeroRegistroIdoneidad { get; set; } = null!;

    public string InstalacionSalud { get; set; } = null!;

    public string? FirmaBase64 { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public virtual ICollection<Solicitud> Solicitud { get; set; } = new List<Solicitud>();

    public virtual Usuario? Usuario { get; set; }
}
