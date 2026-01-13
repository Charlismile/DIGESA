namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosAcompananteVM : PersonaBaseViewModel
{
    public string Parentesco { get; set; } = string.Empty;
    public string TelefonoMovil { get; set; } = string.Empty;
    
    public string? Nacionalidad { get; set; }

}