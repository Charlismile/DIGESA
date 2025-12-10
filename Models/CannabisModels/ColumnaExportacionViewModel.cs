namespace DIGESA.Models.CannabisModels;

public class ColumnaExportacionViewModel
{
    public string Nombre { get; set; }
    public string Titulo { get; set; }
    public bool Visible { get; set; } = true;
    public int Ancho { get; set; }
    public string Formato { get; set; } // "Texto", "Numero", "Fecha", "Moneda"
    public string Alineacion { get; set; } = "Izquierda";
}