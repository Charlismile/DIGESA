using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum NombreProductoE {CBD,THC, Otro}

public class ProductoPacienteModel
{
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string? NombreProducto { get; set; }
        public NombreProductoE NombreProductoEnum { get; set; }
        
        public int FormaId { get; set; }
    
        [Required(ErrorMessage = "La forma farmaceutica es obligatoria.")]
        public string? NombreForma { get; set; }
    
        public bool IsSelectedForma { get; set; } = false;

        [Required(ErrorMessage = "La Cantidad de Concentracion es obligatoria.")]
        public decimal? CantidadConcentracion { get; set; }

        [Required(ErrorMessage = "La Concentracion es obligatoria.")]
        public string? Concentracion { get; set; }

        [Required(ErrorMessage = "Seleccione una via de consumo.")]
        public string? ViaConsumoProducto { get; set; }

        [Required(ErrorMessage = "El detalle de la dosis del tratamiento es obligatoria.")]
        public string? DetDosisPaciente { get; set; }

        [Required(ErrorMessage = "El tiempo de tratamiento es obligatoria.")]
        public string? DuracionTratamiento { get; set; }
}

