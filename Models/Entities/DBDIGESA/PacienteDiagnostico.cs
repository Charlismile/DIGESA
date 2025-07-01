using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class PacienteDiagnostico
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public int DiagnosticoId { get; set; }

    public DateOnly? FechaDiagnostico { get; set; }

    public string? Observaciones { get; set; }

    public virtual Diagnostico Diagnostico { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;
}
