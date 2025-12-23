namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosPacienteVM : PersonaBaseViewModel
{
    public string PrimerNombre { get; set; } = string.Empty;
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = string.Empty;
    public string? SegundoApellido { get; set; }

    public EnumViewModel.TipoDocumento TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;

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