namespace DIGESA.Models.CannabisModels;

public class FiltroPacientesViewModel
{
    public string TipoFiltro { get; set; } // "Activos", "Inactivos", "PrimeraVez", "Renovaciones"
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? ProvinciaId { get; set; }
    public int? RegionSaludId { get; set; }
    public string EstadoSolicitud { get; set; }
    public string TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; }
    public string NombrePaciente { get; set; }
    
    // Para exportación
    public bool IncluirAcompanantes { get; set; }
    public bool IncluirDiagnosticos { get; set; }
    public bool IncluirProductos { get; set; }
}