namespace DIGESA.Models.CannabisModels;

public class EstadisticasViewModel
{
    // Totales generales
    public int TotalPacientesRegistrados { get; set; }
    public int PacientesActivos { get; set; }
    public int PacientesInactivos { get; set; }
    public int SolicitudesAprobadas { get; set; }
    public int SolicitudesRechazadas { get; set; }
    public int SolicitudesPendientes { get; set; }
    
    // Por tipo de solicitud
    public int SolicitudesPrimeraVez { get; set; }
    public int SolicitudesRenovacion { get; set; }
    
    // Por período
    public int SolicitudesEsteMes { get; set; }
    public int SolicitudesMesAnterior { get; set; }
    public decimal VariacionPorcentual { get; set; }
    
    // Distribución geográfica
    public List<EstadisticaProvinciaViewModel> DistribucionPorProvincia { get; set; } = new List<EstadisticaProvinciaViewModel>();
    
    // Distribución por diagnóstico
    public List<EstadisticaDiagnosticoViewModel> DistribucionPorDiagnostico { get; set; } = new List<EstadisticaDiagnosticoViewModel>();
    
    // Carnets próximos a vencer
    public int CarnetsPorVencer30Dias { get; set; }
    public int CarnetsPorVencer60Dias { get; set; }
    public int CarnetsVencidos { get; set; }
    
    // Por edad
    public int MenoresDe18 { get; set; }
    public int Entre18y65 { get; set; }
    public int MayoresDe65 { get; set; }
}