namespace DIGESA.Models.CannabisModels;

public abstract class PersonaBaseViewModel
{
    private string _primerNombre = string.Empty;
    private string? _segundoNombre;
    private string _primerApellido = string.Empty;
    private string? _segundoApellido;
    private string _numeroDocumento = string.Empty;

    public string PrimerNombre 
    { 
        get => _primerNombre;
        set => _primerNombre = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }
    
    public string? SegundoNombre 
    { 
        get => _segundoNombre;
        set => _segundoNombre = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }
    
    public string PrimerApellido 
    { 
        get => _primerApellido;
        set => _primerApellido = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }
    
    public string? SegundoApellido 
    { 
        get => _segundoApellido;
        set => _segundoApellido = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }
    
    public EnumViewModel.TipoDocumento TipoDocumento { get; set; }

    public string NumeroDocumento 
    { 
        get => _numeroDocumento;
        set => _numeroDocumento = !string.IsNullOrEmpty(value) ? value.Trim().ToUpper() : value;
    }

    public string NombreCompleto =>
        $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}"
            .Replace("  ", " ")
            .Trim();

    // Método auxiliar para normalización de texto
    protected string NormalizarTexto(string texto)
    {
        if (string.IsNullOrEmpty(texto))
            return texto;

        // Remover espacios extras, normalizar a mayúsculas y capitalizar
        texto = texto.Trim();
        
        // Convertir a mayúsculas pero mantener acentos
        texto = texto.ToUpperInvariant();
        
        // Remover múltiples espacios
        while (texto.Contains("  "))
            texto = texto.Replace("  ", " ");
            
        return texto;
    }
}