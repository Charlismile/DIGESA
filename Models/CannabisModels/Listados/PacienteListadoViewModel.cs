namespace DIGESA.Models.CannabisModels.Listados;

public class PacienteListadoViewModel
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; }
    public string Documento { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public int Edad { get; set; }

    public string Provincia { get; set; }
    public string Telefono { get; set; }

    public string NumeroCarnet { get; set; }
    public string EstadoSolicitud { get; set; }
    public DateTime? FechaVencimiento { get; set; }

    public bool CarnetActivo { get; set; }
    public bool EstaPorVencer =>
        FechaVencimiento.HasValue &&
        (FechaVencimiento.Value - DateTime.Now).Days <= 30;
}