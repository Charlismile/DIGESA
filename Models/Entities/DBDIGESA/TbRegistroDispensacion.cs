using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbRegistroDispensacion
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public int FarmaciaId { get; set; }

    public DateTime FechaDispensacion { get; set; }

    public string Producto { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public string UnidadMedida { get; set; } = null!;

    public string? LoteProducto { get; set; }

    public DateOnly? FechaVencimientoProducto { get; set; }

    public string? FarmaceuticoResponsable { get; set; }

    public string? NumeroFactura { get; set; }

    public string? Comentarios { get; set; }

    public string? UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual TbFarmaciaAutorizada Farmacia { get; set; } = null!;

    public virtual TbSolRegCannabis Solicitud { get; set; } = null!;
}
