namespace DIGESA.Models.CannabisModels.Estadisticas;

public class EstadisticasViewModel
{
    public int TotalPacientes { get; set; }
    public int PacientesActivos { get; set; }
    public int PacientesInactivos { get; set; }

    public int SolicitudesAprobadas { get; set; }
    public int SolicitudesRechazadas { get; set; }
    public int SolicitudesPendientes { get; set; }

    public List<ConteoGenericoViewModel> PorProvincia { get; set; } = new();
    public List<ConteoGenericoViewModel> PorDiagnostico { get; set; } = new();
}