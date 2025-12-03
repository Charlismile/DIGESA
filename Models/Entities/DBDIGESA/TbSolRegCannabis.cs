using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

[Index("CarnetActivo", Name = "IX_TbSolRegCannabis_CarnetActivo")]
[Index("FechaVencimientoCarnet", Name = "IX_TbSolRegCannabis_FechaVencimiento")]
[Index("NumeroCarnet", Name = "IX_TbSolRegCannabis_NumeroCarnet")]
public partial class TbSolRegCannabis
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaSolicitud { get; set; }

    public int? PacienteId { get; set; }

    public DateOnly? FechaRevision { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? UsuarioRevisor { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? ComentarioRevision { get; set; }

    public int? NumSolSecuencia { get; set; }

    public int? NumSolAnio { get; set; }

    public int? NumSolMes { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? NumSolCompleta { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? CreadaPor { get; set; }

    public DateOnly? ModificadaEn { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? ModificadaPor { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaAprobacion { get; set; }

    public int? EstadoSolicitudId { get; set; }

    public bool? EsRenovacion { get; set; }

    [StringLength(500)]
    public string? FotoCarnetUrl { get; set; }

    [StringLength(500)]
    public string? FirmaDigitalUrl { get; set; }

    public bool? CarnetActivo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? NumeroCarnet { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaEmisionCarnet { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaVencimientoCarnet { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaUltimaRenovacion { get; set; }

    [ForeignKey("EstadoSolicitudId")]
    [InverseProperty("TbSolRegCannabis")]
    public virtual TbEstadoSolicitud? EstadoSolicitud { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("TbSolRegCannabis")]
    public virtual TbPaciente? Paciente { get; set; }

    [InverseProperty("SolRegCannabis")]
    public virtual ICollection<TbDeclaracionJurada> TbDeclaracionJurada { get; set; } = new List<TbDeclaracionJurada>();

    [InverseProperty("SolRegCannabis")]
    public virtual ICollection<TbDocumentoAdjunto> TbDocumentoAdjunto { get; set; } = new List<TbDocumentoAdjunto>();

    [InverseProperty("SolRegCannabis")]
    public virtual ICollection<TbNotificacionVencimiento> TbNotificacionVencimiento { get; set; } = new List<TbNotificacionVencimiento>();

    [InverseProperty("SolRegCannabis")]
    public virtual ICollection<TbSolRegCannabisHistorial> TbSolRegCannabisHistorial { get; set; } = new List<TbSolRegCannabisHistorial>();

    [InverseProperty("SolRegCannabis")]
    public virtual ICollection<TbTransferenciaResponsabilidad> TbTransferenciaResponsabilidad { get; set; } = new List<TbTransferenciaResponsabilidad>();
}
