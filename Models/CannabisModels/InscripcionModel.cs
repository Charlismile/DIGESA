namespace DIGESA.Models.CannabisModels;

public class InscripcionPacienteModel
{
    public int Id { get; set; }
    public string NumeroCarnet { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public TipoInscripcion TipoInscripcion { get; set; }
    public DateTime? FechaPrimeraInscripcion { get; set; }
    public DateTime? FechaUltimaRenovacion { get; set; }
    public int TotalRenovaciones { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public bool CarnetActivo { get; set; }
    public string EstadoSolicitud { get; set; } = string.Empty;
    public string RegionSalud { get; set; } = string.Empty;
    public string InstalacionSalud { get; set; } = string.Empty;
}

public class EstadisticasInscripcionesModel
{
    public int TotalPacientes { get; set; }
    public int PrimerasInscripciones { get; set; }
    public int Renovaciones { get; set; }
    public int CarnetsVigentes { get; set; }
    public int CarnetsPorVencer30Dias { get; set; }
    public int CarnetsPorVencer15Dias { get; set; }
    public int CarnetsPorVencer7Dias { get; set; }
    public int CarnetsVencidos { get; set; }
    
    // Estadísticas por mes
    public Dictionary<string, int> InscripcionesPorMes { get; set; } = new();
    public Dictionary<string, int> RenovacionesPorMes { get; set; } = new();
}

public class FiltroInscripcionesModel
{
    public TipoInscripcion? Tipo { get; set; }
    public string? Estado { get; set; }
    public int? RegionSaludId { get; set; }
    public int? InstalacionSaludId { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public bool? CarnetActivo { get; set; }
    public string? TerminoBusqueda { get; set; }
}