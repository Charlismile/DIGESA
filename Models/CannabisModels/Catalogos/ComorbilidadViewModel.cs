using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels.Formularios;

public class ComorbilidadViewModel
{
    private string? _nombreDiagnostico;
    private string? _detalleTratamiento;

    [Required(ErrorMessage = "Seleccione si tiene comorbilidad")]
    public EnumViewModel.TieneComorbilidad TieneComorbilidadEnum { get; set; }

    [MaxLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? NombreDiagnostico 
    { 
        get => _nombreDiagnostico;
        set => _nombreDiagnostico = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }

    [MaxLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string? DetalleTratamiento 
    { 
        get => _detalleTratamiento;
        set => _detalleTratamiento = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
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