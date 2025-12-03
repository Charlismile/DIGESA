using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class SolicitudBaseModel
{
    public int Id { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = string.Empty;
    public bool EsRenovacion { get; set; }
}

public class SolicitudListModel : SolicitudBaseModel
{
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public string PacienteCorreo { get; set; } = string.Empty;
    public string? NumeroCarnet { get; set; }
    public bool CarnetActivo { get; set; }
    public DateTime? FechaVencimientoCarnet { get; set; }
}

public class SolicitudDetalleModel : SolicitudBaseModel
{
    public PacienteModel? Paciente { get; set; }
    public AcompananteModel? Acompanante { get; set; }
    public MedicoModel? Medico { get; set; }
    public List<DiagnosticoModel> Diagnosticos { get; set; } = new();
    public List<ComorbilidadModel> Comorbilidades { get; set; } = new();
    public List<ProductoPacienteModel> Productos { get; set; } = new();
    public List<DeclaracionJuradaModel> DeclaracionesJuradas { get; set; } = new();
    public List<DocumentoAdjuntoModel> Documentos { get; set; } = new();
    
    // Historial
    public string? ComentarioRevision { get; set; }
    public DateTime? FechaRevision { get; set; }
    public string? UsuarioRevisor { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public DateTime? FechaEmisionCarnet { get; set; }
    public DateTime? FechaVencimientoCarnet { get; set; }
    public string? NumeroCarnet { get; set; }
    public bool CarnetActivo { get; set; }
}

public class DocumentoAdjuntoModel
{
    public int Id { get; set; }
    public string NombreOriginal { get; set; } = string.Empty;
    public string NombreGuardado { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = string.Empty;
    public DateTime FechaSubida { get; set; }
    public bool EsValido { get; set; }
    public bool EsDocumentoMedico { get; set; }
}

public class RegistroCannabisModel
{
    public PacienteModel Paciente { get; set; } = new();
    public AcompananteModel? Acompanante { get; set; }
    public MedicoModel Medico { get; set; } = new();
    public List<DiagnosticoModel> Diagnosticos { get; set; } = new();
    public ComorbilidadModel? Comorbilidad { get; set; }
    public List<ProductoPacienteModel> Productos { get; set; } = new();
    public List<DeclaracionJuradaModel> DeclaracionesJuradas { get; set; } = new();
    public bool EsRenovacion { get; set; }
    public int? SolicitudRenovadaId { get; set; }
}

public class EvaluacionSolicitudModel
{
    public int SolicitudId { get; set; }
    
    [Required(ErrorMessage = "La acción es requerida")]
    public string Accion { get; set; } = string.Empty; // "Aprobar", "Rechazar", "Documentacion"
    
    [Required(ErrorMessage = "El motivo es obligatorio")]
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string Motivo { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
    public string? ComentarioAdicional { get; set; }
}

public class ResultadoEvaluacionModel : ResultModel
{
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string EmailDestinatario { get; set; } = string.Empty;
    public DateTime FechaEvaluacion { get; set; } = DateTime.Now;
    public string? NumeroCarnet { get; set; }
    public DateTime? FechaVencimientoCarnet { get; set; }
}