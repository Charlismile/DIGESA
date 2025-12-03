using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPlantillaEmail
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(200)]
    [Unicode(false)]
    public string Asunto { get; set; } = null!;

    public string CuerpoHtml { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? TipoNotificacion { get; set; }

    public bool? Activa { get; set; }

    public string? VariablesDisponibles { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaActualizacion { get; set; }
}
