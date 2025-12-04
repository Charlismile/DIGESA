namespace DIGESA.Models.CannabisModels;

// Para el estado del paciente
public class PacienteEstadoModel
{
    public string Documento { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public DateTime? FechaVencimiento { get; set; }
    public bool Activo { get; set; }
    public string EstadoSolicitud { get; set; } = string.Empty;
    public DateTime? FechaAprobacion { get; set; }
    
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
}

// Para filtros de solicitudes
public class SolicitudesFiltroModel
{
    public string? Estado { get; set; }
    public string? TerminoBusqueda { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? RegionSaludId { get; set; }
}

// Modelos para API
public class ApiSolicitudModel
{
    public string NumeroSolicitud { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool EsRenovacion { get; set; }
    public PacienteApiModel Paciente { get; set; } = new();
    public List<ProductoApiModel> Productos { get; set; } = new();
    public DateTime? FechaEmisionCarnet { get; set; }
    public DateTime? FechaVencimientoCarnet { get; set; }
    public string? NumeroCarnet { get; set; }
    public bool CarnetActivo { get; set; }
}

public class PacienteApiModel
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string NumeroDocumento { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string TelefonoPersonal { get; set; } = string.Empty;
    public string RegionSalud { get; set; } = string.Empty;
    public string InstalacionSalud { get; set; } = string.Empty;
}

public class ProductoApiModel
{
    public string NombreComercial { get; set; } = string.Empty;
    public string FormaFarmaceutica { get; set; } = string.Empty;
    public string ViaAdministracion { get; set; } = string.Empty;
    public string Concentracion { get; set; } = string.Empty;
    public string DetalleDosis { get; set; } = string.Empty;
}