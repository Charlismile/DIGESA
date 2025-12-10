using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ConfiguracionExportacionViewModel
{
    [Required]
    public string TipoDocumento { get; set; } // "Excel", "PDF", "CSV"
    
    // Para Excel
    public bool IncluirFormulas { get; set; }
    public bool IncluirFiltros { get; set; } = true;
    public bool IncluirGraficos { get; set; }
    public string NombreHoja { get; set; } = "Pacientes";
    
    // Para PDF
    public string Orientacion { get; set; } = "Portrait";
    public string TamanioPagina { get; set; } = "Letter";
    public bool IncluirEncabezado { get; set; } = true;
    public bool IncluirPiePagina { get; set; } = true;
    public bool IncluirNumeracionPaginas { get; set; } = true;
    
    // Columnas
    public List<ColumnaExportacionViewModel> Columnas { get; set; } = new List<ColumnaExportacionViewModel>();
    
    // Ordenamiento
    public string OrdenarPor { get; set; }
    public bool OrdenAscendente { get; set; } = true;
    
    // Segmentación (para Excel)
    public bool AgruparPorProvincia { get; set; }
    public bool AgruparPorEstado { get; set; }
    public bool AgruparPorDiagnostico { get; set; }
}