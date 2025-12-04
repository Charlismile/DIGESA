using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ReporteFiltrosModel
{
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Estado { get; set; }
    public int? ProvinciaId { get; set; }
    public int? RegionSaludId { get; set; }
    public TipoInscripcion? TipoInscripcion { get; set; }
    public bool? CarnetActivo { get; set; }
    public string? TipoDocumento { get; set; }
    public Sexo? Sexo { get; set; }
    public int? EdadMin { get; set; }
    public int? EdadMax { get; set; }
    public string? TerminoBusqueda { get; set; }
}

public class ReporteGeneradoModel
{
    public int Id { get; set; }
    public string NombreArchivo { get; set; } = string.Empty;
    public string TipoReporte { get; set; } = string.Empty;
    public TipoExportacion Formato { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public string GeneradoPor { get; set; } = string.Empty;
    public string FiltrosAplicados { get; set; } = string.Empty;
    public string RutaArchivo { get; set; } = string.Empty;
    public long TamanoBytes { get; set; }
    public bool Descargado { get; set; }
}

public class EstadisticasDashboardModel
{
    public int TotalSolicitudes { get; set; }
    public int SolicitudesAprobadas { get; set; }
    public int SolicitudesPendientes { get; set; }
    public int SolicitudesRechazadas { get; set; }
    public int TotalUsuarios { get; set; }
    public int UsuariosAprobados { get; set; }
    public int UsuariosPendientes { get; set; }
    public int TotalPacientes { get; set; }
    public int CarnetsActivos { get; set; }
    public int CarnetsPorVencer { get; set; }
    
    // Estadísticas por región
    public List<EstadisticasPorRegion> SolicitudesPorRegion { get; set; } = new();
    
    // Estadísticas por mes
    public Dictionary<string, int> SolicitudesPorMes { get; set; } = new();
    public Dictionary<string, int> AprobacionesPorMes { get; set; } = new();
}

public class EstadisticasPorRegion
{
    public string Region { get; set; } = string.Empty;
    public int TotalSolicitudes { get; set; }
    public int SolicitudesAprobadas { get; set; }
    public int SolicitudesPendientes { get; set; }
    public int SolicitudesRechazadas { get; set; }
}

public class ExportacionRequestModel
{
    [Required(ErrorMessage = "El formato es requerido")]
    public TipoExportacion Formato { get; set; }
    
    public ReporteFiltrosModel? Filtros { get; set; }
    public List<string> CamposIncluir { get; set; } = new();
}
public class SolicitudReporteModel
{
    public int Id { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public string PacienteCorreo { get; set; } = string.Empty;
    public string PacienteTelefono { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Instalacion { get; set; } = string.Empty;
    public bool EsRenovacion { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public string? NumeroCarnet { get; set; }
    public bool CarnetActivo { get; set; }
    public DateTime? FechaVencimiento { get; set; }
}

public class PacienteReporteModel
{
    public int Id { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Instalacion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class CarnetReporteModel
{
    public string NumeroCarnet { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public string PacienteCorreo { get; set; } = string.Empty;
    public string PacienteTelefono { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public bool Activo { get; set; }
    public string Region { get; set; } = string.Empty;
    public string Instalacion { get; set; } = string.Empty;
}