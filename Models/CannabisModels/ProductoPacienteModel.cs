using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum ConcentracionE { CBD, THC, OTRO }
public enum NombreProductoE { CBD, THC, OTRO }
public enum UsaDosisRescate { Si, No }

public class ProductoPacienteModel : IValidatableObject // AGREGADO: Implementar IValidatableObject
{
    [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
    public NombreProductoE? NombreProductoEnum { get; set; }
    
    [Required(ErrorMessage = "La concentracion es obligatoria.")]
    public ConcentracionE? ConcentracionEnum { get; set; } // CORREGIDO: Hacer nullable
    
    public UsaDosisRescate? UsaDosisRescateEnum { get; set; }
    
    public string? DetDosisRescate { get; set; }
    public bool IsOtraFormaSelected { get; set; } = false;
    
    [Required(ErrorMessage = "Seleccione al menos una forma farmacéutica.")]
    public List<int> SelectedFormaIds { get; set; } = new();
    
    public string? NombreOtraForma { get; set; }
    
    public bool IsOtraViaAdmSelected { get; set; } = false;
    
    [Required(ErrorMessage = "Seleccione al menos una vía de administración.")]
    public List<int> SelectedViaAdmIds { get; set; } = new();
    
    public string? NombreOtraViaAdm { get; set; }
    
    public int Id { get; set; }
    public string? NombreProducto { get; set; }
    
    [Required(ErrorMessage = "El nombre comercial del producto es obligatorio.")]
    public string? NombreComercialProd { get; set; }
    public string? NombreConcentracion { get; set; }

    [Required(ErrorMessage = "La cantidad de concentracion es obligatoria.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public decimal? CantidadConcentracion { get; set; }
    
    public bool IsSelectedUnidad { get; set; } = false;
    
    [Required(ErrorMessage = "El detalle de la dosis del tratamiento es obligatoria.")]
    public string? DetDosisPaciente { get; set; }
    
    [Required(ErrorMessage = "La duracion de tratamiento es obligatoria.")]
    [Range(1, 6, ErrorMessage = "Indique un valor entre 1 y 6.")]
    public int DosisDuracion { get; set; }
    
    [Required(ErrorMessage = "La frecuencia de tratamiento es obligatoria.")]
    [Range(1, 31, ErrorMessage = "Indique un valor entre 1 y 31.")]
    public int DosisFrecuencia { get; set; }
    
    [Required(ErrorMessage = "La unidad del producto es obligatoria.")]
    public int? ProductoUnidadId { get; set; }
    
    public string? ProductoUnidad { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Validación para nombre de producto "OTRO"
        if (NombreProductoEnum == NombreProductoE.OTRO && string.IsNullOrWhiteSpace(NombreProducto))
        {
            yield return new ValidationResult("Especifique el nombre del producto.",
                new[] { nameof(NombreProducto) });
        }

        // Validación para concentración "OTRO"
        if (ConcentracionEnum == ConcentracionE.OTRO && string.IsNullOrWhiteSpace(NombreConcentracion))
        {
            yield return new ValidationResult("Especifique la concentración.",
                new[] { nameof(NombreConcentracion) });
        }

        // Validación para forma farmacéutica "OTRO"
        if (IsOtraFormaSelected && string.IsNullOrWhiteSpace(NombreOtraForma))
        {
            yield return new ValidationResult("Especifique la forma farmacéutica.",
                new[] { nameof(NombreOtraForma) });
        }

        // Validación para vía de administración "OTRO"
        if (IsOtraViaAdmSelected && string.IsNullOrWhiteSpace(NombreOtraViaAdm))
        {
            yield return new ValidationResult("Especifique la vía de administración.",
                new[] { nameof(NombreOtraViaAdm) });
        }

        // Validación para dosis de rescate
        if (UsaDosisRescateEnum == UsaDosisRescate.Si && string.IsNullOrWhiteSpace(DetDosisRescate))
        {
            yield return new ValidationResult("Especifique los detalles de la dosis de rescate.",
                new[] { nameof(DetDosisRescate) });
        }
    }
}