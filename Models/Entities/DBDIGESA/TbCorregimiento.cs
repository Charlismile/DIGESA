using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbCorregimiento
{
    public int Id { get; set; }

    public string? NombreCorregimiento { get; set; }

    public int? DistritoId { get; set; }

    public virtual TbDistrito? Distrito { get; set; }

    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
