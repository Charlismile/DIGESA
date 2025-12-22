namespace DIGESA.Models.CannabisModels.CodigoQr;

public class CodigoQrViewModel
{
    // =============================
    // Identificadores
    // =============================
    public int Id { get; set; }
    public int SolicitudId { get; set; }

    // =============================
    // Datos del QR
    // =============================
    public string CodigoQR { get; set; } = string.Empty;

    public DateTime FechaGeneracion { get; set; }
    public DateTime? FechaVencimiento { get; set; }

    public bool Activo { get; set; }

    // =============================
    // Control de escaneos
    // =============================
    public int VecesEscaneado { get; set; }
    public DateTime? UltimoEscaneo { get; set; }
    public string? UltimoEscaneadoPor { get; set; }

    // =============================
    // Información adicional
    // =============================
    public string? Comentarios { get; set; }
    public string? NumeroCarnet { get; set; }

    // URL pública para validación
    public string UrlValidacionPublica { get; set; } = string.Empty;

    // =============================
    // Resultado de validación
    // =============================
    public bool EsValido { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
}