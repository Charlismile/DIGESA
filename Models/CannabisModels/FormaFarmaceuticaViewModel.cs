using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class FormaFarmaceuticaViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; }
    
    public bool IsActivo { get; set; } = true;
    
    // Validación personalizada para formas farmacéuticas de cannabis
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var formasPermitidas = new[] 
        { 
            "Aceite", "Cápsulas", "Crema", "Ungüento", 
            "Spray", "Vaporizador", "Comestibles", "Extracto" 
        };
        
        if (!formasPermitidas.Contains(Nombre, StringComparer.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                $"La forma farmacéutica '{Nombre}' no está registrada en el sistema",
                new[] { nameof(Nombre) });
        }
    }
}