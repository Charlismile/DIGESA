using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Medico
{
    public int MedicoId { get; set; }

    public string? UsuarioId { get; set; }

    public string MedicoNombreCompleto { get; set; } = null!;

    public string MedicoDisciplina { get; set; } = null!;

    public string? MedicoEspecialidad { get; set; }

    public string MedicoNumeroRegistroIdoneidad { get; set; } = null!;

    public string MedicoNumeroTelefono { get; set; } = null!;

    public string? MedicoCorreoElectronico { get; set; }

    public string MedicoInstalacionSalud { get; set; } = null!;

    public DateTime MedicoFechaRegistro { get; set; }

    public virtual ICollection<Solicitud> Solicitud { get; set; } = new List<Solicitud>();
}
