using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ProductoPacienteViewModel
{
    public int Id { get; set; }
    
    [StringLength(200)]
    public string NombreProducto { get; set; }
    
    public int? PacienteId { get; set; }
    
    [StringLength(150)]
    public string FormaFarmaceutica { get; set; }
    
    public decimal? CantidadConcentracion { get; set; }
    
    [StringLength(100)]
    public string NombreConcentracion { get; set; }
    
    [StringLength(200)]
    public string ViaConsumoProducto { get; set; }
    
    [StringLength(300)]
    public string DetDosisPaciente { get; set; }
    
    public bool UsaDosisRescate { get; set; }
    
    [StringLength(50)]
    public string ProductoUnidad { get; set; }
    
    public int? DosisDuracion { get; set; }
    public int? DosisFrecuencia { get; set; }
    
    [StringLength(200)]
    public string NombreComercialProd { get; set; }
    
    [StringLength(300)]
    public string DetDosisRescate { get; set; }
    
    public int? ProductoUnidadId { get; set; }
    public int? FormaFarmaceuticaId { get; set; }
    public int? ViaAdministracionId { get; set; }
    
    // Propiedades de navegación
    public UnidadViewModel Unidad { get; set; }
    public FormaFarmaceuticaViewModel FormaFarmaceuticaNav { get; set; }
    public ViaAdministracionViewModel ViaAdministracion { get; set; }
}