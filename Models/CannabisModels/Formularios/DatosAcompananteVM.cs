using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosAcompananteVM : PersonaBaseViewModel
{
    private string _parentesco = string.Empty;
    private string _telefonoMovil = string.Empty;
    private string? _nacionalidad;

    [Required(ErrorMessage = "Seleccione el parentesco")]
    public string Parentesco 
    { 
        get => _parentesco;
        set => _parentesco = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }

    [Required(ErrorMessage = "El teléfono móvil es requerido")]
    [Phone(ErrorMessage = "Formato de teléfono inválido")]
    [MaxLength(20, ErrorMessage = "Máximo 20 caracteres")]
    public string TelefonoMovil 
    { 
        get => _telefonoMovil;
        set => _telefonoMovil = !string.IsNullOrEmpty(value) ? value.Trim() : value;
    }
    
    public string? Nacionalidad 
    { 
        get => _nacionalidad;
        set => _nacionalidad = !string.IsNullOrEmpty(value) ? NormalizarTexto(value) : value;
    }
}