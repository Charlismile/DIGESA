using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbUnidades
{
    public int Id { get; set; }

    public string NombreUnidad { get; set; } = null!;

    public bool IsActivo { get; set; }

    public virtual ICollection<TbNombreProductoPaciente> TbNombreProductoPaciente { get; set; } = new List<TbNombreProductoPaciente>();
}
