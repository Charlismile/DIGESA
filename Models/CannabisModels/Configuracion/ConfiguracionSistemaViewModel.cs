namespace DIGESA.Models.CannabisModels.Configuracion;

public class ConfiguracionSistemaViewModel
{
    public int DiasVigenciaCarnet { get; set; } = 730;
    public int DiasAvisoVencimiento { get; set; } = 30;

    public bool NotificarEmail { get; set; } = true;
    public bool NotificarSMS { get; set; }
}