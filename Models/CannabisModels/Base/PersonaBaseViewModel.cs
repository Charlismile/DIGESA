namespace DIGESA.Models.CannabisModels;

public abstract class PersonaBaseViewModel
{
    public string PrimerNombre { get; set; }
    public string SegundoNombre { get; set; }
    public string PrimerApellido { get; set; }
    public string SegundoApellido { get; set; }

    public string TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; }

    public string NombreCompleto =>
        $"{PrimerNombre} {SegundoNombre} {PrimerApellido} {SegundoApellido}".Replace("  ", " ").Trim();
}
