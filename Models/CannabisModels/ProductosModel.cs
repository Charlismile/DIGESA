// using System.ComponentModel.DataAnnotations;
//
// namespace DIGESA.Models.CannabisModels;
//
// public class ProductosModel
// {
//     public string ProductoId { get; set; } = Guid.NewGuid().ToString();
//     public int Id { get; set; }
//     public int NombreProducto { get; set; }
//
//     public double NombreProductoEnum { get; set; }
//
//     [Required(ErrorMessage = "El código es requerido.")]
//     public string NombreForma { get; set; } = "";
//
//     [Required(ErrorMessage = "La unidad es requerida.")]
//     public string IsSelectedForma { get; set; } = "";
//
//     [Required(ErrorMessage = "La descripcion es requerida.")]
//     public string CantidadConcentracion { get; set; } = "";
//
//     public decimal ViaConsumoProducto { get; set; }
//     public decimal DetDosisPaciente { get; set; }
//     public decimal DuracionTratamiento { get; set; }
//     
//     public bool ShowRow { get; set; } = true;
//     public bool UpdateRow { get; set; } = false;
//     public bool InsertRow { get; set; } = false;
//     public bool DeleteRow { get; set; } = false;
// }