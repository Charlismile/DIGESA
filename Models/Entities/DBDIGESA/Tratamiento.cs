using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Tratamiento
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string? NombreGenericoProducto { get; set; }

    public string? NombreComercialProducto { get; set; }

    public decimal? ConcentracionCbd { get; set; }

    public int? UnidadCbdid { get; set; }

    public decimal? ConcentracionThc { get; set; }

    public int? UnidadThcid { get; set; }

    public string? OtrosCannabinoides { get; set; }

    public string Dosis { get; set; } = null!;

    public string FrecuenciaAdministracion { get; set; } = null!;

    public int DuracionTratamientoDias { get; set; }

    public string? CantidadPrescrita { get; set; }

    public string? InstruccionesAdicionales { get; set; }

    public DateOnly? FechaInicioTratamientoPrevista { get; set; }

    public virtual Solicitud Solicitud { get; set; } = null!;

    public virtual ICollection<TratamientoCannabinoide> TratamientoCannabinoide { get; set; } = new List<TratamientoCannabinoide>();

    public virtual ICollection<TratamientoFormaFarmaceutica> TratamientoFormaFarmaceutica { get; set; } = new List<TratamientoFormaFarmaceutica>();

    public virtual ICollection<TratamientoViaAdministracion> TratamientoViaAdministracion { get; set; } = new List<TratamientoViaAdministracion>();

    public virtual UnidadMedida? UnidadCbd { get; set; }

    public virtual UnidadMedida? UnidadThc { get; set; }
}
