using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbReporteGenerado
{
    public int Id { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string TipoReporte { get; set; } = null!;

    public DateTime? FechaGeneracion { get; set; }

    public string? GeneradoPor { get; set; }

    public string? FiltrosAplicados { get; set; }

    public string? RutaArchivo { get; set; }

    public long? TamanoBytes { get; set; }

    public bool? Descargado { get; set; }
}
