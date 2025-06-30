using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class Tratamiento
{
    public int TratamientoId { get; set; }

    public int TratamientoSolicitudId { get; set; }

    public string? TratamientoNombreGenericoProducto { get; set; }

    public string? TratamientoNombreComercialProducto { get; set; }

    public decimal? TratamientoConcentracionCbd { get; set; }

    public int? TratamientoUnidadCbdid { get; set; }

    public decimal? TratamientoConcentracionThc { get; set; }

    public int? TratamientoUnidadThcid { get; set; }

    public string? TratamientoOtrosCannabinoides { get; set; }

    public string TratamientoDosis { get; set; } = null!;

    public string TratamientoFrecuenciaAdministracion { get; set; } = null!;

    public int TratamientoDuracionTratamientoDias { get; set; }

    public string? TratamientoCantidadPrescrita { get; set; }

    public string? TratamientoInstruccionesAdicionales { get; set; }

    public DateOnly? TratamientoFechaInicioTratamientoPrevista { get; set; }

    public DateOnly? TratamientoFechaFinEstimada { get; set; }

    public string TratamientoEstado { get; set; } = null!;

    public virtual ICollection<TratamientoFormaFarmaceutica> TratamientoFormaFarmaceutica { get; set; } = new List<TratamientoFormaFarmaceutica>();

    public virtual Solicitud TratamientoSolicitud { get; set; } = null!;

    public virtual ICollection<TratamientoViaAdministracion> TratamientoViaAdministracion { get; set; } = new List<TratamientoViaAdministracion>();
}
