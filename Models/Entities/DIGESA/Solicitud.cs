using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Solicitud
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public int MedicoId { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public string Estado { get; set; } = null!;

    public string? MotivoSolicitud { get; set; }

    public virtual ICollection<Certificacion> Certificacions { get; set; } = new List<Certificacion>();

    public virtual ICollection<DocumentoAdjunto> DocumentoAdjuntos { get; set; } = new List<DocumentoAdjunto>();

    public virtual Medico Medico { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual ICollection<RevisionMedica> RevisionMedicas { get; set; } = new List<RevisionMedica>();

    public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();

    public virtual ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();
}
