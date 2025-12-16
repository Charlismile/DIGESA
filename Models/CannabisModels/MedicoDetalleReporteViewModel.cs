namespace DIGESA.Models.CannabisModels;

public class MedicoDetalleReporteViewModel
{
    public int Id { get; set; }
    public string CodigoMedico { get; set; }
    public string NombreCompleto { get; set; }
    public string Especialidad { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
    public int? ProvinciaId { get; set; }
    public bool Activo { get; set; }
    public bool Verificado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaVerificacion { get; set; }
}