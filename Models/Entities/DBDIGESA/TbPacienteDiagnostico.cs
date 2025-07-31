using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPacienteDiagnostico
{
    public int Id { get; set; }

    public int? PacienteId { get; set; }

    public string? NombreDiagnostico { get; set; }

    public virtual TbPaciente? Paciente { get; set; }
}
