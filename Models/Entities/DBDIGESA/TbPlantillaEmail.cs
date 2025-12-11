using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPlantillaEmail
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Asunto { get; set; } = null!;

    public string CuerpoHtml { get; set; } = null!;

    public string? TipoNotificacion { get; set; }

    public bool? Activa { get; set; }

    public string? VariablesDisponibles { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
