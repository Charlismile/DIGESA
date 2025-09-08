namespace DIGESA.Models.CannabisModels;

public class PacienteEstadoModel
{
    public string Documento { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public bool Activo { get; set; }
        
    // Propiedades adicionales para información completa
    public string NombreCompleto => $"{Nombre} {Apellido}".Trim();
}