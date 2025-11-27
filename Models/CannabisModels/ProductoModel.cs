using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ProductoModel
{
    public NombreProductoE NombreProductoEnum { get; set; }
    public string? NombreProducto { get; set; }
    public string? NombreComercialProd { get; set; }
    
    public List<int> SelectedFormaIds { get; set; } = new();
    public bool IsOtraFormaSelected { get; set; }
    public string? NombreOtraForma { get; set; }
    
    public ConcentracionE ConcentracionEnum { get; set; }
    public string? NombreConcentracion { get; set; }
    public decimal? CantidadConcentracion { get; set; }
    
    public int? ProductoUnidadId { get; set; }
    public string? ProductoUnidad { get; set; }
    
    public List<int> SelectedViaAdmIds { get; set; } = new();
    public bool IsOtraViaAdmSelected { get; set; }
    public string? NombreOtraViaAdm { get; set; }
    
    public string? DetDosisPaciente { get; set; }
    public int? DosisFrecuencia { get; set; }
    public int? DosisDuracion { get; set; }
    
    public UsaDosisRescate UsaDosisRescateEnum { get; set; }
    public string? DetDosisRescate { get; set; }
}