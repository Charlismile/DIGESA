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

    public int? TipoProductoId { get; set; }

    public string? OtroProductoDescripcion { get; set; }

    public int? FormaFarmaceuticaId { get; set; }

    public string? OtraFormaFarmaceuticaDescripcion { get; set; }

    public int? UnidadCbdid { get; set; }

    public int? UnidadThcid { get; set; }

    public string? OtraUnidadCbddescripcion { get; set; }

    public string? OtraUnidadThcdescripcion { get; set; }

    public string? OtroCannabinode1 { get; set; }

    public decimal? ConcentracionOtroCannabinoide1 { get; set; }

    public int? UnidadOtroCannabinoide1Id { get; set; }

    public string? OtraUnidadOtroCannabinoide1Descripcion { get; set; }

    public string? OtroCannabinode2 { get; set; }

    public decimal? ConcentracionOtroCannabinoide2 { get; set; }

    public int? UnidadOtroCannabinoide2Id { get; set; }

    public string? OtraUnidadOtroCannabinoide2Descripcion { get; set; }

    public int? ViaAdministracionId { get; set; }

    public string? OtraViaAdministracionDescripcion { get; set; }

    public int? FrecuenciaAdministracionId { get; set; }

    public int? DuracionMeses { get; set; }

    public int? DuracionDiasExtra { get; set; }

    public virtual FormaFarmaceutica? FormaFarmaceuticaNavigation { get; set; }

    public virtual FrecuenciaAdministracion? FrecuenciaAdministracionNavigation { get; set; }

    public virtual Solicitud Solicitud { get; set; } = null!;

    public virtual TipoProducto? TipoProducto { get; set; }

    public virtual UnidadConcentracion? UnidadCbdNavigation { get; set; }

    public virtual UnidadConcentracion? UnidadOtroCannabinoide1 { get; set; }

    public virtual UnidadConcentracion? UnidadOtroCannabinoide2 { get; set; }

    public virtual UnidadConcentracion? UnidadThcNavigation { get; set; }

    public virtual ViaAdministracion? ViaAdministracionNavigation { get; set; }
}
