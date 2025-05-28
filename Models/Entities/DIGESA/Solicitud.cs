using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Solicitud
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public int? AcompananteId { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public string Estado { get; set; } = null!;

    public string? MotivoSolicitud { get; set; }

    public int? FuncionarioRecibeId { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public DateTime? FechaAprobacionRechazo { get; set; }

    public string? ObservacionesRevision { get; set; }

    public virtual Acompanante? Acompanante { get; set; }

    public virtual Certificacion? Certificacion { get; set; }

    public virtual ICollection<DocumentoAdjunto> DocumentoAdjuntos { get; set; } = new List<DocumentoAdjunto>();

    public virtual Usuario? FuncionarioRecibe { get; set; }

    public virtual Medico Medico { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual ICollection<Revision> Revisions { get; set; } = new List<Revision>();

    public virtual ICollection<SolicitudDiagnostico> SolicitudDiagnosticos { get; set; } = new List<SolicitudDiagnostico>();

    public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
}
