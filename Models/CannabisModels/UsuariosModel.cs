using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class UsuarioModel
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool IsAproved { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool MustChangePassword { get; set; }
}

public class UsuarioAprobacionModel : UsuarioModel
{
    public DateTime? FechaAprobacion { get; set; }
    public string AprobadoPor { get; set; } = string.Empty;
    public string Estado => IsAproved ? "Aprobado" : "Pendiente";
    
    // Historial de cambios
    public List<HistorialUsuarioModel> Historial { get; set; } = new();
}

public class HistorialUsuarioModel
{
    public int Id { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public string EstadoAnterior { get; set; } = string.Empty;
    public string EstadoNuevo { get; set; } = string.Empty;
    public DateTime FechaCambio { get; set; }
    public string CambioPor { get; set; } = string.Empty;
    public string TipoCambio { get; set; } = string.Empty;
    public string Comentario { get; set; } = string.Empty;
}

public class FiltroUsuariosModel
{
    public string? Estado { get; set; } // "Aprobado", "Pendiente", "Activo", "Inactivo"
    public string? Rol { get; set; }
    public string? TerminoBusqueda { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public bool? SoloActivos { get; set; }
}

public class CambioEstadoUsuarioModel
{
    public string UsuarioId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El nuevo estado es requerido")]
    public string Estado { get; set; } = string.Empty; // "Aprobar", "Rechazar", "Activar", "Desactivar"
    
    [Required(ErrorMessage = "El motivo es obligatorio")]
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string Motivo { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
    public string? Comentario { get; set; }
}