namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosMedicosVM : PersonaBaseViewModel
{
    public string Nacionalidad { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public EnumViewModel.Sexo Sexo { get; set; }

    public string? TelefonoPersonal { get; set; }
    public string? TelefonoLaboral { get; set; }
    public string? CorreoElectronico { get; set; }
    public string? DireccionExacta { get; set; }

    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    public int? CorregimientoId { get; set; }
    public int? RegionSaludId { get; set; }

    public int? InstalacionSaludId { get; set; }
    public string? InstalacionSaludPersonalizada { get; set; }

    public EnumViewModel.RequiereAcompanante RequiereAcompanante { get; set; }
    public EnumViewModel.MotivoRequerimientoAcompanante? MotivoRequerimientoAcompanante { get; set; }
    public string? TipoDiscapacidad { get; set; }
}