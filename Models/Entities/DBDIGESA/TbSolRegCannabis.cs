using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbSolRegCannabis
{
    public int Id { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    public int? PacienteId { get; set; }

    public string? EstadoSolicitud { get; set; }

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

    public virtual TbPaciente? Paciente { get; set; }

    public virtual ICollection<TbDeclaracionJurada> TbDeclaracionJurada { get; set; } = new List<TbDeclaracionJurada>();

    public virtual ICollection<TbSolRegCannabisHistorial> TbSolRegCannabisHistorial { get; set; } = new List<TbSolRegCannabisHistorial>();
}
