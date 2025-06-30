using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Certificacion
{
    public int CertificacionId { get; set; }

    public int CertificacionSolicitudId { get; set; }

    public string CertificacionCodigoCertificado { get; set; } = null!;

    public string? CertificacionCodigoQr { get; set; }

    public string? CertificacionRutaArchivoQr { get; set; }

    public DateTime? CertificacionFechaEmision { get; set; }

    public DateTime CertificacionFechaVencimiento { get; set; }

    public string CertificacionEstado { get; set; } = null!;

    public virtual Solicitud CertificacionSolicitud { get; set; } = null!;
}
