using DIGESA.Models.CannabisModels.Formularios;

namespace DIGESA.Models.CannabisModels.Renovaciones;

public class PacienteViewModel
{
    public int Id { get; set; }
    public string? PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public string? PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }

    public EnumViewModel.TipoDocumento TipoDocumento { get; set; }
    public string Documento { get; set; } = string.Empty;
    public string? Nacionalidad { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public EnumViewModel.Sexo Sexo { get; set; }

    public string? TelefonoPersonal { get; set; }
    public string? TelefonoLaboral { get; set; }
    public string? CorreoElectronico { get; set; }
    public string? DireccionExacta { get; set; }

    public EnumViewModel.RequiereAcompanante RequiereAcompanante { get; set; }
    public EnumViewModel.MotivoRequerimientoAcompanante? MotivoRequerimientoAcompanante { get; set; }
    public string? TipoDiscapacidad { get; set; }
    
    // acompanante
    public DatosAcompananteVM? Acompanante { get; set; }

}