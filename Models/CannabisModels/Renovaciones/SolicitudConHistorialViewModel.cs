namespace DIGESA.Models.CannabisModels.Renovaciones;

public class SolicitudConHistorialViewModel : SolicitudCannabisViewModel
{
    public int? SolicitudPadreId { get; set; }
    public int VersionCarnet { get; set; }

    public string? RazonInactivacion { get; set; }
    public DateTime? FechaInactivacion { get; set; }
    public string? UsuarioInactivador { get; set; }

    public PacienteViewModel? Paciente { get; set; }
    public EstadoSolicitudViewModel? EstadoSolicitud { get; set; }
}