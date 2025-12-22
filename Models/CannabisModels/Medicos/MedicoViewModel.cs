using DIGESA.Models.CannabisModels.Catalogos;

namespace DIGESA.Models.CannabisModels.Medicos;

public class MedicoViewModel
{
    public int Id { get; set; }
    public string CodigoMedico { get; set; }

    public string PrimerNombre { get; set; }
    public string SegundoNombre { get; set; }
    public string PrimerApellido { get; set; }
    public string SegundoApellido { get; set; }

    public string TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; }

    public string Especialidad { get; set; }
    public string Subespecialidad { get; set; }
    public string NumeroColegiatura { get; set; }

    public string TelefonoConsultorio { get; set; }
    public string TelefonoMovil { get; set; }
    public string Email { get; set; }
    public string DireccionConsultorio { get; set; }

    public int? ProvinciaId { get; set; }
    public int? DistritoId { get; set; }
    public int? RegionSaludId { get; set; }
    public int? InstalacionSaludId { get; set; }
    public string InstalacionPersonalizada { get; set; }

    public DateTime FechaRegistro { get; set; }
    public DateTime? FechaActualizacion { get; set; }

    public bool Activo { get; set; }
    public bool Verificado { get; set; }
    public DateTime? FechaVerificacion { get; set; }

    public string UsuarioRegistro { get; set; }
    public string UsuarioVerificador { get; set; }
    public string Observaciones { get; set; }

    public int TotalPacientesAtendidos { get; set; }

    // Navegación
    public ProvinciaViewModel Provincia { get; set; }
    public DistritoViewModel Distrito { get; set; }
    public RegionSaludViewModel RegionSalud { get; set; }
    public InstalacionSaludViewModel InstalacionSalud { get; set; }
}