using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Paciente
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string? Genero { get; set; }

    public string? TelefonoResidencial { get; set; }

    public string? TelefonoPersonal { get; set; }

    public string? TelefonoLaboral { get; set; }

    public string? Direccion { get; set; }

    public string? Nacionalidad { get; set; }

    public string? TipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = null!;

    public string? InstalacionSalud { get; set; }

    public string? RegionSalud { get; set; }

    public bool? RequiereAcompanante { get; set; }

    public string? MotivoRequerimiento { get; set; }

    public string? TipoDiscapacidad { get; set; }

    public virtual ICollection<Acompanante> Acompanantes { get; set; } = new List<Acompanante>();

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();

    public virtual Usuario Usuario { get; set; } = null!;
}
