using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Solicitud
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public int? AcompananteId { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public string Estado { get; set; } = null!;

    public string? MotivoSolicitud { get; set; }

    public string? FuncionarioRecibeId { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public DateTime? FechaAprobacionRechazo { get; set; }

    public string? ObservacionesRevision { get; set; }

    public bool AceptaTerminos { get; set; }

    public string? NombreFirmante { get; set; }

    public virtual Acompanante? Acompanante { get; set; }

    public virtual ICollection<Certificacion> Certificacion { get; set; } = new List<Certificacion>();

    public virtual ICollection<DocumentoAdjunto> DocumentoAdjunto { get; set; } = new List<DocumentoAdjunto>();

    public virtual Medico Medico { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual ICollection<Revision> Revision { get; set; } = new List<Revision>();

    public virtual ICollection<SolicitudDiagnostico> SolicitudDiagnostico { get; set; } = new List<SolicitudDiagnostico>();

    public virtual ICollection<Tratamiento> Tratamiento { get; set; } = new List<Tratamiento>();
}
