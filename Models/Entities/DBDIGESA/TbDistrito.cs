using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbDistrito
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? NombreDistrito { get; set; }

    public int? ProvinciaId { get; set; }

    [ForeignKey("ProvinciaId")]
    [InverseProperty("TbDistrito")]
    public virtual TbProvincia? Provincia { get; set; }

    [InverseProperty("Distrito")]
    public virtual ICollection<TbCorregimiento> TbCorregimiento { get; set; } = new List<TbCorregimiento>();

    [InverseProperty("Distrito")]
    public virtual ICollection<TbPaciente> TbPaciente { get; set; } = new List<TbPaciente>();
}
