using DIGESA.Models.DTOs;

public class PacienteRegistroDTO
{
    public string NombreCompleto { get; set; } = "";
    public string TipoDocumento { get; set; } = "";
    public string NumeroDocumento { get; set; } = "";
    public string Nacionalidad { get; set; } = "";
    public DateTime FechaNacimiento { get; set; }
    public string Sexo { get; set; } = "";
    public string DireccionResidencia { get; set; } = "";
    public string? TelefonoResidencial { get; set; }
    public string? TelefonoPersonal { get; set; }
    public string? TelefonoLaboral { get; set; }
    public string? CorreoElectronico { get; set; }
    public string InstalacionSalud { get; set; } = "";
    public string RegionSalud { get; set; } = "";

    public bool RequiereAcompanante { get; set; }
    public string? MotivoRequerimientoAcompanante { get; set; }
    public string? TipoDiscapacidad { get; set; }

    // Datos del acompañante (opcional)
    public AcompananteRegistroDTO Acompanante { get; set; } = new();
}