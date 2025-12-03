using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbDeclaracionJurada
{
    [Key]
    public int Id { get; set; }

    public int? SolRegCannabisId { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? Detalle { get; set; }

    public DateOnly? Fecha { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? NombreDeclarante { get; set; }

    public bool? Aceptada { get; set; }

    [ForeignKey("SolRegCannabisId")]
    [InverseProperty("TbDeclaracionJurada")]
    public virtual TbSolRegCannabis? SolRegCannabis { get; set; }
}
