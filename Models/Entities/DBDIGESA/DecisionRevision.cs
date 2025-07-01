using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class DecisionRevision
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Revision> Revision { get; set; } = new List<Revision>();
}
