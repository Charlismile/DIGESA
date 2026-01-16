using DIGESA.Helpers;

namespace DIGESA.Models.CannabisModels.Listados;

public class PacienteListadoViewModel
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; }
    public string TipoDocumento { get; set; }
    public string Documento { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public DateTime? FechaSolicitud { get; set; }
    public int Edad { get; set; }
    public string Provincia { get; set; }
    public string Telefono { get; set; }
    public string Celular { get; set; }
    public string NumeroCarnet { get; set; }
    public string EstadoSolicitud { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public bool CarnetActivo { get; set; }
    
    public bool EstaPorVencer =>
        FechaVencimiento.HasValue &&
        (FechaVencimiento.Value - DateTime.Now).Days <= 30;
    
    public bool EstaVencido =>
        FechaVencimiento.HasValue &&
        FechasCarnetHelper.EstaVencido(FechaVencimiento.Value);

    public bool EstaProximoAVencer =>
        FechaVencimiento.HasValue &&
        FechasCarnetHelper.EstaProximoAVencer(FechaVencimiento.Value);
    
    // Propiedad de solo lectura para mostrar el documento formateado
    public string DocumentoFormateado 
    {
        get
        {
            if (string.IsNullOrEmpty(TipoDocumento))
                return Documento ?? "N/A";
            
            return $"{TipoDocumento}: {Documento}";
        }
    }
}