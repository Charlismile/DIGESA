using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPacienteComorbilidad
{
    public int Id { get; set; }

    public int PacienteId { get; set; }

    public string NombreDiagnostico { get; set; } = null!;

    public string? DetalleTratamiento { get; set; }

    public DateOnly? FechaDiagnostico { get; set; }

    public bool TieneComorbilidad { get; set; }

    public virtual TbPaciente Paciente { get; set; } = null!;
}
