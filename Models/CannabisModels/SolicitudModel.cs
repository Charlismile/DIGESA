namespace DIGESA.Models.CannabisModels;

public class SolicitudModel
{
    public int Id { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty; // Renombrado
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = string.Empty; // Renombrado
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty; // Renombrado
    public string? PacienteCorreo { get; set; } // Agregado
}

public class SolicitudDetalleModel : SolicitudModel
{
    public string? ComentarioRevision { get; set; }
    public DateTime? FechaRevision { get; set; } // Agregado
    public string? UsuarioRevisor { get; set; } // Agregado
    public List<string> DocumentosAdjuntos { get; set; } = new();
    public List<string> DeclaracionesJuradas { get; set; } = new(); // Renombrado
    public PacienteModel? Paciente { get; set; } // Agregado para detalle completo
}

public class SolicitudesFiltroModel
{
    public string? Estado { get; set; }
    public string? TerminoBusqueda { get; set; }
    public DateTime? FechaDesde { get; set; } // Agregado
    public DateTime? FechaHasta { get; set; } // Agregado
    public int? RegionSaludId { get; set; } // Agregado
}