using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbAcompanantePaciente
{
    public int Id { get; set; }

    public int? PacienteId { get; set; }

    public string? PrimerNombre { get; set; }

    public string? SegundoNombre { get; set; }

    public string? PrimerApellido { get; set; }

    public string? SegundoApellido { get; set; }

    public string? TipoDocumento { get; set; }

    public string? NumeroDocumento { get; set; }

    public string? Nacionalidad { get; set; }

    public string? Parentesco { get; set; }

    public string? TelefonoMovil { get; set; }

    public virtual TbPaciente? Paciente { get; set; }
}
