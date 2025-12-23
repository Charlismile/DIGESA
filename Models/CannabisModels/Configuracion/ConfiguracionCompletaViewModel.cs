namespace DIGESA.Models.CannabisModels.Configuracion;

public class ConfiguracionCompletaViewModel
{
    // Vencimientos
    public int DiasVigenciaCarnet { get; set; }
    public int DiasAntesNotificar { get; set; }
    public int DiasGraciaRenovacion { get; set; }
    public int AutoInactivarDias { get; set; }

    // Renovaciones
    public int MaximoRenovaciones { get; set; }

    // Notificaciones
    public bool NotificarPorEmail { get; set; }
    public bool NotificarPorSMS { get; set; }

    // UI
    public int ItemsPorPagina { get; set; }
    public string? EmailRemitente { get; set; }

    /* =========================
       MAPEO INVERSO A ENTIDAD
       ========================= */
    public List<ConfiguracionSistemaViewModel> ToConfiguracionesList()
    {
        return new List<ConfiguracionSistemaViewModel>
        {
            Nueva("DiasVigenciaCarnet", DiasVigenciaCarnet, "Vencimientos"),
            Nueva("DiasAntesNotificar", DiasAntesNotificar, "Notificaciones"),
            Nueva("DiasGraciaRenovacion", DiasGraciaRenovacion, "Renovaciones"),
            Nueva("AutoInactivarDias", AutoInactivarDias, "Vencimientos"),
            Nueva("MaximoRenovaciones", MaximoRenovaciones, "Renovaciones"),
            Nueva("NotificarPorEmail", NotificarPorEmail, "Notificaciones"),
            Nueva("NotificarPorSMS", NotificarPorSMS, "Notificaciones"),
            Nueva("ItemsPorPagina", ItemsPorPagina, "UI"),
            Nueva("EmailRemitente", EmailRemitente ?? "", "Notificaciones")
        };
    }

    private static ConfiguracionSistemaViewModel Nueva(
        string clave,
        object valor,
        string grupo
    )
    {
        return new ConfiguracionSistemaViewModel
        {
            Clave = clave,
            Valor = valor.ToString()!,
            Grupo = grupo,
            EsEditable = true
        };
    }
}