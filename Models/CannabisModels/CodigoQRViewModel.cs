using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class CodigoQrViewModel
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
    // En CodigoQRViewModel.cs
    public string NumeroCarnet { get; set; } // Cambiar de get-only a con setter
    public bool EstaVencido => FechaVencimiento.HasValue && FechaVencimiento.Value < DateTime.Now;
    public bool PuedeEscanear => Activo && !EstaVencido;
    
    // Para generación de QR
    public string UrlValidacion => $"https://digesa.gob.pa/validar-carnet/{CodigoQR}";
    public string DatosCodificados => $"CANNABIS-MED|{SolicitudId}|{NumeroCarnet}|{FechaGeneracion:yyyyMMdd}";
    
    // NUEVAS PROPIEDADES PARA EL SERVICIO
    public bool EsValido { get; set; }
    public string Estado { get; set; }
    public string Mensaje { get; set; }
    public string UrlValidacionPublica { get; set; }
    public int DiasRestantes { get; set; }
    public bool EnPeriodoGracia { get; set; }
    public int DiasVencido { get; set; }
    public string PacienteNombre { get; set; }
    public string PacienteCedula { get; set; }
    public string EscaneadoPor { get; set; } // Propiedad temporal para escaneo
    
    // Método para registrar escaneo
    public void RegistrarEscaneo(string escaneadoPor)
    {
        VecesEscaneado++;
        UltimoEscaneo = DateTime.Now;
        UltimoEscaneadoPor = escaneadoPor;
    }
}