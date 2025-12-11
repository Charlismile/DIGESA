namespace DIGESA.Models.CannabisModels;

public class ReporteMedicosViewModel
{
    public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    
    // Estadísticas
    public int TotalMedicos { get; set; }
    public int MedicosVerificados { get; set; }
    public int MedicosActivos { get; set; }
    public int MedicosEspecialistasCannabis { get; set; }
    
    // Distribución por especialidad
    public List<EspecialidadCountViewModel> DistribucionEspecialidad { get; set; } = new();
    
    // Distribución por provincia
    public List<ProvinciaCountViewModel> DistribucionProvincia { get; set; } = new();
    
    // Médicos registrados por mes
    public List<RegistroMensualViewModel> RegistrosMensuales { get; set; } = new();
    
    // Lista detallada
    public List<MedicoReporteViewModel> Medicos { get; set; } = new();
}