using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.Entities.DIGESA;

public class Solicitud
{
    public int Id { get; set; }

    [Required]
    public int PacienteId { get; set; }

    [Required]
    public int MedicoId { get; set; }

    public int? AcompananteId { get; set; }

    public DateTime? FechaSolicitud { get; set; } = DateTime.Now;

    [Required]
    public string Estado { get; set; } = "Pendiente";

    public string? MotivoSolicitud { get; set; }

    public int? FuncionarioRecibeId { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public DateTime? FechaAprobacionRechazo { get; set; }

    public string? ObservacionesRevision { get; set; }

    public virtual Acompanante Acompanante { get; set; }

    public virtual Certificacion? Certificacion { get; set; }
    
    public bool AceptaTerminos { get; set; }
    public string? NombreFirmante { get; set; }

    public virtual ICollection<DocumentoAdjunto> DocumentoAdjuntos { get; set; } = new List<DocumentoAdjunto>();

    public virtual Usuario? FuncionarioRecibe { get; set; }

    [Required]
    public virtual Medico Medico { get; set; } = new();

    [Required]
    public virtual Paciente Paciente { get; set; } = new();

    public string DiagnosticoPrincipal { get; set; }
    public virtual ICollection<Revision> Revisiones { get; set; } = new List<Revision>();

    public virtual ICollection<SolicitudDiagnostico> SolicitudDiagnosticos { get; set; } = new List<SolicitudDiagnostico>();

    public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
}