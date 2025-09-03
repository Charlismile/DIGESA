using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum ConcentracionE {CBD,THC, OTRO}
public enum NombreProductoE {CBD,THC, OTRO}
public enum UsaDosisRescate {Si, No}

public class ProductoPacienteModel
{

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public NombreProductoE? NombreProductoEnum { get; set; }
        
        [Required(ErrorMessage = "La concentracion es obligatoria.")]
        public ConcentracionE ConcentracionEnum { get; set; }
        public UsaDosisRescate? UsaDosisRescateEnum{ get; set; }
        
        
        public bool IsOtraFormaSelected { get; set; } = false;
        public List<int> SelectedFormaIds { get; set; } = new();
        public string? NombreOtraForma { get; set; }
        
        public bool IsOtraViaAdmSelected { get; set; } = false;
        public List<int> SelectedViaAdmIds { get; set; } = new();
        public string? NombreOtraViaAdm { get; set; }
        
        
        public int Id { get; set; }
        public string? NombreProducto { get; set; }
        
        [Required(ErrorMessage = "El nombre comercial del producto es obligatorio.")]
        public string? NombreComercialProd { get; set; }
        public string? NombreConcentracion { get; set; }

        [Required(ErrorMessage = "La cantidad de concentracion es obligatoria.")]
        public decimal? CantidadConcentracion { get; set; }
        public bool IsSelectedUnidad { get; set; } = false;
        
        [Required(ErrorMessage = "La unidad de concentracion es obligatoria.")]
        public string? ProductoUnidad { get; set; }

        [Required(ErrorMessage = "El detalle de la dosis del tratamiento es obligatoria.")]
        public string? DetDosisPaciente { get; set; }
        
        [Required(ErrorMessage = "La duracion de tratamiento es obligatoria.")]
        [Range(1, 6, ErrorMessage = "Indique un valor entre 1 y 6.")]
        public int DosisDuracion { get; set; }
        
        [Required(ErrorMessage = "La frecuencia de tratamiento es obligatoria.")]
        [Range(1, 31, ErrorMessage = "Indique un valor entre 1 y 31.")]
        public int DosisFrecuencia { get; set; }
}

