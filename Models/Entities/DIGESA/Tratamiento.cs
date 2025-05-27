using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Tratamiento
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string? FormaFarmaceutica { get; set; }

    public string? ConcentracionCbd { get; set; }

    public string? ConcentracionThc { get; set; }

    public string? ViaAdministracion { get; set; }

    public string? Dosis { get; set; }

    public string? Frecuencia { get; set; }

    public int? DuracionDias { get; set; }

    public int? DuracionMeses { get; set; }

    public virtual Solicitud Solicitud { get; set; } = null!;
}
