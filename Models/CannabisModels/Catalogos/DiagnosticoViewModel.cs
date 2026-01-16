using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels.Formularios;

public class DiagnosticoViewModel
{
    private string? _nombreOtroDiagnostico;

    [Required(ErrorMessage = "Seleccione al menos un diagnóstico")]
    public List<int> SelectedDiagnosticosIds { get; set; } = new();

    // Agregar esta propiedad
    public bool IsOtroDiagSelected { get; set; }

    public string? NombreOtroDiagnostico 
    { 
        get => _nombreOtroDiagnostico;
        set => _nombreOtroDiagnostico = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }

    private string NormalizarTexto(string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;

        texto = texto.Trim().ToUpperInvariant();
        
        while (texto.Contains("  "))
            texto = texto.Replace("  ", " ");
            
        return texto;
    }
}