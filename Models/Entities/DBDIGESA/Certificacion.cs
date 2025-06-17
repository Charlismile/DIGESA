using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Certificacion
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string CodigoCertificado { get; set; } = null!;

    public string? CodigoQr { get; set; }

    public string? RutaArchivoQr { get; set; }

    public DateTime? FechaEmision { get; set; }

    public DateTime FechaVencimiento { get; set; }

    public string EstadoCertificado { get; set; } = null!;

    public virtual Solicitud Solicitud { get; set; } = null!;
}
