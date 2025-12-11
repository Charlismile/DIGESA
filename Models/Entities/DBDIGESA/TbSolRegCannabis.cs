using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbSolRegCannabis
{
    public int Id { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public int? PacienteId { get; set; }

    public DateOnly? FechaRevision { get; set; }

    public string? UsuarioRevisor { get; set; }

    public string? ComentarioRevision { get; set; }

    public int? NumSolSecuencia { get; set; }

    public int? NumSolAnio { get; set; }

    public int? NumSolMes { get; set; }

    public string? NumSolCompleta { get; set; }

    public string? CreadaPor { get; set; }

    public DateOnly? ModificadaEn { get; set; }

    public string? ModificadaPor { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public int? EstadoSolicitudId { get; set; }

    public bool? EsRenovacion { get; set; }

    public string? FotoCarnetUrl { get; set; }

    public string? FirmaDigitalUrl { get; set; }

    public bool? CarnetActivo { get; set; }

    public string? NumeroCarnet { get; set; }

    public DateTime? FechaEmisionCarnet { get; set; }

    public DateTime? FechaVencimientoCarnet { get; set; }

    public DateTime? FechaUltimaRenovacion { get; set; }

    public int? SolicitudPadreId { get; set; }

    public int VersionCarnet { get; set; }

    public string? RazonInactivacion { get; set; }

    public DateTime? FechaInactivacion { get; set; }

    public string? UsuarioInactivador { get; set; }

    public virtual TbEstadoSolicitud? EstadoSolicitud { get; set; }

    public virtual ICollection<TbSolRegCannabis> InverseSolicitudPadre { get; set; } = new List<TbSolRegCannabis>();

    public virtual TbPaciente? Paciente { get; set; }

    public virtual TbSolRegCannabis? SolicitudPadre { get; set; }

    public virtual ICollection<TbCodigoQr> TbCodigoQr { get; set; } = new List<TbCodigoQr>();

    public virtual ICollection<TbDeclaracionJurada> TbDeclaracionJurada { get; set; } = new List<TbDeclaracionJurada>();

    public virtual ICollection<TbDocumentoAdjunto> TbDocumentoAdjunto { get; set; } = new List<TbDocumentoAdjunto>();

    public virtual ICollection<TbHistorialRenovacion> TbHistorialRenovacionSolicitudAnterior { get; set; } = new List<TbHistorialRenovacion>();

    public virtual ICollection<TbHistorialRenovacion> TbHistorialRenovacionSolicitudNueva { get; set; } = new List<TbHistorialRenovacion>();

    public virtual ICollection<TbLogNotificaciones> TbLogNotificaciones { get; set; } = new List<TbLogNotificaciones>();

    public virtual ICollection<TbNotificacionVencimiento> TbNotificacionVencimiento { get; set; } = new List<TbNotificacionVencimiento>();

    public virtual ICollection<TbRegistroDispensacion> TbRegistroDispensacion { get; set; } = new List<TbRegistroDispensacion>();

    public virtual ICollection<TbSolRegCannabisHistorial> TbSolRegCannabisHistorial { get; set; } = new List<TbSolRegCannabisHistorial>();

    public virtual ICollection<TbTransferenciaResponsabilidad> TbTransferenciaResponsabilidad { get; set; } = new List<TbTransferenciaResponsabilidad>();
}
