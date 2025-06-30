using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Acompanante
{
    public int AcompananteId { get; set; }

    public int AcompanantePacienteId { get; set; }

    public string AcompananteNombreCompleto { get; set; } = null!;

    public string AcompananteTipoDocumento { get; set; } = null!;

    public string AcompananteNumeroDocumento { get; set; } = null!;

    public string AcompananteNacionalidad { get; set; } = null!;

    public string AcompananteParentesco { get; set; } = null!;

    public DateTime AcompananteFechaRegistro { get; set; }

    public bool AcompananteActivo { get; set; }

    public virtual Paciente AcompanantePaciente { get; set; } = null!;
}
