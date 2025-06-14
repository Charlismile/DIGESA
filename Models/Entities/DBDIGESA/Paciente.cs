﻿using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Paciente
{
    public int Id { get; set; }

    public string? UsuarioId { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Nacionalidad { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Sexo { get; set; } = null!;

    public string DireccionResidencia { get; set; } = null!;

    public string? TelefonoResidencial { get; set; }

    public string? TelefonoPersonal { get; set; }

    public string? TelefonoLaboral { get; set; }

    public string? CorreoElectronico { get; set; }

    public string InstalacionSalud { get; set; } = null!;

    public string RegionSalud { get; set; } = null!;

    public bool RequiereAcompanante { get; set; }

    public string? MotivoRequerimientoAcompanante { get; set; }

    public string? TipoDiscapacidad { get; set; }
    
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public string? EstadoSolicitud { get; set; } = "Pendiente"; // Pendiente / Aprobado / Rechazado

    public virtual ICollection<Acompanante> Acompanantes { get; set; } = new List<Acompanante>();

    public virtual ICollection<PacienteDiagnostico> PacienteDiagnosticos { get; set; } = new List<PacienteDiagnostico>();

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();

    public virtual AspNetUser? Usuario { get; set; }
}
