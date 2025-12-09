using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class NotificacionVencimientoModel
{
    public int Id { get; set; }
    public int SolicitudId { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string NumeroCarnet { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteCorreo { get; set; } = string.Empty;
    public DateTime FechaVencimiento { get; set; }
    public int DiasRestantes { get; set; }
    public int DiasAntelacion { get; set; }
    public DateTime? FechaEnvio { get; set; }
    public bool EmailEnviado { get; set; }
    public string TipoNotificacion { get; set; } = string.Empty;
    public string EstadoNotificacion => EmailEnviado ? "Enviada" : "Pendiente";
}

public class EmailNotificacionModel
{
    public string Destinatario { get; set; } = string.Empty;
    public string Asunto { get; set; } = string.Empty;
    public string Cuerpo { get; set; } = string.Empty;
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string NombrePaciente { get; set; } = string.Empty;
    public string EstadoSolicitud { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public DateTime? FechaVencimiento { get; set; }
    public int? DiasRestantes { get; set; }
}

public class DashboardVencimientosModel
{
    public int CarnetsPorVencer30Dias { get; set; }
    public int CarnetsPorVencer15Dias { get; set; }
    public int CarnetsPorVencer7Dias { get; set; }
    public int CarnetsVencidos { get; set; }
    public int CarnetsVigentes { get; set; }
    
    public List<NotificacionVencimientoModel> ProximosVencimientos { get; set; } = new();
    public List<NotificacionVencimientoModel> NotificacionesRecientes { get; set; } = new();
}