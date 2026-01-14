namespace DIGESA.Models.CannabisModels.Formularios;

public class DatosProductoVM
{
    public bool UsaMultiplesMedicamentos { get; set; }
    
    public List<ProductoIndividualVM> Productos { get; set; } = new();

    // Propiedades de compatibilidad con getters y setters
    public string NombreProducto { 
        get => Productos.Count > 0 ? Productos[0].NombreProducto : string.Empty;
        set { if (Productos.Count > 0) Productos[0].NombreProducto = value; }
    }
    
    public EnumViewModel.NombreProductoE NombreProductoEnum { 
        get => Productos.Count > 0 ? Productos[0].NombreProductoEnum : EnumViewModel.NombreProductoE.CBD;
        set { if (Productos.Count > 0) Productos[0].NombreProductoEnum = value; }
    }
    
    public string? NombreComercialProd { 
        get => Productos.Count > 0 ? Productos[0].NombreComercialProd : null;
        set { if (Productos.Count > 0) Productos[0].NombreComercialProd = value; }
    }
    
    public List<int> SelectedFormaIds { 
        get => Productos.Count > 0 ? Productos[0].SelectedFormaIds : new();
        set { if (Productos.Count > 0) Productos[0].SelectedFormaIds = value; }
    }
    
    public string? NombreOtraForma { 
        get => Productos.Count > 0 ? Productos[0].NombreOtraForma : null;
        set { if (Productos.Count > 0) Productos[0].NombreOtraForma = value; }
    }
    
    public bool IsOtraFormaSelected { 
        get => Productos.Count > 0 ? Productos[0].IsOtraFormaSelected : false;
        set { if (Productos.Count > 0) Productos[0].IsOtraFormaSelected = value; }
    }
    
    public string? ProductoUnidad { 
        get => Productos.Count > 0 ? Productos[0].ProductoUnidad : null;
        set { if (Productos.Count > 0) Productos[0].ProductoUnidad = value; }
    }
    
    public List<int> SelectedViaAdmIds { 
        get => Productos.Count > 0 ? Productos[0].SelectedViaAdmIds : new();
        set { if (Productos.Count > 0) Productos[0].SelectedViaAdmIds = value; }
    }
    
    public string? NombreOtraViaAdm { 
        get => Productos.Count > 0 ? Productos[0].NombreOtraViaAdm : null;
        set { if (Productos.Count > 0) Productos[0].NombreOtraViaAdm = value; }
    }
    
    public bool IsOtraViaAdmSelected { 
        get => Productos.Count > 0 ? Productos[0].IsOtraViaAdmSelected : false;
        set { if (Productos.Count > 0) Productos[0].IsOtraViaAdmSelected = value; }
    }
    
    public int? ProductoUnidadId { 
        get => Productos.Count > 0 ? Productos[0].ProductoUnidadId : null;
        set { if (Productos.Count > 0) Productos[0].ProductoUnidadId = value; }
    }
    
    public decimal? CantidadConcentracion { 
        get => Productos.Count > 0 ? Productos[0].CantidadConcentracion : null;
        set { if (Productos.Count > 0) Productos[0].CantidadConcentracion = value; }
    }
    
    public List<EnumViewModel.ConcentracionE> ConcentracionesSeleccionadas { 
        get => Productos.Count > 0 ? Productos[0].ConcentracionesSeleccionadas : new();
        set { if (Productos.Count > 0) Productos[0].ConcentracionesSeleccionadas = value; }
    }
    
    public bool IsOtraConcentracionSelected { 
        get => Productos.Count > 0 ? Productos[0].IsOtraConcentracionSelected : false;
        set { if (Productos.Count > 0) Productos[0].IsOtraConcentracionSelected = value; }
    }
    
    public string? NombreOtraConcentracion { 
        get => Productos.Count > 0 ? Productos[0].NombreOtraConcentracion : null;
        set { if (Productos.Count > 0) Productos[0].NombreOtraConcentracion = value; }
    }
    
    public string? DetDosisPaciente { 
        get => Productos.Count > 0 ? Productos[0].DetDosisPaciente : null;
        set { if (Productos.Count > 0) Productos[0].DetDosisPaciente = value; }
    }
    
    public int? DosisFrecuencia { 
        get => Productos.Count > 0 ? Productos[0].DosisFrecuencia : null;
        set { if (Productos.Count > 0) Productos[0].DosisFrecuencia = value; }
    }
    
    public int? DosisDuracion { 
        get => Productos.Count > 0 ? Productos[0].DosisDuracion : null;
        set { if (Productos.Count > 0) Productos[0].DosisDuracion = value; }
    }
    
    public EnumViewModel.UsaDosisRescate UsaDosisRescateEnum { 
        get => Productos.Count > 0 ? Productos[0].UsaDosisRescateEnum : EnumViewModel.UsaDosisRescate.No;
        set { if (Productos.Count > 0) Productos[0].UsaDosisRescateEnum = value; }
    }
    
    public string? DetDosisRescate { 
        get => Productos.Count > 0 ? Productos[0].DetDosisRescate : null;
        set { if (Productos.Count > 0) Productos[0].DetDosisRescate = value; }
    }
}