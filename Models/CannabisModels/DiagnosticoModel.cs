using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DiagnosticoModel
{
    public List<int> SelectedDiagnosticosIds { get; set; } = new();
    public bool IsOtroDiagSelected { get; set; }
    public string? NombreOtroDiagnostico { get; set; }
}