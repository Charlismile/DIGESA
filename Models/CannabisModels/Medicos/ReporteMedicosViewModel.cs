using DIGESA.Models.CannabisModels.Reportes;

namespace DIGESA.Models.CannabisModels.Medicos;

public class ReporteMedicosViewModel
{
    public DateTime FechaGeneracion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }

    public int TotalMedicos { get; set; }
    public int MedicosVerificados { get; set; }
    public int MedicosActivos { get; set; }
    public int MedicosEspecialistasCannabis { get; set; }

    public List<EspecialidadCountViewModel> DistribucionEspecialidad { get; set; } = new();
    public List<ProvinciaCountViewModel> DistribucionProvincia { get; set; } = new();
    public List<RegistroMensualViewModel> RegistrosMensuales { get; set; } = new();

    public List<MedicoReporteViewModel> Medicos { get; set; } = new();
}