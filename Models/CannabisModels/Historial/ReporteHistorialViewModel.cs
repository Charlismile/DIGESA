namespace DIGESA.Models.CannabisModels.Historial;

public class ReporteHistorialViewModel
{
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaGeneracion { get; set; }

    public int TotalEventos { get; set; }
    public int TotalPacientes { get; set; }
    public int TotalSolicitudes { get; set; }
    public int TotalRenovaciones { get; set; }

    public List<EventoHistorialViewModel> Eventos { get; set; } = new();
    public List<ResumenPacienteViewModel> ResumenPacientes { get; set; } = new();
}