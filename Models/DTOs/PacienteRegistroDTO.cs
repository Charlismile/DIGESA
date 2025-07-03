using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class PacienteRegistroDTO
{
    // Datos personales del paciente
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    public string NombreCompleto { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un tipo de documento.")]
    public string TipoDocumento { get; set; } // Ej: Cédula, Pasaporte

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z0-9\-]{5,30}$", ErrorMessage = "Número de documento no válido.")]
    public string NumeroDocumento { get; set; }

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    public string Nacionalidad { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el sexo del paciente.")]
    public string Sexo { get; set; } // Femenino, Masculino

    [Required(ErrorMessage = "La dirección de residencia es obligatoria.")]
    public string DireccionResidencia { get; set; }

    // Contacto
    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Teléfono residencial no válido.")]
    public string TelefonoResidencial { get; set; }

    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Teléfono personal no válido.")]
    public string TelefonoPersonal { get; set; }

    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Teléfono laboral no válido.")]
    public string TelefonoLaboral { get; set; }

    [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
    public string CorreoElectronico { get; set; }

    // Instalación de salud
    [Required(ErrorMessage = "La instalación de salud es obligatoria.")]
    public string InstalacionSalud { get; set; }

    [Required(ErrorMessage = "La región de salud es obligatoria.")]
    public string RegionSalud { get; set; }

    // Discapacidad
    public bool RequiereAcompanante { get; set; }

    public string MotivoRequerimientoAcompanante { get; set; }

    public string TipoDiscapacidad { get; set; }

    // Acompañante
    public AcompananteDTO Acompanante { get; set; } = new();

    // Médico tratante
    [Required(ErrorMessage = "Los datos del médico son obligatorios.")]
    public MedicoDTO Medico { get; set; } = new();

    // Diagnósticos
    [Required(ErrorMessage = "Debe seleccionar al menos un diagnóstico.")]
    public List<DiagnosticoSeleccionadoDTO> Diagnosticos { get; set; } = new();

    // Tratamiento
    [Required(ErrorMessage = "Debe proporcionar los datos del tratamiento.")]
    public TratamientoDTO Tratamiento { get; set; } = new();

    // Otras enfermedades
    public OtrasEnfermedadesDTO OtrasEnfermedades { get; set; } = new();
}

// Clases auxiliares

public class AcompananteDTO
{
    [Required(ErrorMessage = "Nombre completo del acompañante es obligatorio.")]
    public string NombreCompleto { get; set; }

    [Required(ErrorMessage = "Tipo de documento es obligatorio.")]
    public string TipoDocumento { get; set; }

    [Required(ErrorMessage = "Número de documento es obligatorio.")]
    [RegularExpression(@"^[a-zA-Z0-9\-]{5,30}$", ErrorMessage = "Número de documento no válido.")]
    public string NumeroDocumento { get; set; }

    [Required(ErrorMessage = "Nacionalidad del acompañante es obligatoria.")]
    public string Nacionalidad { get; set; }

    [Required(ErrorMessage = "Parentesco es obligatorio.")]
    public string Parentesco { get; set; }

    public bool EsPacienteMenorEdad { get; set; }
    public bool EsPacienteMayorDiscapacidad { get; set; }
}

public class MedicoDTO
{
    [Required(ErrorMessage = "Nombre completo del médico es obligatorio.")]
    public string NombreCompleto { get; set; }

    [Required(ErrorMessage = "Especialidad del médico es obligatoria.")]
    public string Especialidad { get; set; }

    [Required(ErrorMessage = "Número de registro de idoneidad es obligatorio.")]
    public string RegistroIdoneidad { get; set; }

    [Required(ErrorMessage = "Instalación de salud del médico es obligatoria.")]
    public string InstalacionSalud { get; set; }

    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Número de teléfono no válido.")]
    public string NumeroTelefono { get; set; }
}

public class DiagnosticoSeleccionadoDTO
{
    public string Nombre { get; set; }
    public string CodigoCIE10 { get; set; }
    public string Observaciones { get; set; }
}

public class TratamientoDTO
{
    [Range(0, 100, ErrorMessage = "La concentración de CBD debe estar entre 0 y 100.")]
    public decimal ConcentracionCBD { get; set; }

    [Range(0, 100, ErrorMessage = "La concentración de THC debe estar entre 0 y 100.")]
    public decimal ConcentracionTHC { get; set; }

    public string OtrosCannabinoides { get; set; }

    [Required(ErrorMessage = "La dosis es obligatoria.")]
    public string Dosis { get; set; }

    [Required(ErrorMessage = "La frecuencia de administración es obligatoria.")]
    public string FrecuenciaAdministracion { get; set; }

    [Range(1, 365, ErrorMessage = "La duración del tratamiento debe estar entre 1 y 365 días.")]
    public int DuracionTratamientoDias { get; set; }

    public string CantidadPrescrita { get; set; }
    public string InstruccionesAdicionales { get; set; }

    public int FormaFarmaceuticaId { get; set; }
    public int FrecuenciaAdministracionId { get; set; }
    public int ViaAdministracionId { get; set; }
    public int UnidadCBDId { get; set; }
    public int UnidadTHCId { get; set; }
    public int? UnidadOtroCannabinoide1Id { get; set; }
    public int? UnidadOtroCannabinoide2Id { get; set; }
    public int TipoProductoId { get; set; }
}

public class OtrasEnfermedadesDTO
{
    [Required(ErrorMessage = "El diagnóstico adicional es obligatorio.")]
    public string Diagnostico { get; set; }

    public string Tratamiento { get; set; }
}