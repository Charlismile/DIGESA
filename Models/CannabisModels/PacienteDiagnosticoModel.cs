using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteDiagnosticoModel
{
    public bool IsOtroDiagSelected { get; set; } = false;
    public List<int> SelectedDiagnosticosIds { get; set; } = new();
    public string? NombreOtroDiagnostico { get; set; }
}