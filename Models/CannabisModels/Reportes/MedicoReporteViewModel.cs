namespace DIGESA.Models.CannabisModels.Reportes;

public class MedicoReporteViewModel
{
    public string CodigoMedico { get; set; }
    public string NombreCompleto { get; set; }
    public string Especialidad { get; set; }

    public string NumeroColegiatura { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }

    public string Provincia { get; set; }
    public string InstalacionSalud { get; set; }

    public bool Verificado { get; set; }
    public bool Activo { get; set; }

    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaVerificacion { get; set; }

    public int PacientesAtendidos { get; set; }
}
