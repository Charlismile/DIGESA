namespace DIGESA.Models.CannabisModels;

public class PacienteListadoViewModel
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; }
    public string TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public int Edad => CalcularEdad();
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public string Provincia { get; set; }
    
    // Información de la solicitud
    public string NumeroCarnet { get; set; }
    public string EstadoSolicitud { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public bool CarnetActivo { get; set; }
    public bool EsRenovacion { get; set; }
    
    // Información médica resumida
    public string DiagnosticoPrincipal { get; set; }
    public bool RequiereAcompanante { get; set; }
    
    // Para acciones
    public bool PuedeRenovar => CarnetActivo && FechaVencimiento.HasValue && 
                                FechaVencimiento.Value.AddDays(-60) <= DateTime.Now;
    public bool EstaPorVencer => FechaVencimiento.HasValue && 
                                 (FechaVencimiento.Value - DateTime.Now).Days <= 30;
    
    private int CalcularEdad()
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - FechaNacimiento.Year;
        if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
        return edad;
    }
}