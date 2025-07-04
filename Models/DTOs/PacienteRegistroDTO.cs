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
    // public OtrasEnfermedadesDTO OtrasEnfermedades { get; set; } = new();
}

// Clases auxiliares
public class PacienteDTO
{
    public int Id { get; set; }

    // Opcional: si usas autenticación y se registra quién hizo la acción
    public int? UsuarioId { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un tipo de documento.")]
    [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
    public string TipoDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    [StringLength(30, ErrorMessage = "Máximo 30 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9\-]{5,30}$", ErrorMessage = "Número de documento no válido.")]
    public string NumeroDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nacionalidad es obligatoria.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string Nacionalidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? FechaNacimiento { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el sexo del paciente.")]
    [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
    public string Sexo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La instalación de salud es obligatoria.")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
    public string InstalacionSalud { get; set; } = string.Empty;

    [Required(ErrorMessage = "La región de salud es obligatoria.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string RegionSalud { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección de residencia es obligatoria.")]
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres.")]
    public string DireccionResidencia { get; set; } = string.Empty;

    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Teléfono residencial no válido.")]
    public string? TelefonoResidencial { get; set; }

    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Teléfono personal no válido.")]
    public string? TelefonoPersonal { get; set; }

    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Teléfono laboral no válido.")]
    public string? TelefonoLaboral { get; set; }

    [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
    public string? CorreoElectronico { get; set; }

    public bool RequiereAcompanante { get; set; }

    public string? MotivoRequerimientoAcompanante { get; set; }

    public string? TipoDiscapacidad { get; set; }

    public string? FirmaBase64 { get; set; }
}
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
// Medico
public class MedicoDTO
{
    public int Id { get; set; }

    // Opcional: si usas autenticación y se registra quién hizo la acción
    public int? UsuarioId { get; set; }

    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad del médico es obligatoria.")]
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres.")]
    public string Especialidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El número de registro de idoneidad es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string RegistroIdoneidad { get; set; } = string.Empty;

    [Required(ErrorMessage = "La instalación de salud es obligatoria.")]
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres.")]
    public string InstalacionSalud { get; set; } = string.Empty;

    [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Número de teléfono no válido.")]
    public string NumeroTelefono { get; set; } = string.Empty;

    public bool EsMedicoEspecialista { get; set; }

    // Firma digital (opcional)
    public string? FirmaBase64 { get; set; }
}

public class AuditoriaAccionDTO
{
    public int Id { get; set; }

    // Opcional: si usas autenticación y se registra quién hizo la acción
    public int? UsuarioId { get; set; }

    [Required(ErrorMessage = "La acción realizada es obligatoria.")]
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres permitidos.")]
    public string AccionRealizada { get; set; } = string.Empty;

    // Nombre de la tabla afectada (opcional pero útil para auditoría)
    [StringLength(128, ErrorMessage = "El nombre de la tabla no puede exceder los 128 caracteres.")]
    public string? NombreTablaAfectada { get; set; }

    // ID del registro afectado (por ejemplo, Id de Paciente, Médico, etc.)
    public int? RegistroAfectadoId { get; set; }

    // Detalles adicionales sobre la acción
    public string? DetallesAdicionales { get; set; }

    // Fecha y hora de la acción
    [DataType(DataType.DateTime)]
    public DateTime? FechaAccion { get; set; }

    // Dirección IP del usuario que realizó la acción
    [StringLength(50, ErrorMessage = "La dirección IP no puede exceder los 50 caracteres.")]
    public string? Ipaddress { get; set; }

    // Información del navegador/dispositivo del usuario
    [StringLength(255, ErrorMessage = "El User-Agent no puede exceder los 255 caracteres.")]
    public string? UserAgent { get; set; }
}

public class CertificacionDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El ID de solicitud es obligatorio.")]
    public int SolicitudId { get; set; }

    [Required(ErrorMessage = "El código del certificado es obligatorio.")]
    [StringLength(100, ErrorMessage = "El código no puede exceder los 100 caracteres.")]
    public string CodigoCertificado { get; set; } = string.Empty;

    // Opcional: campos relacionados con el QR
    public string? CodigoQR { get; set; }

    public string? RutaArchivoQR { get; set; }

    public string? QRBase64 { get; set; }

    [Required(ErrorMessage = "La fecha de emisión es obligatoria.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime FechaEmision { get; set; }

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime FechaVencimiento { get; set; }

    [Required(ErrorMessage = "El estado del certificado es obligatorio.")]
    [StringLength(20, ErrorMessage = "El estado no puede exceder los 20 caracteres.")]
    public string EstadoCertificado { get; set; } = "Activa"; // Valores posibles: Activa, Vencida, Revocada
}

public class ContactoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El tipo de propietario es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string PropietarioTipo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ID del propietario es obligatorio.")]
    public int PropietarioId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un tipo de contacto.")]
    public int TipoContactoId { get; set; }

    [Required(ErrorMessage = "El valor del contacto es obligatorio.")]
    [StringLength(255, ErrorMessage = "El valor no puede exceder los 255 caracteres.")]
    public string Valor { get; set; } = string.Empty;
}


public class DecisionRevisionDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la decisión es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}

public class DiagnosticoDTO
{
    public int Id { get; set; }

    [StringLength(10, ErrorMessage = "El código CIE10 no puede exceder los 10 caracteres.")]
    public string? CodigoCie10 { get; set; }

    [Required(ErrorMessage = "El nombre del diagnóstico es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "La descripción no puede exceder los 300 caracteres.")]
    public string? Descripcion { get; set; }

    public bool EsOtro { get; set; }
}

//DocumentoAdjunto
public class DocumentoAdjuntoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El ID de la solicitud es obligatorio.")]
    public int SolicitudId { get; set; }

    [Required(ErrorMessage = "El nombre del archivo es obligatorio.")]
    [StringLength(255, ErrorMessage = "El nombre del archivo no puede exceder los 255 caracteres.")]
    public string NombreArchivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    [StringLength(100, ErrorMessage = "El tipo de documento no puede exceder los 100 caracteres.")]
    public string TipoDocumento { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "El tipo MIME no puede exceder los 100 caracteres.")]
    public string? TipoContenidoMIME { get; set; }

    [Required(ErrorMessage = "La ruta de almacenamiento es obligatoria.")]
    [StringLength(500, ErrorMessage = "La ruta no puede exceder los 500 caracteres.")]
    public string RutaAlmacenamiento { get; set; } = string.Empty;

    public DateTime? FechaSubida { get; set; }

    public int? SubidoPorUsuarioId { get; set; }

    [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
    public string? Descripcion { get; set; }
}

//Estado Solicitud

public class EstadoSolicitudDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del estado es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}

//Forma Farmaceutica

public class FormaFarmaceuticaDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la forma farmacéutica es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}

//Frecuencia Administracion
public class FrecuenciaAdministracionDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la frecuencia es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}

//Paciente Diagnostico
public class PacienteDiagnosticoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El ID del paciente es obligatorio.")]
    public int PacienteId { get; set; }

    [Required(ErrorMessage = "El ID del diagnóstico es obligatorio.")]
    public int DiagnosticoId { get; set; }

    // Opcional: para mostrar nombre/descripción del diagnóstico
    public string? NombreDiagnostico { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateOnly? FechaDiagnostico { get; set; }

    [StringLength(300, ErrorMessage = "Las observaciones no pueden exceder los 300 caracteres.")]
    public string? Observaciones { get; set; }
}

// Revision
public class RevisionDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El ID de la solicitud es obligatorio.")]
    public int SolicitudId { get; set; }

    [Required(ErrorMessage = "El ID del revisor es obligatorio.")]
    public int RevisorId { get; set; }

    [Required(ErrorMessage = "El tipo de revisión es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string TipoRevision { get; set; } = string.Empty;

    public DateTime? FechaRevision { get; set; }

    [Required(ErrorMessage = "Debe seleccionar una decisión de revisión.")]
    public int DecisionRevisionId { get; set; }

    [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres.")]
    public string? Observaciones { get; set; }

    // Campos adicionales para mostrar información en UI (opcional)
    public string? NombreRevisor { get; set; }
    public string? DecisionNombre { get; set; }
}

//rol
public class RolDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}

// Solicitud
public class SolicitudDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El ID del paciente es obligatorio.")]
    public int PacienteId { get; set; }

    [Required(ErrorMessage = "El ID del médico es obligatorio.")]
    public int MedicoId { get; set; }

    public int? AcompananteId { get; set; }

    public DateTime? FechaSolicitud { get; set; }

    [Required(ErrorMessage = "Debe seleccionar el estado de la solicitud.")]
    public int EstadoSolicitudId { get; set; }

    [StringLength(500, ErrorMessage = "El motivo no puede exceder los 500 caracteres.")]
    public string? MotivoSolicitud { get; set; }

    public int? FuncionarioRecibeId { get; set; }

    public DateTime? FechaRecepcion { get; set; }

    public DateTime? FechaAprobacionRechazo { get; set; }

    [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres.")]
    public string? ObservacionesRevision { get; set; }

    public string? FirmaBase64 { get; set; }

    // Campos adicionales para mostrar información en UI (opcional)
    public string? EstadoNombre { get; set; }
    public string? NombrePaciente { get; set; }
    public string? NombreMedico { get; set; }
    public string? NombreFuncionario { get; set; }
}

//Solicitud Diagnostico
public class SolicitudDiagnosticoDTO
{
    [Required(ErrorMessage = "El ID de la solicitud es obligatorio.")]
    public int SolicitudId { get; set; }

    [Required(ErrorMessage = "El ID del diagnóstico es obligatorio.")]
    public int DiagnosticoId { get; set; }

    [Required(ErrorMessage = "Debe indicar si el diagnóstico es primario.")]
    public bool? EsPrimario { get; set; }

    [StringLength(300, ErrorMessage = "Las observaciones no pueden exceder los 300 caracteres.")]
    public string? Observaciones { get; set; }

    // Campos adicionales para mostrar información en UI (opcional)
    public string? NombreDiagnostico { get; set; }
}

//TipoContacto
public class TipoContactoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del tipo de contacto es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}

//TipoProducto
public class TipoProductoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del tipo de producto es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}





public class DiagnosticoSeleccionadoDTO
{
    public string Nombre { get; set; }
    public string CodigoCIE10 { get; set; }
    public string Observaciones { get; set; }
}
//Tratamiento
public class TratamientoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El ID de la solicitud es obligatorio.")]
    public int SolicitudId { get; set; }

    [StringLength(150, ErrorMessage = "El nombre genérico no puede exceder los 150 caracteres.")]
    public string? NombreGenericoProducto { get; set; }

    [StringLength(150, ErrorMessage = "El nombre comercial no puede exceder los 150 caracteres.")]
    public string? NombreComercialProducto { get; set; }

    [Required(ErrorMessage = "La forma farmacéutica es obligatoria.")]
    [StringLength(100, ErrorMessage = "La forma farmacéutica no puede exceder los 100 caracteres.")]
    public string FormaFarmaceutica { get; set; } = string.Empty;

    [Range(0, 100, ErrorMessage = "La concentración de CBD debe estar entre 0 y 100.")]
    public decimal? ConcentracionCBD { get; set; }

    [StringLength(20, ErrorMessage = "La unidad de CBD no puede exceder los 20 caracteres.")]
    public string? UnidadCBD { get; set; }

    [Range(0, 100, ErrorMessage = "La concentración de THC debe estar entre 0 y 100.")]
    public decimal? ConcentracionTHC { get; set; }

    [StringLength(20, ErrorMessage = "La unidad de THC no puede exceder los 20 caracteres.")]
    public string? UnidadTHC { get; set; }

    [StringLength(200, ErrorMessage = "Otros cannabinoides no pueden exceder los 200 caracteres.")]
    public string? OtrosCannabinoides { get; set; }

    [Required(ErrorMessage = "La vía de administración es obligatoria.")]
    [StringLength(100, ErrorMessage = "La vía de administración no puede exceder los 100 caracteres.")]
    public string ViaAdministracion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dosis es obligatoria.")]
    [StringLength(100, ErrorMessage = "La dosis no puede exceder los 100 caracteres.")]
    public string Dosis { get; set; } = string.Empty;

    [Required(ErrorMessage = "La frecuencia de administración es obligatoria.")]
    [StringLength(100, ErrorMessage = "La frecuencia no puede exceder los 100 caracteres.")]
    public string FrecuenciaAdministracion { get; set; } = string.Empty;

    [Range(1, 365, ErrorMessage = "La duración del tratamiento debe estar entre 1 y 365 días.")]
    public int DuracionTratamientoDias { get; set; }

    [StringLength(100, ErrorMessage = "La cantidad prescrita no puede exceder los 100 caracteres.")]
    public string? CantidadPrescrita { get; set; }

    [StringLength(500, ErrorMessage = "Las instrucciones adicionales no pueden exceder los 500 caracteres.")]
    public string? InstruccionesAdicionales { get; set; }

    public DateOnly? FechaInicioTratamientoPrevista { get; set; }

    public int? TipoProductoId { get; set; }

    [StringLength(255, ErrorMessage = "La descripción del otro producto no puede exceder los 255 caracteres.")]
    public string? OtroProductoDescripcion { get; set; }

    public int? FormaFarmaceuticaId { get; set; }

    [StringLength(255, ErrorMessage = "La descripción de otra forma farmacéutica no puede exceder los 255 caracteres.")]
    public string? OtraFormaFarmaceuticaDescripcion { get; set; }

    public int? UnidadCBDId { get; set; }

    [StringLength(50, ErrorMessage = "La descripción de otra unidad de CBD no puede exceder los 50 caracteres.")]
    public string? OtraUnidadCBDDescripcion { get; set; }

    public string? OtraUnidadTHCDescripcion { get; set; }

    public string? OtroCannabinode1 { get; set; }

    public decimal? ConcentracionOtroCannabinoide1 { get; set; }

    public int? UnidadOtroCannabinoide1Id { get; set; }

    [StringLength(50, ErrorMessage = "La descripción de otra unidad de cannabinoide 1 no puede exceder los 50 caracteres.")]
    public string? OtraUnidadOtroCannabinoide1Descripcion { get; set; }

    public string? OtroCannabinode2 { get; set; }

    public decimal? ConcentracionOtroCannabinoide2 { get; set; }

    public int? UnidadOtroCannabinoide2Id { get; set; }

    [StringLength(50, ErrorMessage = "La descripción de otra unidad de cannabinoide 2 no puede exceder los 50 caracteres.")]
    public string? OtraUnidadOtroCannabinoide2Descripcion { get; set; }

    public int? ViaAdministracionId { get; set; }

    [StringLength(255, ErrorMessage = "La descripción de otra vía de administración no puede exceder los 255 caracteres.")]
    public string? OtraViaAdministracionDescripcion { get; set; }

    public int? FrecuenciaAdministracionId { get; set; }

    public int? DuracionMeses { get; set; }

    public int? DuracionDiasExtra { get; set; }
}

//UnidadConcentracion
public class UnidadConcentracionDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la unidad es obligatorio.")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}

//Usuario
public class UsuarioDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Correo electrónico no válido.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    [StringLength(256, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string Contraseña { get; set; } = string.Empty;

    [Compare("Contraseña", ErrorMessage = "Las contraseñas no coinciden.")]
    [DataType(DataType.Password)]
    public string ConfirmarContraseña { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar un rol.")]
    public int RolId { get; set; }

    public string? FirmaBase64 { get; set; }
}

//ViaAdministracion
public class ViaAdministracionDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la vía es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}

//Catalogos
public class CatalogoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;
}

public class CatalogosResponseDTO
{
    public List<CatalogoDTO> Diagnosticos { get; set; } = new();
    public List<CatalogoDTO> FormasFarmaceuticas { get; set; } = new();
    public List<CatalogoDTO> ViasAdministracion { get; set; } = new();
    public List<CatalogoDTO> FrecuenciasAdministracion { get; set; } = new();
    public List<CatalogoDTO> UnidadesConcentracion { get; set; } = new();
    public List<CatalogoDTO> TiposProducto { get; set; } = new();
    public List<CatalogoDTO> EstadosSolicitud { get; set; } = new();
    public List<CatalogoDTO> DecisionesRevision { get; set; } = new();
    public List<CatalogoDTO> Roles { get; set; } = new();
}