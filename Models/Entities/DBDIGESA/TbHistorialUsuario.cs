using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbHistorialUsuario
{
    public int Id { get; set; }

    public string UsuarioId { get; set; } = null!;

    public string? EstadoAnterior { get; set; }

    public string? EstadoNuevo { get; set; }

    public DateTime? FechaCambio { get; set; }

    public string? CambioPor { get; set; }

    public string? Comentario { get; set; }

    public string? TipoCambio { get; set; }
}
