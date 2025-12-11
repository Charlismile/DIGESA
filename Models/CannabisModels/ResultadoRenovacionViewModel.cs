namespace DIGESA.Models.CannabisModels;

public class ResultadoRenovacionViewModel
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; }
    public int? NuevaSolicitudId { get; set; }
    public SolicitudCannabisViewModel Solicitud { get; set; }
    public List<string> Advertencias { get; set; } = new List<string>();
    public List<string> Errores { get; set; } = new List<string>();

    public static ResultadoRenovacionViewModel Exito(int nuevaSolicitudId, string mensaje, 
        SolicitudCannabisViewModel solicitud = null)
    {
        return new ResultadoRenovacionViewModel
        {
            Exitoso = true,
            Mensaje = mensaje,
            NuevaSolicitudId = nuevaSolicitudId,
            Solicitud = solicitud
        };
    }

    public static ResultadoRenovacionViewModel Error(string mensaje, List<string> errores = null)
    {
        return new ResultadoRenovacionViewModel
        {
            Exitoso = false,
            Mensaje = mensaje,
            Errores = errores ?? new List<string>()
        };
    }
}