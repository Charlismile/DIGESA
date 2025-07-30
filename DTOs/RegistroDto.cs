using DIGESA.Dtos;

namespace DIGESA.DTOs;

public class RegistroDto
{
    // === DATOS DEL PACIENTE ===
    public string? NombreCompleto { get; set; }
    public string? TipoDocumento { get; set; }
    public string? NumeroDocumento { get; set; }
    public string? Nacionalidad { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? Sexo { get; set; }

    // === DATOS DE CONTACTO ===
    public string? TelefonoResidencial { get; set; }
    public string? TelefonoPersonal { get; set; }
    public string? TelefonoLaboral { get; set; }
    public string? CorreoElectronico { get; set; }

    // === UBICACIÓN Y RESIDENCIA ===
    public int? RId { get; set; }
    public int? PId { get; set; }
    public int? DId { get; set; }
    public int? CId { get; set; }
    public string? DireccionExacta { get; set; }
    public string? NombreRegion { get; set; }

    // === DATOS DEL MÉDICO ===
    public string? MedicoNombreCompleto { get; set; }
    public bool EsMedicoEspecialista { get; set; }
    public string? MedicoEspecialidad { get; set; }
    public string? MedicoNumeroRegistroIdoneidad { get; set; }
    public string? MedicoInstalacionSalud { get; set; }
    public string? MedicoNumeroTelefono { get; set; }

    // === INSTALACIÓN DE SALUD (PACIENTE) ===
    public int? IId { get; set; }
    public string? NombreInstalacion { get; set; }

    // === ACOMPAÑANTE (SI APLICA) ===
    public bool RequiereAcompanante { get; set; }
    public string? AcompananteNombreCompleto { get; set; }
    public string? AcompananteTipoDocumento { get; set; }
    public string? AcompananteNumeroDocumento { get; set; }
    public string? AcompananteNacionalidad { get; set; }
    public string? AcompananteParentesco { get; set; }
    public bool EsPacienteMenorEdad { get; set; }
    public bool EsPacienteMayorDiscapacidad { get; set; }
    public string? EspecificarDiscapacidad { get; set; }
    
    public string? DiscapacidadDescripcion { get; set; }
    public string? MotivoAcompanante { get; set; }

    // === SOLICITUD GENERAL ===
    public DateTime FechaSolicitud { get; set; } = DateTime.Today;
    public string? FirmaBase64 { get; set; }

    // === TRATAMIENTO PRESCRITO ===
    public int? FormaFarmaceuticaId { get; set; }
    public string? OtraFormaFarmaceuticaDescripcion { get; set; }
    public int? ViaAdministracionId { get; set; }
    public string? OtraViaAdministracionDescripcion { get; set; }
    public string? Dosis { get; set; }
    public int? FrecuenciaAdministracionId { get; set; }
    public int? DuracionMeses { get; set; }
    public int? DuracionDiasExtra { get; set; }
    
    public string? OtrosTratamientos { get; set; }

    public string? OtrosDiagnosticos { get; set; }
    public string? OtrasCondiciones { get; set; }
    public decimal? ConcentracionCBD { get; set; }
    public string? UnidadCBD { get; set; }
    public decimal? ConcentracionTHC { get; set; }
    public string? UnidadTHC { get; set; }
    public bool UsaCBD { get; set; }
    public bool UsaTHC { get; set; }
    public bool UsaOtroCannabinoide { get; set; }
    public string? OtroCannabinoideEspecifique { get; set; }
    public string? OtraUnidadCBDDescripcion { get; set; }
    public string? OtraUnidadTHCDescripcion { get; set; }

    // === DIAGNÓSTICOS PRINCIPALES ===
    public List<int> DiagnosticoIds { get; set; } = new();

    // === COMORBILIDADES ===
    public List<ComorbilidadDto> Comorbilidades { get; set; } = new();

    // === OTROS ===
    public List<string> CondicionesMedicas { get; set; } = new();
    public List<string> FormasFarmaceuticas { get; set; } = new();
    public List<string> ViasAdministracion { get; set; } = new();
    public int VecesAlDia { get; set; } = 1;
    
    public string? DeRescate { get; set; }
    public string? Observaciones { get; set; }
    public string? Mes { get; set; }
}