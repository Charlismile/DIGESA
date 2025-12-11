using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ConfiguracionCompletaViewModel
{
    // Configuraciones de vencimientos
    [Range(1, 3650, ErrorMessage = "Los días de vigencia deben estar entre 1 y 3650")]
    public int DiasVigenciaCarnet { get; set; } = 730; // 2 años
    
    [Range(1, 365, ErrorMessage = "Los días para notificar deben estar entre 1 y 365")]
    public int DiasAntesNotificar { get; set; } = 30;
    
    [Range(0, 365, ErrorMessage = "Los días de gracia deben estar entre 0 y 365")]
    public int DiasGraciaRenovacion { get; set; } = 90;
    
    [Range(0, 365, ErrorMessage = "Los días para autoinactivar deben estar entre 0 y 365")]
    public int AutoInactivarDias { get; set; } = 0; // 0 = manual
    
    [Range(1, 100, ErrorMessage = "El máximo de renovaciones debe estar entre 1 y 100")]
    public int MaximoRenovaciones { get; set; } = 10;
    
    // Configuraciones de notificaciones
    public bool NotificarPorEmail { get; set; } = true;
    public bool NotificarPorSMS { get; set; } = false;
    public string EmailRemitente { get; set; }
    public string SMSSender { get; set; }
    
    // Configuraciones del sistema
    public string NombreSistema { get; set; } = "Sistema de Identificación de Cannabis Medicinal";
    public string LogoUrl { get; set; }
    public string ColorPrimario { get; set; } = "#0d6efd";
    public int ItemsPorPagina { get; set; } = 20;
    
    // Configuraciones de carnet
    public int AnchoCarnet { get; set; } = 85; // mm
    public int AltoCarnet { get; set; } = 54; // mm
    public bool IncluirQR { get; set; } = true;
    public bool IncluirBarras { get; set; } = true;
    
    // Método para convertir a lista de entidades
    public List<ConfiguracionSistemaViewModel> ToConfiguracionesList()
    {
        return new List<ConfiguracionSistemaViewModel>
        {
            new ConfiguracionSistemaViewModel { Clave = "DiasVigenciaCarnet", Valor = DiasVigenciaCarnet.ToString(), Grupo = "Vencimientos", Descripcion = "Días de vigencia del carnet" },
            new ConfiguracionSistemaViewModel { Clave = "DiasAntesNotificar", Valor = DiasAntesNotificar.ToString(), Grupo = "Vencimientos", Descripcion = "Días antes para notificar vencimiento" },
            new ConfiguracionSistemaViewModel { Clave = "DiasGraciaRenovacion", Valor = DiasGraciaRenovacion.ToString(), Grupo = "Vencimientos", Descripcion = "Días de gracia después del vencimiento" },
            new ConfiguracionSistemaViewModel { Clave = "AutoInactivarDias", Valor = AutoInactivarDias.ToString(), Grupo = "Vencimientos", Descripcion = "Días después del vencimiento para autoinactivar" },
            new ConfiguracionSistemaViewModel { Clave = "MaximoRenovaciones", Valor = MaximoRenovaciones.ToString(), Grupo = "Vencimientos", Descripcion = "Máximo número de renovaciones permitidas" },
            new ConfiguracionSistemaViewModel { Clave = "NotificarPorEmail", Valor = NotificarPorEmail.ToString(), Grupo = "Notificaciones", Descripcion = "Habilitar notificaciones por email" },
            new ConfiguracionSistemaViewModel { Clave = "NotificarPorSMS", Valor = NotificarPorSMS.ToString(), Grupo = "Notificaciones", Descripcion = "Habilitar notificaciones por SMS" },
            new ConfiguracionSistemaViewModel { Clave = "ItemsPorPagina", Valor = ItemsPorPagina.ToString(), Grupo = "Sistema", Descripcion = "Items por página en listados" }
        };
    }
}