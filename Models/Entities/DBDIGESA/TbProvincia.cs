using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbProvincia
{
    public int Id { get; set; }

    public string? NombreProvincia { get; set; }

    public virtual ICollection<TbDistrito> TbDistrito { get; set; } = new List<TbDistrito>();

    public virtual ICollection<TbFarmaciaAutorizada> TbFarmaciaAutorizada { get; set; } = new List<TbFarmaciaAutorizada>();

    public virtual ICollection<TbMedico> TbMedico { get; set; } = new List<TbMedico>();

    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
