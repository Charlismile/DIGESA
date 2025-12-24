namespace DIGESA.Models.CannabisModels.Catalogos;

public class DiagnosticoViewModel
{
    public List<int> SelectedDiagnosticosIds { get; set; } = new();
    public string? NombreOtroDiagnostico { get; set; }
    public bool IsOtroDiagSelected { get; set; }

}
