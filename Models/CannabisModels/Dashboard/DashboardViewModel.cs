namespace DIGESA.Models.CannabisModels.Dashboard;

public class DashboardViewModel
{
    public int TotalPacientes { get; set; }
    public int PacientesActivos { get; set; }
    public int CarnetsPorVencer { get; set; }
    public int SolicitudesPendientes { get; set; }

    public List<ChartDataViewModel> PacientesPorProvincia { get; set; } = new();
    public List<ChartDataViewModel> SolicitudesPorMes { get; set; } = new();
}