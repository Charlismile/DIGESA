using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbRegionSalud
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [InverseProperty("Region")]
    public virtual ICollection<TbMedicoPaciente> TbMedicoPaciente { get; set; } = new List<TbMedicoPaciente>();

    [InverseProperty("Region")]
    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
