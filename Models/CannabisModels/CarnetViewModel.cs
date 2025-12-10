namespace DIGESA.Models.CannabisModels;

public class CarnetViewModel
{
    public int SolicitudId { get; set; }
    public string NumeroCarnet { get; set; }
    
    // Datos del paciente para el carnet
    public string NombreCompletoPaciente { get; set; }
    public string DocumentoIdentidad { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string FotoUrl { get; set; }
    public string FirmaDigitalUrl { get; set; }
    
    // Fechas importantes
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    
    // Información médica resumida
    public string DiagnosticoPrincipal { get; set; }
    public string ProductoPrincipal { get; set; }
    public string DosisRecomendada { get; set; }
    
    // Información del médico
    public string NombreMedico { get; set; }
    public string CodigoMedico { get; set; }
    
    // Información del acompañante (si aplica)
    public string NombreAcompanante { get; set; }
    public string ParentescoAcompanante { get; set; }
    
    // Códigos QR/Barras
    public string CodigoQRBase64 { get; set; }
    public string CodigoBarrasBase64 { get; set; }
    
    // Validación de vigencia
    public bool EstaVigente => DateTime.Now <= FechaVencimiento;
    public bool EstaPorVencer => DateTime.Now.AddDays(30) >= FechaVencimiento;
    public int DiasParaVencimiento => (FechaVencimiento - DateTime.Now).Days;
    
    // Textos legales
    public string TextoLegal => "Este carnet autoriza el uso medicinal de cannabis según la ley vigente. " +
                                "El Ministerio de Salud no se hace responsable del uso que se le dé al producto.";
    public string TextoVencimiento => $"VÁLIDO HASTA: {FechaVencimiento:dd/MM/yyyy}";
}