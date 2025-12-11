using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class CodigoQRViewModel
{
    public int Id { get; set; }
    
    [Required]
    public int SolicitudId { get; set; }
    
    [Required]
    [StringLength(500)]
    public string CodigoQR { get; set; } // URL o datos codificados
    
    [Required]
    public DateTime FechaGeneracion { get; set; }
    
    public DateTime? FechaVencimiento { get; set; }
    
    public bool Activo { get; set; } = true;
    
    public int VecesEscaneado { get; set; } = 0;
    
    public DateTime? UltimoEscaneo { get; set; }
    
    [StringLength(200)]
    public string UltimoEscaneadoPor { get; set; }
    
    [StringLength(500)]
    public string Comentarios { get; set; }
    
    // Propiedades de navegación
    public SolicitudCannabisViewModel Solicitud { get; set; }
    
    // Propiedades calculadas
    public string NumeroCarnet => Solicitud?.NumeroCarnet;
    public bool EstaVencido => FechaVencimiento.HasValue && FechaVencimiento.Value < DateTime.Now;
    public bool PuedeEscanear => Activo && !EstaVencido;
    
    // Para generación de QR
    public string UrlValidacion => $"https://digesa.gob.pa/validar-carnet/{CodigoQR}";
    public string DatosCodificados => $"CANNABIS-MED|{SolicitudId}|{NumeroCarnet}|{FechaGeneracion:yyyyMMdd}";
    
    // Método para registrar escaneo
    public void RegistrarEscaneo(string escaneadoPor)
    {
        VecesEscaneado++;
        UltimoEscaneo = DateTime.Now;
        UltimoEscaneadoPor = escaneadoPor;
    }

}