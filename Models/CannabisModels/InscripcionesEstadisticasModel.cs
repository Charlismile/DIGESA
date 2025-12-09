namespace DIGESA.Models.CannabisModels;

public class InscripcionesEstadisticasModel
{
    public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    public string TerminosBusqueda { get; set; } = string.Empty;
    
    public int TotalSolicitudes { get; set; }
    public int PrimerasInscripciones { get; set; }
    public int Renovaciones { get; set; }
    
    public int SolicitudesAprobadas { get; set; }
    public int SolicitudesPendientes { get; set; }
    public int SolicitudesRechazadas { get; set; }
    
    public int CarnetsActivos { get; set; }
    public int CarnetsInactivos { get; set; }
    public int CarnetsPorVencer30Dias { get; set; }
    public int CarnetsPorVencer15Dias { get; set; }
    public int CarnetsPorVencer7Dias { get; set; }
    public int CarnetsVencidos { get; set; }
    
    public Dictionary<string, InscripcionesMesModel> SolicitudesPorMes { get; set; } = new();
    public List<EstadisticaRegionModel> SolicitudesPorRegion { get; set; } = new();
}