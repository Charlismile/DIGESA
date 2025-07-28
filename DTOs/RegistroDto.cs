using System.ComponentModel.DataAnnotations;

namespace DIGESA.DTOs;

public class RegistroDto
{
    // 1. DATOS PERSONALES DEL PACIENTE
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar el tipo de documento.")]
    public string TipoDocumento { get; set; } = "Cédula"; // Cédula, Pasaporte

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [StringLength(30, ErrorMessage = "Máximo 30 caracteres.")]
    public string NumeroDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    public string Nacionalidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es obligatorio.")]
    public string Sexo { get; set; } = string.Empty; // Femenino, Masculino

    [Required(ErrorMessage = "La provincia es obligatoria.")]
    public int? PId { get; set; }
    
    public string NombreProvincia { get; set; } = " ";

    [Required(ErrorMessage = "El distrito es obligatorio.")]
    public int DId { get; set; }
    public string NombreDistrito { get; set; } = "";
    public int ProvinciaId { get; set; }

    [Required(ErrorMessage = "El corregimiento es obligatorio.")]
    public int CId { get; set; }
    public string NombreCorregimiento { get; set; } = "";
    public int DistritoId { get; set; }

    [Required(ErrorMessage = "La dirección exacta es obligatoria.")]
    [StringLength(300, ErrorMessage = "La dirección no puede exceder 300 caracteres.")]
    public string DireccionExacta { get; set; } = string.Empty;

    public string? TelefonoResidencial { get; set; }
    public string? TelefonoPersonal { get; set; }
    public string? TelefonoLaboral { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Required(ErrorMessage = "La instalación de salud es obligatoria.")]
    public string InstalacionSalud { get; set; } = string.Empty;

    [Required(ErrorMessage = "La región de salud es obligatoria.")]
    public string RegionSalud { get; set; } = string.Empty;

    // 1.10 ¿Requiere acompañante?
    public bool RequiereAcompanante { get; set; }

    // 1.10.1 Motivo
    public bool EsPacienteMenorEdad { get; set; }
    public bool EsPacienteMayorDiscapacidad { get; set; }
    public string? DiscapacidadDescripcion { get; set; }

    // 2. DATOS DEL ACOMPAÑANTE
    public string? AcompananteNombreCompleto { get; set; }
    public string? AcompananteTipoDocumento { get; set; } = "Cédula";
    public string? AcompananteNumeroDocumento { get; set; }
    public string? AcompananteNacionalidad { get; set; }
    public string? AcompananteParentesco { get; set; } // Madre, Padre, Tutor

    // 3. DATOS DEL MÉDICO
    [Required(ErrorMessage = "El nombre del médico es obligatorio.")]
    public string MedicoNombreCompleto { get; set; } = string.Empty;

    public bool EsMedicoEspecialista { get; set; } = false;
    public string? MedicoEspecialidad { get; set; }

    [Required(ErrorMessage = "El número de registro de idoneidad es obligatorio.")]
    public string MedicoNumeroRegistroIdoneidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono del médico es obligatorio.")]
    public string MedicoNumeroTelefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La instalación de salud del médico es obligatoria.")]
    public string MedicoInstalacionSalud { get; set; } = string.Empty;

    // 4.1 Diagnósticos principales
    [MinLength(1, ErrorMessage = "Debe seleccionar al menos un diagnóstico principal.")]
    public List<int> DiagnosticoIds { get; set; } = new();

    public string? OtroDiagnosticoEspecifique { get; set; }

    // 4.2 Tipo de producto
    public bool UsaCBD { get; set; }
    public bool UsaTHC { get; set; }
    public bool UsaOtroCannabinoide { get; set; }
    public string? OtroCannabinoideEspecifique { get; set; }

    // 4.3 Forma farmacéutica
    [Required(ErrorMessage = "Debe seleccionar una forma farmacéutica.")]
    public int? FormaFarmaceuticaId { get; set; }
    public string? OtraFormaFarmaceuticaDescripcion { get; set; }

    // 4.4 Concentración
    public decimal? ConcentracionCBD { get; set; }
    public int? UnidadCBDId { get; set; }
    public string? OtraUnidadCBDDescripcion { get; set; }

    public decimal? ConcentracionTHC { get; set; }
    public int? UnidadTHCId { get; set; }
    public string? OtraUnidadTHCDescripcion { get; set; }

    public string? OtroCannabinode1 { get; set; }
    public decimal? ConcentracionOtroCannabinoide1 { get; set; }
    public int? UnidadOtroCannabinoide1Id { get; set; }
    public string? OtraUnidadOtroCannabinoide1Descripcion { get; set; }

    public string? OtroCannabinode2 { get; set; }
    public decimal? ConcentracionOtroCannabinoide2 { get; set; }
    public int? UnidadOtroCannabinoide2Id { get; set; }
    public string? OtraUnidadOtroCannabinoide2Descripcion { get; set; }

    // 4.5 Vía de administración
    [Required(ErrorMessage = "Debe seleccionar una vía de administración.")]
    public int? ViaAdministracionId { get; set; }
    public string? OtraViaAdministracionDescripcion { get; set; }

    // 4.6 Dosis
    [Required(ErrorMessage = "La dosis es obligatoria.")]
    public string Dosis { get; set; } = string.Empty;

    // 4.7 Frecuencia
    [Required(ErrorMessage = "Debe seleccionar una frecuencia de administración.")]
    public int? FrecuenciaAdministracionId { get; set; }

    // 4.8 Duración del tratamiento
    [Required(ErrorMessage = "La duración en meses es obligatoria.")]
    public int? DuracionMeses { get; set; }

    [Required(ErrorMessage = "La duración en días es obligatoria.")]
    public int? DuracionDiasExtra { get; set; }

    // 5. COMORBILIDADES (OTRAS ENFERMEDADES)
    // ✅ Corregido: ahora es una lista, no un solo campo
    public List<ComorbilidadItem> Comorbilidades { get; set; } = new();

    // Firma del paciente
    public string? FirmaBase64 { get; set; }

    // Clase anidada para comorbilidades
    public class ComorbilidadItem
    {
        [Required(ErrorMessage = "Debe seleccionar un diagnóstico.")]
        public int? DiagnosticoId { get; set; }

        [Required(ErrorMessage = "El tratamiento recibido es obligatorio.")]
        [StringLength(300, ErrorMessage = "El tratamiento no puede exceder 300 caracteres.")]
        public string TratamientoRecibido { get; set; } = string.Empty;
    }
}