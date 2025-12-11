namespace DIGESA.Models.CannabisModels;

public class EventoHistorialViewModel
{
    public DateTime Fecha { get; set; }
    public string Tipo { get; set; } // "Solicitud", "Aprobación", "Renovación", "Inactivación", "Notificación"
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public string Icono { get; set; }
    public string Color { get; set; } // CSS color class
    public string Usuario { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
}