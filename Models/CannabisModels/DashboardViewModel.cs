namespace DIGESA.Models.CannabisModels;

public class DashboardViewModel
{
    // Estadísticas principales
    public int TotalPacientes { get; set; }
    public int PacientesActivos { get; set; }
    public int PacientesInactivos { get; set; }
    public int SolicitudesPrimeraVez { get; set; }
    public int SolicitudesRenovacion { get; set; }
    public int CarnetsPorVencer { get; set; }
    public int CarnetsVencidos { get; set; }
    
    // Distribución por mes
    public List<ChartDataViewModel> SolicitudesPorMes { get; set; }
    
    // Distribución por provincia
    public List<ChartDataViewModel> PacientesPorProvincia { get; set; }
    
    // Solicitudes recientes
    public List<SolicitudCannabisViewModel> SolicitudesRecientes { get; set; }
    
    // Próximos vencimientos
    public List<SolicitudCannabisViewModel> ProximosVencimientos { get; set; }
}