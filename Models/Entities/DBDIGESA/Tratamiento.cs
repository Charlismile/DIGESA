using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Tratamiento
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string? NombreGenericoProducto { get; set; }

    public string? NombreComercialProducto { get; set; }

    public string FormaFarmaceutica { get; set; } = null!;

    public decimal? ConcentracionCbd { get; set; }

    public string? UnidadCbd { get; set; }

    public decimal? ConcentracionThc { get; set; }

    public string? UnidadThc { get; set; }

    public string? OtrosCannabinoides { get; set; }

    public string ViaAdministracion { get; set; } = null!;

    public string Dosis { get; set; } = null!;

    public string FrecuenciaAdministracion { get; set; } = null!;

    public int DuracionTratamientoDias { get; set; }

    public string? CantidadPrescrita { get; set; }

    public string? InstruccionesAdicionales { get; set; }

    public DateOnly? FechaInicioTratamientoPrevista { get; set; }

    public virtual Solicitud Solicitud { get; set; } = null!;
}
