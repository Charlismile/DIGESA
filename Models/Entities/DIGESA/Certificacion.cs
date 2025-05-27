using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Certificacion
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string? CodigoQr { get; set; }

    public byte[]? ImagenQr { get; set; }

    public string? NombreArchivoQr { get; set; }

    public DateTime? FechaEmision { get; set; }

    public DateTime? VigenciaHasta { get; set; }

    public virtual Solicitud Solicitud { get; set; } = null!;
}
