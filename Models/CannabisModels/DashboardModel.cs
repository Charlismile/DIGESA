namespace DIGESA.Models.CannabisModels;

public class DashboardModel
{
    public DateTime FechaConsulta { get; set; } = DateTime.Now;
    
    // Resumen general
    public int TotalPacientesRegistrados { get; set; }
    public int TotalSolicitudesActivas { get; set; }
    public int TotalCarnetsVigentes { get; set; }
    public int TotalCarnetsVencidos { get; set; }
    
    // Solicitudes por estado
    public int SolicitudesPendientes { get; set; }
    public int SolicitudesEnRevision { get; set; }
    public int SolicitudesAprobadasHoy { get; set; }
    public int SolicitudesRechazadasHoy { get; set; }
    
    // Tipos de solicitud
    public int PrimerasInscripciones { get; set; }
    public int Renovaciones { get; set; }
    
    // Carnets por vencer
    public int CarnetsVencen30Dias { get; set; }
    public int CarnetsVencen15Dias { get; set; }
    public int CarnetsVencen7Dias { get; set; }
    public int CarnetsVencidosSinRenovar { get; set; }
    
    // Distribución geográfica
    public List<DashboardRegionModel> DistribucionPorRegion { get; set; } = new();
    
    // Histórico (últimos 6 meses)
    public List<DashboardMesModel> SolicitudesPorMes { get; set; } = new();
}

public class DashboardRegionModel
{
    public string Region { get; set; } = string.Empty;
    public int TotalPacientes { get; set; }
    public int CarnetsActivos { get; set; }
    public int CarnetsPorVencer { get; set; }
    public decimal Porcentaje { get; set; }
}

public class DashboardMesModel
{
    public string Mes { get; set; } = string.Empty;
    public int TotalSolicitudes { get; set; }
    public int SolicitudesAprobadas { get; set; }
    public int SolicitudesRechazadas { get; set; }
}