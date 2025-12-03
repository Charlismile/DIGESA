using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbFormaFarmaceutica
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    public bool IsActivo { get; set; }

    [InverseProperty("FormaFarmaceuticaNavigation")]
    public virtual ICollection<TbNombreProductoPaciente> TbNombreProductoPaciente { get; set; } = new List<TbNombreProductoPaciente>();
}
