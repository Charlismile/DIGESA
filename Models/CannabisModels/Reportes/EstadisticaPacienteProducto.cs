namespace DIGESA.Models.CannabisModels.Reportes;

public class EstadisticaPacienteProducto
{
    public string NombreProducto { get; set; } = string.Empty;
    public int CantidadPacientes { get; set; }
    public string FormaFarmaceutica { get; set; } = string.Empty;
    public string ViaAdministracion { get; set; } = string.Empty;
    public string ConcentracionPrincipal { get; set; } = string.Empty;
    public decimal CantidadConcentracionPromedio { get; set; }
    public DateTime FechaUltimoUso { get; set; }
    public int CantidadTotalProductos { get; set; }
}