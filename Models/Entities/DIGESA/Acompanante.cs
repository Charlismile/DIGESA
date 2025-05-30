﻿

namespace DIGESA.Models.Entities.DIGESA;

public partial class Acompanante
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Nacionalidad { get; set; } = null!;

    public string Parentesco { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
}
