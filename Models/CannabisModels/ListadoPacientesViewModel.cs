namespace DIGESA.Models.CannabisModels;

public class ListadoPacientesViewModel
{
    // Filtros
    public string TipoLista { get; set; } // "Activos", "Inactivos", "PrimeraVez", "Renovaciones", "PorVencer"
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? ProvinciaId { get; set; }
    public int? RegionSaludId { get; set; }
    public string EstadoSolicitud { get; set; }
    
    // Resultados
    public List<PacienteListadoViewModel> Pacientes { get; set; } = new List<PacienteListadoViewModel>();
    
    // Estadísticas
    public int TotalRegistros { get; set; }
    public int PaginaActual { get; set; } = 1;
    public int TotalPaginas { get; set; }
    public int RegistrosPorPagina { get; set; } = 20;
}