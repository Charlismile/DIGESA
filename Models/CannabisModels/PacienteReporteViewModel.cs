namespace DIGESA.Models.CannabisModels;

public class PacienteReporteViewModel
{
    // Información personal
    public string NumeroCarnet { get; set; }
    public string NombreCompleto { get; set; }
    public string DocumentoIdentidad { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public int Edad { get; set; }
    public string Sexo { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    
    // Ubicación
    public string Provincia { get; set; }
    public string Distrito { get; set; }
    public string Direccion { get; set; }
    
    // Información médica
    public string DiagnosticoPrincipal { get; set; }
    public string ProductoPrescrito { get; set; }
    public string Dosis { get; set; }
    public string MedicoTratante { get; set; }
    public string CentroSalud { get; set; }
    
    // Estado del carnet
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public string Estado { get; set; }
    public bool RequiereAcompanante { get; set; }
    public string Acompanante { get; set; }
    
    // Para Excel
    public bool IncluirEnExcel { get; set; } = true;
}