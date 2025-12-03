using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbProvincia
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? NombreProvincia { get; set; }

    [InverseProperty("Provincia")]
    public virtual ICollection<TbDistrito> TbDistrito { get; set; } = new List<TbDistrito>();

    [InverseProperty("Provincia")]
    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
