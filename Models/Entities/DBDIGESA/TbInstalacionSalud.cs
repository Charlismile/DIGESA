using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbInstalacionSalud
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? Nombre { get; set; }

    [InverseProperty("Instalacion")]
    public virtual ICollection<TbMedicoPaciente> TbMedicoPaciente { get; set; } = new List<TbMedicoPaciente>();

    [InverseProperty("Instalacion")]
    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
