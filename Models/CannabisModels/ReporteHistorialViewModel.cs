namespace DIGESA.Models.CannabisModels;

public class ReporteHistorialViewModel
{
    public DateTime FechaGeneracion { get; set; } = DateTime.Now;
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    
    public int TotalEventos { get; set; }
    public int TotalPacientes { get; set; }
    public int TotalSolicitudes { get; set; }
    public int TotalRenovaciones { get; set; }
    
    public List<EventoHistorialViewModel> Eventos { get; set; } = new();
    public List<ResumenPacienteViewModel> ResumenPacientes { get; set; } = new();
    
    // Método para exportar
    public byte[] ExportarAExcel()
    {
        // Implementar lógica de exportación
        return new byte[0];
    }
    
    public byte[] ExportarAPDF()
    {
        // Implementar lógica de exportación
        return new byte[0];
    }
}