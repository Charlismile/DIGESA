using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Models.CannabisModels;

public class SolicitudModel
{
    public int Id { get; set; }
    public string? NumSolCompleta { get; set; }
    public DateTime? FechaSolicitud { get; set; }
    public string? EstadoSolicitud { get; set; }
    public string? PacienteNombre { get; set; }
    public string? PacienteCedula { get; set; }
}