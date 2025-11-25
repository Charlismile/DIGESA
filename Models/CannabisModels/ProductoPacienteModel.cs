using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ProductoPacienteModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre comercial del producto es obligatorio.")]
    public string? NombreComercial { get; set; } // Renombrado

    [Required(ErrorMessage = "La forma farmacéutica es obligatoria.")]
    public int? FormaFarmaceuticaId { get; set; }

    public string? FormaFarmaceuticaPersonalizada { get; set; }

    [Required(ErrorMessage = "La vía de administración es obligatoria.")]
    public int? ViaAdministracionId { get; set; }

    public string? ViaAdministracionPersonalizada { get; set; }

    [Required(ErrorMessage = "El tipo de concentración es obligatorio.")]
    public TipoConcentracion TipoConcentracion { get; set; }

    public string? ConcentracionPersonalizada { get; set; }

    [Required(ErrorMessage = "La cantidad de concentración es obligatoria.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public decimal? CantidadConcentracion { get; set; }

    [Required(ErrorMessage = "La unidad del producto es obligatoria.")]
    public int? UnidadId { get; set; }

    [Required(ErrorMessage = "El detalle de la dosis es obligatorio.")]
    public string? DetalleDosis { get; set; } // Renombrado

    [Required(ErrorMessage = "La duración del tratamiento es obligatoria.")]
    [Range(1, 6, ErrorMessage = "Indique un valor entre 1 y 6 meses.")]
    public int? DuracionTratamiento { get; set; } // Renombrado

    [Required(ErrorMessage = "La frecuencia del tratamiento es obligatoria.")]
    [Range(1, 31, ErrorMessage = "Indique un valor entre 1 y 31 días.")]
    public int? FrecuenciaTratamiento { get; set; } // Renombrado

    public UsaDosisRescate UsaDosisRescate { get; set; }
    public string? DetalleDosisRescate { get; set; } // Renombrado

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TipoConcentracion == TipoConcentracion.OTRO && string.IsNullOrWhiteSpace(ConcentracionPersonalizada))
        {
            yield return new ValidationResult("Especifique la concentración personalizada.",
                new[] { nameof(ConcentracionPersonalizada) });
        }

        if (UsaDosisRescate == UsaDosisRescate.Si && string.IsNullOrWhiteSpace(DetalleDosisRescate))
        {
            yield return new ValidationResult("Especifique los detalles de la dosis de rescate.",
                new[] { nameof(DetalleDosisRescate) });
        }
    }
}