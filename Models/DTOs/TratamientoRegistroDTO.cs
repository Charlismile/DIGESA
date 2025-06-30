namespace DIGESA.Models.DTOs;

public class TratamientoRegistroDTO
{
    public decimal? ConcentracionCBD { get; set; }
    public decimal? ConcentracionTHC { get; set; }
    public string OtrosCannabinoides { get; set; } = string.Empty;
    public string Dosis { get; set; } = string.Empty;
    public string FrecuenciaAdministracion { get; set; } = string.Empty;
    public int DuracionTratamientoDias { get; set; }
    public string CantidadPrescrita { get; set; } = string.Empty;
    public string InstruccionesAdicionales { get; set; } = string.Empty;
}