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

    public long? MedicoTelefono { get; set; }

    public string? MedicoInstalacion { get; set; }

    public int? RegionId { get; set; }

    public int? InstalacionId { get; set; }

    public int? PacienteId { get; set; }

    public virtual TbInstalacionSalud? Instalacion { get; set; }

    public virtual TbPaciente? Paciente { get; set; }

    public virtual TbRegionSalud? Region { get; set; }

    public virtual ICollection<TbDocumentoMedico> TbDocumentoMedico { get; set; } = new List<TbDocumentoMedico>();
}
