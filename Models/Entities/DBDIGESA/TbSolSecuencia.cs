using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbSolSecuencia
{
    [Key]
    public int Id { get; set; }

    public int? IdEntidad { get; set; }

    public int? Anio { get; set; }

    public int? Numeracion { get; set; }

    public bool? IsActivo { get; set; }
}
