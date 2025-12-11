using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbViaAdministracion
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public bool IsActivo { get; set; }

    public virtual ICollection<TbNombreProductoPaciente> TbNombreProductoPaciente { get; set; } = new List<TbNombreProductoPaciente>();
}
