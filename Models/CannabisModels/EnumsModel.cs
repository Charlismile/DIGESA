using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public enum TipoDocumento
{
    [Display(Name = "Cédula")]
    Cedula,
    
    [Display(Name = "Pasaporte")]
    Pasaporte
}

public enum Parentesco
{
    [Display(Name = "Padre")]
    Padre,
    
    [Display(Name = "Madre")]
    Madre,
    
    [Display(Name = "Tutor")]
    Tutor,
    
    [Display(Name = "Otro")]
    Otro
}

public enum Sexo
{
    [Display(Name = "Masculino")]
    Masculino,
    
    [Display(Name = "Femenino")]
    Femenino
}

public enum RequiereAcompanante
{
    [Display(Name = "Sí")]
    Si,
    
    [Display(Name = "No")]
    No
}

public enum MotivoRequerimientoAcompananteE
{
    [Display(Name = "Paciente menor de edad")]
    PacienteMenorEdad,
    
    [Display(Name = "Paciente con discapacidad")]
    PacienteDiscapacidad
}

public enum MedicoDisciplina
{
    [Display(Name = "Médico General")]
    General,
    
    [Display(Name = "Odontólogo")]
    Odontologo,
    
    [Display(Name = "Especialista")]
    Especialista
}

public enum TipoConcentracion
{
    [Display(Name = "CBD")]
    CBD,
    
    [Display(Name = "THC")]
    THC,
    
    [Display(Name = "Otro")]
    OTRO
}

public enum EstadoSolicitud
{
    [Display(Name = "Pendiente")]
    Pendiente,
    
    [Display(Name = "En Revisión")]
    EnRevision,
    
    [Display(Name = "Aprobada")]
    Aprobada,
    
    [Display(Name = "Rechazada")]
    Rechazada,
    
    [Display(Name = "Documentación Pendiente")]
    DocumentacionPendiente
}

public enum EstadoTransferencia
{
    [Display(Name = "Pendiente")]
    Pendiente,
    
    [Display(Name = "Aprobada")]
    Aprobada,
    
    [Display(Name = "Rechazada")]
    Rechazada,
    
    [Display(Name = "En Proceso")]
    EnProceso
}

public enum TipoNotificacion
{
    [Display(Name = "Vencimiento Carnet")]
    VencimientoCarnet,
    
    [Display(Name = "Recordatorio")]
    Recordatorio,
    
    [Display(Name = "Cambio Estado")]
    CambioEstado,
    
    [Display(Name = "Transferencia")]
    Transferencia
}

public enum TipoInscripcion
{
    [Display(Name = "Primera Inscripción")]
    Primera,
    
    [Display(Name = "Renovación")]
    Renovacion
}

public enum TipoExportacion
{
    Excel,
    PDF,
    CSV
}