using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DatosProductoVM
{
    [Required]
    public int ProductoId { get; set; }

    public int? FormaFarmaceuticaId { get; set; }

    public decimal? Concentracion { get; set; }

    public string ViaAdministracion { get; set; }

    [StringLength(300)]
    public string Dosis { get; set; }

    public bool UsaDosisRescate { get; set; }
}
