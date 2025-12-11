using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbRegionSalud
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<TbMedico> TbMedico { get; set; } = new List<TbMedico>();

    public virtual ICollection<TbMedicoPaciente> TbMedicoPaciente { get; set; } = new List<TbMedicoPaciente>();

    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
