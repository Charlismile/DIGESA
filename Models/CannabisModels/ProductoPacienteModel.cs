using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum DuracionTratamientoE {VECES, DIAS}
public enum ConcentracionE {CBD,THC, OTRO}
public enum NombreProductoE {CBD,THC, OTRO}

public enum UsaDosisRescate {Si, No}

public class ProductoPacienteModel
{
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string? NombreProducto { get; set; }
        public NombreProductoE NombreProductoEnum { get; set; }
        
        [Required(ErrorMessage = "El nombre comercial del producto es obligatorio.")]
        public string? NombreComercialProd { get; set; }
    
        [Required(ErrorMessage = "La forma farmaceutica es obligatoria.")]
        public string? NombreForma { get; set; }
        public bool IsSelectedForma { get; set; } = false;
        public string? NombreConcentracion { get; set; }

        [Required(ErrorMessage = "La Cantidad de Concentracion es obligatoria.")]
        public decimal? CantidadConcentracion { get; set; }

        [Required(ErrorMessage = "La Concentracion es obligatoria.")]
        public ConcentracionE ConcentracionEnum { get; set; }
        public bool IsSelectedUnidad { get; set; } = false;
        public bool IsSelectedViaConsumo { get; set; } = false;
        
        [Required(ErrorMessage = "La Unidad de Concentracion es obligatoria.")]
        public string? ProductoUnidad { get; set; }

        [Required(ErrorMessage = "Seleccione una via de consumo.")]
        public string? ViaConsumoProducto { get; set; }

        [Required(ErrorMessage = "El detalle de la dosis del tratamiento es obligatoria.")]
        public string? DetDosisPaciente { get; set; }
        
        [Required(ErrorMessage = "La Duracion de Tratamiento es obligatoria.")]
        [Range(1, 6, ErrorMessage = "Indique un valor entre 1 y 6.")]
        public int DosisDuracion { get; set; }
        
        [Required(ErrorMessage = "La Frecuencia de Tratamiento es obligatoria.")]
        [Range(1, 31, ErrorMessage = "Indique un valor entre 1 y 31.")]
        public int DosisFrecuencia { get; set; }
        public DuracionTratamientoE DuracionTratamientoEnum { get; set; }
        public UsaDosisRescate UsaDosisRescateEnum{ get; set; }
}

