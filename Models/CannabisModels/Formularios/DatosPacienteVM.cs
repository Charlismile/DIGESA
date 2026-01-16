using System.ComponentModel.DataAnnotations;
using DIGESA.Models.CannabisModels.Common;

namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosPacienteVM : PersonaBaseViewModel
{
    private string _nacionalidad = string.Empty;
    private string? _telefonoPersonal;
    private string? _telefonoLaboral;
    private string? _correoElectronico;
    private string? _direccionExacta;
    private string? _tipoDiscapacidad;
    private string? _instalacionSaludPersonalizada;

    public string Nacionalidad 
    { 
        get => _nacionalidad;
        set => _nacionalidad = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }

    public DateTime? FechaNacimiento { get; set; }
    public EnumViewModel.Sexo Sexo { get; set; }

    [Phone(ErrorMessage = "Formato de teléfono inválido")]
    [MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
    public string? TelefonoPersonal 
    { 
        get => _telefonoPersonal;
        set => _telefonoPersonal = !string.IsNullOrEmpty(value) ? value.Trim() : value;
    }

    [Phone(ErrorMessage = "Formato de teléfono inválido")]
    [MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
    public string? TelefonoLaboral 
    { 
        get => _telefonoLaboral;
        set => _telefonoLaboral = !string.IsNullOrEmpty(value) ? value.Trim() : value;
    }

    [EmailAddress(ErrorMessage = "Formato de correo electrónico inválido")]
    [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? CorreoElectronico 
    { 
        get => _correoElectronico;
        set => _correoElectronico = !string.IsNullOrEmpty(value) ? value.Trim().ToLower() : value;
    }

    [MaxLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string? DireccionExacta 
    { 
        get => _direccionExacta;
        set => _direccionExacta = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }

    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    public int? CorregimientoId { get; set; }
    public int? RegionSaludId { get; set; }
    
    public int? InstalacionSaludId { get; set; }
    
    [MaxLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string? InstalacionSaludPersonalizada 
    { 
        get => _instalacionSaludPersonalizada;
        set => _instalacionSaludPersonalizada = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }

    [Required(ErrorMessage = "Seleccione si requiere acompañante")]
    public EnumViewModel.RequiereAcompanante RequiereAcompanante { get; set; }
    
    public EnumViewModel.MotivoRequerimientoAcompanante? MotivoRequerimientoAcompanante { get; set; }
    
    [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string? TipoDiscapacidad 
    { 
        get => _tipoDiscapacidad;
        set => _tipoDiscapacidad = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }
}