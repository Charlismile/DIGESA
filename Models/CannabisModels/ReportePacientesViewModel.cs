namespace DIGESA.Models.CannabisModels;

public class ReportePacientesViewModel
{
    public List<PacienteReporteViewModel> Pacientes { get; set; } = new List<PacienteReporteViewModel>();
    public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    public string TituloReporte { get; set; }
    public Dictionary<string, string> FiltrosAplicados { get; set; } = new Dictionary<string, string>();
    public EstadisticasViewModel Estadisticas { get; set; }
}