namespace DIGESA.Models.CannabisModels.Configuracion;

public class ConfiguracionSistemaViewModel
{
    public int Id { get; set; }

    public string Clave { get; set; } = string.Empty;

    public string Valor { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public string? Grupo { get; set; }

    public bool EsEditable { get; set; }

    /* ======================
       PROPIEDADES CALCULADAS
       ====================== */

    public int ValorEntero
        => int.TryParse(Valor, out var v) ? v : 0;

    public bool ValorBooleano
        => bool.TryParse(Valor, out var v) && v;
}