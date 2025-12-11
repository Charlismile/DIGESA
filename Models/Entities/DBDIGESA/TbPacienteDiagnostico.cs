using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPacienteDiagnostico
{
    public int Id { get; set; }

    public int? PacienteId { get; set; }

    public string? NombreDiagnostico { get; set; }

    public int? DiagnosticoId { get; set; }

    public string? Tipo { get; set; }

    public string? DetalleTratamiento { get; set; }

    public DateOnly? FechaDiagnostico { get; set; }

    public virtual TbPaciente? Paciente { get; set; }
}
