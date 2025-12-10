using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    [StringLength(100)]
    public string PrimerNombre { get; set; }
    
    [StringLength(100)]
    public string SegundoNombre { get; set; }
    
    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    [StringLength(100)]
    public string PrimerApellido { get; set; }
    
    [StringLength(100)]
    public string SegundoApellido { get; set; }
    
    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    [StringLength(50)]
    public string TipoDocumento { get; set; }
    
    [StringLength(100)]
    public string DocumentoCedula { get; set; }
    
    [StringLength(100)]
    public string DocumentoPasaporte { get; set; }
    
    [Required(ErrorMessage = "La nacionalidad es obligatoria")]
    [StringLength(100)]
    public string Nacionalidad { get; set; }
    
    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime FechaNacimiento { get; set; }
    
    [StringLength(10)]
    public string Sexo { get; set; }
    
    public bool RequiereAcompanante { get; set; }
    
    [StringLength(300)]
    public string MotivoRequerimientoAcompanante { get; set; }
    
    [StringLength(150)]
    public string TipoDiscapacidad { get; set; }
    
    [StringLength(15)]
    public string TelefonoPersonal { get; set; }
    
    [StringLength(15)]
    public string TelefonoLaboral { get; set; }
    
    [EmailAddress]
    [StringLength(200)]
    public string CorreoElectronico { get; set; }
    
    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    public int? CorregimientoId { get; set; }
    
    [StringLength(300)]
    public string DireccionExacta { get; set; }
    
    public int? RegionId { get; set; }
    public int? InstalacionId { get; set; }
    
    [StringLength(200)]
    public string InstalacionPersonalizada { get; set; }
    
    // Propiedades de navegación para UI
    public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}";
    public string DocumentoIdentidad => TipoDocumento.ToUpper() switch
    {
        "CEDULA" => DocumentoCedula,
        "PASAPORTE" => DocumentoPasaporte,
        _ => DocumentoCedula ?? DocumentoPasaporte
    };
    
    // Para mostrar en listas
    public int Edad => CalcularEdad();
    
    private int CalcularEdad()
    {
        var hoy = DateTime.Today;
        var edad = hoy.Year - FechaNacimiento.Year;
        if (FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;
        return edad;
    }
}