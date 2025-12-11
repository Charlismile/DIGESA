using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbAuditoriaMedico
{
    public int Id { get; set; }

    public int MedicoId { get; set; }

    public string TipoAccion { get; set; } = null!;

    public string UsuarioAccion { get; set; } = null!;

    public DateTime FechaAccion { get; set; }

    public string? DetallesAnteriores { get; set; }

    public string? DetallesNuevos { get; set; }

    public string? Comentarios { get; set; }

    public string? IpOrigen { get; set; }

    public virtual TbMedico Medico { get; set; } = null!;
}
