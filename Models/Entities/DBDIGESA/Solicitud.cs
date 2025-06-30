using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Solicitud
{
    public int SolicitudId { get; set; }

    public int SolicitudPacienteId { get; set; }

    public int SolicitudMedicoId { get; set; }

    public string SolicitudEstado { get; set; } = null!;

    public DateTime SolicitudFechaSolicitud { get; set; }

    public string? SolicitudObservaciones { get; set; }

    public bool SolicitudAceptaTerminos { get; set; }

    public virtual ICollection<Certificacion> Certificacion { get; set; } = new List<Certificacion>();

    public virtual ICollection<DocumentoAdjunto> DocumentoAdjunto { get; set; } = new List<DocumentoAdjunto>();

    public virtual ICollection<Revision> Revision { get; set; } = new List<Revision>();

    public virtual Medico SolicitudMedico { get; set; } = null!;

    public virtual Paciente SolicitudPaciente { get; set; } = null!;

    public virtual ICollection<Tratamiento> Tratamiento { get; set; } = new List<Tratamiento>();
}
