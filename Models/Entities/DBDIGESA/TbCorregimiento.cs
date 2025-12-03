using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbCorregimiento
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? NombreCorregimiento { get; set; }

    public int? DistritoId { get; set; }

    [ForeignKey("DistritoId")]
    [InverseProperty("TbCorregimiento")]
    public virtual TbDistrito? Distrito { get; set; }

    [InverseProperty("Corregimiento")]
    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
