using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ProductoPacienteModel
{
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        public string? NombreProducto { get; set; }
        
        [Required(ErrorMessage = "Seleccione la forma farmaceutica.")]
        public string? FormaFarmaceutica { get; set; }

        [Required(ErrorMessage = "La Cantidad de Concentracion es obligatoria.")]
        public decimal? CantidadConcentracion { get; set; }

        [Required(ErrorMessage = "La Concentracion es obligatoria.")]
        public string? Concentracion { get; set; }

        [Required(ErrorMessage = "El Parentesco es obligatoria.")]
        public string? Parentesco { get; set; }
}

