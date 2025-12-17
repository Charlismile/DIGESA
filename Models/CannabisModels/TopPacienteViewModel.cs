namespace DIGESA.Models.CannabisModels;

public class TopPacienteViewModel
{
    public int PacienteId { get; set; }
    public string PacienteNombre { get; set; }
    public string PacienteCedula { get; set; }
    public int CantidadDispensaciones { get; set; }
    public decimal CantidadTotal { get; set; }
}