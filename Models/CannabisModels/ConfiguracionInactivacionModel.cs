namespace DIGESA.Models.CannabisModels;

public class ConfiguracionInactivacionModel
{
    public bool ActivarInactivacionAutomatica { get; set; } = true;
    public int DiasInactivacionDespuesVencimiento { get; set; } = 0;
    public bool NotificarInactivacion { get; set; } = true;
    public string EmailNotificacionInactivacion { get; set; } = "cannabis@digesa.gob.pa";
}