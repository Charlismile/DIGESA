namespace DIGESA.Models.CannabisModels.Renovaciones;

public class PacienteViewModel
{
    public int Id { get; set; }

    public string? PrimerNombre { get; set; }
    public string? SegundoNombre { get; set; }
    public string? PrimerApellido { get; set; }
    public string? SegundoApellido { get; set; }

    public string? TipoDocumento { get; set; }
    public string? DocumentoCedula { get; set; }
    public string? DocumentoPasaporte { get; set; }

    public string? Nacionalidad { get; set; }
    public DateTime FechaNacimiento { get; set; }

    public string? Sexo { get; set; }
    public string? CorreoElectronico { get; set; }
}