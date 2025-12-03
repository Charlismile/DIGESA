using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class TransferenciaModel
{
    public int Id { get; set; }
    public int SolicitudId { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string UsuarioOrigenId { get; set; } = string.Empty;
    public string UsuarioOrigenNombre { get; set; } = string.Empty;
    public string UsuarioDestinoId { get; set; } = string.Empty;
    public string UsuarioDestinoNombre { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Comentario { get; set; } = string.Empty;
    public string AprobadoPor { get; set; } = string.Empty;
    
    // Niveles de aprobación
    public List<AprobacionTransferenciaModel> Aprobaciones { get; set; } = new();
}

public class AprobacionTransferenciaModel
{
    public int Id { get; set; }
    public int TransferenciaId { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public string UsuarioNombre { get; set; } = string.Empty;
    public bool? Aprobada { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public int NivelAprobacion { get; set; }
}

public class SolicitudTransferenciaModel
{
    public int SolicitudId { get; set; }
    
    [Required(ErrorMessage = "El usuario destino es requerido")]
    public string UsuarioDestinoId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El motivo es obligatorio")]
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string Motivo { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
    public string? ComentarioAdicional { get; set; }
    
    public int NivelesAprobacionRequeridos { get; set; } = 1;
}

public class AprobacionTransferenciaInputModel
{
    public int TransferenciaId { get; set; }
    
    [Required(ErrorMessage = "La decisión es requerida")]
    public bool Aprobada { get; set; }
    
    [Required(ErrorMessage = "El comentario es obligatorio")]
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string Comentario { get; set; } = string.Empty;
}