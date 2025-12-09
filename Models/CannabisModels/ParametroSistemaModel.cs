namespace DIGESA.Models.CannabisModels;

public class ParametroSistemaModel
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Valor { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // "Texto", "Entero", "Decimal", "Booleano"
    public string Descripcion { get; set; } = string.Empty;
    public DateTime? FechaModificacion { get; set; }
    public string? ModificadoPor { get; set; }
}