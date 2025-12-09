using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class TipoDocumentoAdjuntoModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(300)]
    public string? Descripcion { get; set; }
    
    // Requisitos por tipo de usuario
    public bool EsRequisitoParaPaciente { get; set; } = false;
    public bool EsRequisitoParaAcompanante { get; set; } = false;
    public bool EsParaMenorEdad { get; set; } = false;
    public bool EsParaMayorEdad { get; set; } = true;
    public bool EsParaRenovacion { get; set; } = false;
    public bool EsObligatorio { get; set; } = true;
    public bool EsSoloMedico { get; set; } = false;
    
    public bool IsActivo { get; set; } = true;
    public int OrdenRequisito { get; set; } = 0;
    public string? ExtensionesPermitidas { get; set; } // ".pdf,.jpg,.jpeg,.png"
    public long? TamanoMaximoBytes { get; set; } = 5 * 1024 * 1024; // 5MB por defecto
    
    // Tipos predefinidos según la legislación
    public static List<TipoDocumentoAdjuntoModel> TiposPredefinidos()
    {
        return new List<TipoDocumentoAdjuntoModel>
        {
            // Documentos del paciente
            new() { 
                Id = 1, 
                Nombre = "Copia de Cédula o Pasaporte", 
                Descripcion = "Documento de identificación del paciente",
                EsRequisitoParaPaciente = true,
                EsObligatorio = true,
                OrdenRequisito = 1
            },
            new() { 
                Id = 2, 
                Nombre = "Receta Médica", 
                Descripcion = "Receta emitida por médico tratante",
                EsRequisitoParaPaciente = true,
                EsObligatorio = true,
                EsSoloMedico = true,
                OrdenRequisito = 2
            },
            new() { 
                Id = 3, 
                Nombre = "Historia Clínica", 
                Descripcion = "Resumen de historia clínica actualizada",
                EsRequisitoParaPaciente = true,
                EsObligatorio = true,
                EsSoloMedico = true,
                OrdenRequisito = 3
            },
            new() { 
                Id = 4, 
                Nombre = "Evaluación Médica", 
                Descripcion = "Evaluación completa del médico tratante",
                EsRequisitoParaPaciente = true,
                EsObligatorio = true,
                EsSoloMedico = true,
                OrdenRequisito = 4
            },
            new() { 
                Id = 5, 
                Nombre = "Comprobante de Domicilio", 
                Descripcion = "Recibo de servicios públicos o similar",
                EsRequisitoParaPaciente = true,
                EsObligatorio = true,
                OrdenRequisito = 5
            },
            
            // Documentos para menores de edad
            new() { 
                Id = 6, 
                Nombre = "Partida de Nacimiento", 
                Descripcion = "Para pacientes menores de edad",
                EsRequisitoParaPaciente = true,
                EsParaMenorEdad = true,
                EsObligatorio = true,
                OrdenRequisito = 6
            },
            new() { 
                Id = 7, 
                Nombre = "Autorización de Padre/Madre/Tutor", 
                Descripcion = "Autorización firmada por el tutor legal",
                EsRequisitoParaPaciente = true,
                EsParaMenorEdad = true,
                EsObligatorio = true,
                OrdenRequisito = 7
            },
            
            // Documentos para acompañantes
            new() { 
                Id = 8, 
                Nombre = "Cédula del Acompañante", 
                Descripcion = "Documento de identificación del acompañante",
                EsRequisitoParaAcompanante = true,
                EsObligatorio = true,
                OrdenRequisito = 8
            },
            new() { 
                Id = 9, 
                Nombre = "Documento que acredite parentesco", 
                Descripcion = "Para acompañantes familiares",
                EsRequisitoParaAcompanante = true,
                EsObligatorio = false,
                OrdenRequisito = 9
            },
            
            // Documentos para renovación
            new() { 
                Id = 10, 
                Nombre = "Carnet anterior (si aplica)", 
                Descripcion = "Copia del carnet anterior para renovación",
                EsRequisitoParaPaciente = true,
                EsParaRenovacion = true,
                EsObligatorio = false,
                OrdenRequisito = 10
            },
            new() { 
                Id = 11, 
                Nombre = "Evaluación de seguimiento médico", 
                Descripcion = "Evaluación médica de seguimiento para renovación",
                EsRequisitoParaPaciente = true,
                EsParaRenovacion = true,
                EsObligatorio = true,
                EsSoloMedico = true,
                OrdenRequisito = 11
            }
        };
    }
}