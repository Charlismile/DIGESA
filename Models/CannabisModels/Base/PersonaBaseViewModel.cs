namespace DIGESA.Models.CannabisModels;

public abstract class PersonaBaseViewModel
{
    public string PrimerNombre { get; set; } = string.Empty;
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = string.Empty;
    public string? SegundoApellido { get; set; }
    
    public EnumViewModel.TipoDocumento TipoDocumento { get; set; }

    public string NumeroDocumento { get; set; } = string.Empty;

    public string NombreCompleto =>
        $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}"
            .Replace("  ", " ")
            .Trim();
}