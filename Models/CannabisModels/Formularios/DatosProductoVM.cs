namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosProductoVM
{
    public string NombreProducto { get; set; } = string.Empty;
    public EnumViewModel.NombreProductoE NombreProductoEnum { get; set; }

    public string? NombreComercialProd { get; set; }

    public List<int> SelectedFormaIds { get; set; } = new();
    public string? NombreOtraForma { get; set; }
    public bool IsOtraFormaSelected { get; set; }

    public string? ProductoUnidad { get; set; }


    public List<int> SelectedViaAdmIds { get; set; } = new();
    public string? NombreOtraViaAdm { get; set; }
    public bool IsOtraViaAdmSelected { get; set; }


    public int? ProductoUnidadId { get; set; }

    public decimal? CantidadConcentracion { get; set; }
    public EnumViewModel.ConcentracionE ConcentracionEnum { get; set; }
    public string? NombreConcentracion { get; set; }

    public string? DetDosisPaciente { get; set; }
    public string? DosisFrecuencia { get; set; }
    public string? DosisDuracion { get; set; }

    public EnumViewModel.UsaDosisRescate UsaDosisRescateEnum { get; set; }
    public string? DetDosisRescate { get; set; }
}