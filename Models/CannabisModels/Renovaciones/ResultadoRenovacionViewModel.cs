namespace DIGESA.Models.CannabisModels.Renovaciones;

public class ResultadoRenovacionViewModel
{
    public bool Exitoso { get; set; }
    public string Mensaje { get; set; }
    public int? SolicitudId { get; set; }
    public SolicitudConHistorialViewModel? Solicitud { get; set; }

    public static ResultadoRenovacionViewModel Exito(
        int solicitudId,
        string mensaje,
        SolicitudConHistorialViewModel? solicitud = null)
    {
        return new ResultadoRenovacionViewModel
        {
            Exitoso = true,
            Mensaje = mensaje,
            SolicitudId = solicitudId,
            Solicitud = solicitud
        };
    }

    public static ResultadoRenovacionViewModel Error(string mensaje)
    {
        return new ResultadoRenovacionViewModel
        {
            Exitoso = false,
            Mensaje = mensaje
        };
    }
}
