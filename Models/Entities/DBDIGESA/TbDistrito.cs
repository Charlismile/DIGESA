using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbDistrito
{
    public int Id { get; set; }

    public string? NombreDistrito { get; set; }

    public int? ProvinciaId { get; set; }

    public virtual TbProvincia? Provincia { get; set; }

    public virtual ICollection<TbCorregimiento> TbCorregimiento { get; set; } = new List<TbCorregimiento>();

    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
