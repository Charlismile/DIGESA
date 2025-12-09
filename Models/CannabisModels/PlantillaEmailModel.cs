using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PlantillaEmailModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [Required]
    [StringLength(200)]
    public string Asunto { get; set; } = string.Empty;
    
    [Required]
    public string CuerpoHtml { get; set; } = string.Empty;
    
    public string? CuerpoTexto { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TipoNotificacion { get; set; } = string.Empty;
    
    public bool Activa { get; set; } = true;
    
    public string? VariablesDisponibles { get; set; }
    
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    
    // Plantillas predefinidas
    public static List<PlantillaEmailModel> PlantillasPredefinidas()
    {
        return new List<PlantillaEmailModel>
        {
            new() {
                Id = 1,
                Nombre = "Recordatorio Vencimiento 30 días",
                Asunto = "Recordatorio: Su carnet de cannabis medicinal vence en 30 días",
                TipoNotificacion = "RecordatorioVencimiento",
                VariablesDisponibles = "{NombrePaciente}, {NumeroCarnet}, {FechaVencimiento}, {EnlaceRenovacion}",
                CuerpoHtml = @"<html><body>
                    <h2>Recordatorio de Vencimiento</h2>
                    <p>Estimado/a {NombrePaciente},</p>
                    <p>Su carnet de cannabis medicinal <strong>{NumeroCarnet}</strong> vence el {FechaVencimiento:dd/MM/yyyy}.</p>
                    <p>Para renovar su carnet, por favor acceda al sistema: {EnlaceRenovacion}</p>
                    <p>Atentamente,<br/>Ministerio de Salud</p>
                </body></html>"
            },
            new() {
                Id = 2,
                Nombre = "Carnet Inactivado",
                Asunto = "Aviso: Su carnet de cannabis medicinal ha sido inactivado",
                TipoNotificacion = "Inactivado",
                VariablesDisponibles = "{NombrePaciente}, {NumeroCarnet}, {FechaVencimiento}",
                CuerpoHtml = @"<html><body>
                    <h2>Carnet Inactivado</h2>
                    <p>Estimado/a {NombrePaciente},</p>
                    <p>Su carnet de cannabis medicinal <strong>{NumeroCarnet}</strong> ha sido inactivado por vencimiento.</p>
                    <p>Para reactivarlo, debe realizar un proceso de renovación completo.</p>
                    <p>Atentamente,<br/>Ministerio de Salud</p>
                </body></html>"
            },
            new() {
                Id = 3,
                Nombre = "Declaración Jurada",
                Asunto = "Declaración Jurada - Sistema de Cannabis Medicinal",
                TipoNotificacion = "DeclaracionJurada",
                VariablesDisponibles = "{NombrePaciente}, {TextoDeclaracion}, {Fecha}",
                CuerpoHtml = @"<html><body>
                    <h2>Declaración Jurada</h2>
                    <p>Estimado/a {NombrePaciente},</p>
                    <p>Como parte del proceso de registro, debe aceptar la siguiente declaración jurada:</p>
                    <div style='border:1px solid #ccc; padding:15px; margin:15px 0;'>
                        {TextoDeclaracion}
                    </div>
                    <p>Fecha: {Fecha:dd/MM/yyyy}</p>
                    <p>Atentamente,<br/>Ministerio de Salud</p>
                </body></html>"
            }
        };
    }
}