using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class AuditoriaAccion
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string AccionRealizada { get; set; } = null!;

    public string? NombreTablaAfectada { get; set; }

    public int? RegistroAfectadoId { get; set; }

    public string? DetallesAdicionales { get; set; }

    public DateTime? FechaAccion { get; set; }

    public string? Ipaddress { get; set; }

    public string? UserAgent { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
