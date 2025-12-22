namespace DIGESA.Models.CannabisModels.Reportes;

public class PacienteReporteViewModel
{
    public string NumeroCarnet { get; set; }
    public string NombreCompleto { get; set; }
    public string Documento { get; set; }
    public int Edad { get; set; }
    public string Provincia { get; set; }

    public string Diagnostico { get; set; }
    public string Medico { get; set; }

    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; }
}