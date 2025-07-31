
namespace DIGESA.Models.CannabisModels;

public class DeclaracionJuradaModel
{
    public int Id { get; set; }
    public string? Detalle { get; set; } = "";
    public DateTime Fecha { get; set; }
    public string? NombreDeclarante { get; set; } = "";
    public bool Aceptada { get; set; }
}