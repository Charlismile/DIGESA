namespace DIGESA.Models.CannabisModels;

public class SolicitudDetalleModel : SolicitudModel
{
    public string? ComentarioRevision { get; set; }
    public List<string>? DocumentosAdjuntos { get; set; }
    public List<string>? Declaraciones { get; set; }
}