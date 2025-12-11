using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ConfiguracionSistemaViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "La clave es requerida")]
    [StringLength(100, ErrorMessage = "La clave no puede exceder 100 caracteres")]
    public string Clave { get; set; }
    
    [Required(ErrorMessage = "El valor es requerido")]
    [StringLength(500, ErrorMessage = "El valor no puede exceder 500 caracteres")]
    public string Valor { get; set; }
    
    [StringLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres")]
    public string Descripcion { get; set; }
    
    [StringLength(50)]
    public string Grupo { get; set; }
    
    public bool EsEditable { get; set; } = true;
    
    // Propiedades fuertemente tipadas para configuración específica
    public int DiasVigenciaCarnet => Clave == "DiasVigenciaCarnet" ? ValorEntero : 730;
    public int DiasAntesNotificar => Clave == "DiasAntesNotificar" ? ValorEntero : 30;
    public int DiasGraciaRenovacion => Clave == "DiasGraciaRenovacion" ? ValorEntero : 90;
    public int MaximoRenovaciones => Clave == "MaximoRenovaciones" ? ValorEntero : 10;
    public int AutoInactivarDias => Clave == "AutoInactivarDias" ? ValorEntero : 0;
    
    // Métodos de conversión
    public int ValorEntero => int.TryParse(Valor, out int resultado) ? resultado : 0;
    public bool ValorBooleano => Valor.Equals("true", StringComparison.OrdinalIgnoreCase) || 
                                 Valor == "1";
    public DateTime? ValorFecha => DateTime.TryParse(Valor, out DateTime fecha) ? fecha : (DateTime?)null;
    
    // Validación personalizada
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Grupo == "Vencimientos" && ValorEntero < 0)
        {
            yield return new ValidationResult(
                "Los días no pueden ser negativos",
                new[] { nameof(Valor) });
        }
        
        if (Clave == "MaximoRenovaciones" && ValorEntero < 1)
        {
            yield return new ValidationResult(
                "El máximo de renovaciones debe ser al menos 1",
                new[] { nameof(Valor) });
        }
    }
}