using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbMedicoPaciente
{
    public int Id { get; set; }

    public string? PrimerNombre { get; set; }

    public string? PrimerApellido { get; set; }

    public string? MedicoDisciplina { get; set; }

    public string? MedicoIdoneidad { get; set; }

    public string? MedicoTelefono { get; set; }

    public int? RegionId { get; set; }

    public int? InstalacionId { get; set; }

    public string DetalleMedico { get; set; } = null!;

    public int? PacienteId { get; set; }

    public string? InstalacionPersonalizada { get; set; }

    public int? MedicoId { get; set; }

    public virtual TbInstalacionSalud? Instalacion { get; set; }

    public virtual TbMedico? Medico { get; set; }

    public virtual TbPaciente? Paciente { get; set; }

    public virtual TbRegionSalud? Region { get; set; }
}
