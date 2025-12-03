using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbReporteGenerado
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string NombreArchivo { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string TipoReporte { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? FechaGeneracion { get; set; }

    [StringLength(450)]
    public string? GeneradoPor { get; set; }

    public string? FiltrosAplicados { get; set; }

    [StringLength(500)]
    public string? RutaArchivo { get; set; }

    public long? TamanoBytes { get; set; }

    public bool? Descargado { get; set; }
}
