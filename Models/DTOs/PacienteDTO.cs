namespace DIGESA.Models.DTOs;

public class PacienteDTO
{
    public int Id { get; set; }
    public required string NombreCompleto { get; set; }
    public required string TipoDocumento { get; set; }
    public required string NumeroDocumento { get; set; }
    public required string Nacionalidad { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public required string Sexo { get; set; }
    public required string DireccionResidencia { get; set; }
    public string? TelefonoResidencial { get; set; }
    public string? TelefonoPersonal { get; set; }
    public string? TelefonoLaboral { get; set; }
    public string? CorreoElectronico { get; set; }
    public required string InstalacionSalud { get; set; }
    public required string RegionSalud { get; set; }
    public bool RequiereAcompanante { get; set; }
    public string? MotivoRequerimientoAcompanante { get; set; }
    public string? TipoDiscapacidad { get; set; }
}