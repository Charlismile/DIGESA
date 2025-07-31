using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPacienteComorbilidad
{
    public int Id { get; set; }

    public string? NombreDiagnostico { get; set; }

    public string? DetalleTratamiento { get; set; }

    public int? PacienteId { get; set; }

    public virtual TbPaciente? Paciente { get; set; }
}
