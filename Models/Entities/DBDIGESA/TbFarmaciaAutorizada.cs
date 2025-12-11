using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbFarmaciaAutorizada
{
    public int Id { get; set; }

    public string CodigoFarmacia { get; set; } = null!;

    public string NombreFarmacia { get; set; } = null!;

    public string Ruc { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public int ProvinciaId { get; set; }

    public int DistritoId { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Responsable { get; set; }

    public DateTime FechaAutorizacion { get; set; }

    public DateTime? FechaVencimientoAutorizacion { get; set; }

    public bool Activo { get; set; }

    public string? UsuarioRegistro { get; set; }

    public DateTime FechaRegistro { get; set; }

    public virtual TbDistrito Distrito { get; set; } = null!;

    public virtual TbProvincia Provincia { get; set; } = null!;

    public virtual ICollection<TbRegistroDispensacion> TbRegistroDispensacion { get; set; } = new List<TbRegistroDispensacion>();
}
