using DIGESA.Models.CannabisModels.Estadisticas;

namespace DIGESA.Models.CannabisModels.Reportes;

public class ReportePacientesViewModel
{
    public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    public string Titulo { get; set; }

    public List<PacienteReporteViewModel> Pacientes { get; set; } = new();
    public EstadisticasViewModel Estadisticas { get; set; }
}