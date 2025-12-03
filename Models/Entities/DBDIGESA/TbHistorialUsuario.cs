using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbHistorialUsuario
{
    [Key]
    public int Id { get; set; }

    [StringLength(450)]
    public string UsuarioId { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? EstadoAnterior { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? EstadoNuevo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaCambio { get; set; }

    [StringLength(450)]
    public string? CambioPor { get; set; }

    [StringLength(500)]
    public string? Comentario { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TipoCambio { get; set; }
}
