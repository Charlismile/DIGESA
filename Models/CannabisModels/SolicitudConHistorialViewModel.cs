namespace DIGESA.Models.CannabisModels;

public class SolicitudConHistorialViewModel : SolicitudCannabisViewModel
{
    // Información del ciclo
    public int? SolicitudPadreId { get; set; }
    public int VersionCarnet { get; set; } = 1;
    public string RazonInactivacion { get; set; }
    public DateTime? FechaInactivacion { get; set; }
    public string UsuarioInactivador { get; set; }
    
    // Historial completo
    public List<HistorialRenovacionViewModel> HistorialRenovaciones { get; set; } = new List<HistorialRenovacionViewModel>();
    public List<LogNotificacionViewModel> NotificacionesEnviadas { get; set; } = new List<LogNotificacionViewModel>();
    public List<SolicitudHistorialViewModel> CambiosEstado { get; set; } = new List<SolicitudHistorialViewModel>();
    
    // Propiedades calculadas para el ciclo
    public bool EsPrimeraSolicitud => !SolicitudPadreId.HasValue;
    public bool PuedeRenovar => ValidarSiPuedeRenovar();
    public string MotivoNoRenovacion => ObtenerMotivoNoRenovacion();
    public DateTime? FechaProximaRenovacion => CalcularFechaProximaRenovacion();
    
    private bool ValidarSiPuedeRenovar()
    {
        if (!CarnetActivo) return false;
        if (!FechaVencimientoCarnet.HasValue) return false;
        
        var config = ObtenerConfiguracionSistema();
        var diasGracia = config.DiasGraciaRenovacion;
        var maximoRenovaciones = config.MaximoRenovaciones;
        
        // No puede renovar si ya pasó el período de gracia
        if ((DateTime.Now - FechaVencimientoCarnet.Value).TotalDays > diasGracia)
            return false;
            
        // No puede renovar si ya alcanzó el máximo
        if (VersionCarnet >= maximoRenovaciones)
            return false;
            
        return true;
    }
    
    private string ObtenerMotivoNoRenovacion()
    {
        if (CarnetActivo && PuedeRenovar) return string.Empty;
        
        if (!CarnetActivo) return "Carnet inactivo";
        if (!FechaVencimientoCarnet.HasValue) return "Sin fecha de vencimiento";
        
        var config = ObtenerConfiguracionSistema();
        
        if ((DateTime.Now - FechaVencimientoCarnet.Value).TotalDays > config.DiasGraciaRenovacion)
            return $"Período de gracia excedido ({config.DiasGraciaRenovacion} días)";
            
        if (VersionCarnet >= config.MaximoRenovaciones)
            return $"Límite de renovaciones alcanzado ({config.MaximoRenovaciones})";
            
        return "No puede renovar en este momento";
    }
    
    private DateTime? CalcularFechaProximaRenovacion()
    {
        if (!FechaVencimientoCarnet.HasValue) return null;
        
        var config = ObtenerConfiguracionSistema();
        var diasAntes = config.DiasAntesNotificar;
        
        return FechaVencimientoCarnet.Value.AddDays(-diasAntes);
    }
    
    private ConfiguracionSistemaViewModel ObtenerConfiguracionSistema()
    {
        // Este método normalmente obtendría la configuración de una base de datos o caché
        return new ConfiguracionSistemaViewModel
        {
            // DiasVigenciaCarnet = 730,
            // DiasAntesNotificar = 30,
            // DiasGraciaRenovacion = 90,
            // MaximoRenovaciones = 10
        };
    }
}