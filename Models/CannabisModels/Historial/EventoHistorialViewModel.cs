namespace DIGESA.Models.CannabisModels.Historial;

public class EventoHistorialViewModel
{
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public string Icono { get; set; }
    public string Color { get; set; }
    public string Usuario { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}