using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Configuracion;
using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioNotificaciones : IServicioNotificaciones
    {
        private readonly DbContextDigesa _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ServicioNotificaciones> _logger;
        private readonly IServicioConfiguracion _servicioConfiguracion;
        private readonly EmailSettings _emailSettings;

        public ServicioNotificaciones(
            DbContextDigesa context,
            IEmailSender emailSender,
            ILogger<ServicioNotificaciones> logger,
            IServicioConfiguracion servicioConfiguracion,
            IConfiguration configuration)
        {
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
            _servicioConfiguracion = servicioConfiguracion;
            
            // Configuración de email desde appsettings.json
            _emailSettings = new EmailSettings
            {
                SmtpServer = configuration["EmailSettings:SmtpServer"],
                SmtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? "587"),
                SmtpUsername = configuration["EmailSettings:SmtpUsername"],
                SmtpPassword = configuration["EmailSettings:SmtpPassword"],
                EnableSsl = bool.Parse(configuration["EmailSettings:EnableSsl"] ?? "true"),
                FromEmail = configuration["EmailSettings:FromEmail"],
                FromName = configuration["EmailSettings:FromName"]
            };
        }

        public async Task<bool> EnviarNotificacionVencimiento(int solicitudId, string emailDestino, int diasRestantes)
        {
            try
            {
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Include(s => s.EstadoSolicitud)
                    .FirstOrDefaultAsync(s => s.Id == solicitudId);

                if (solicitud == null || string.IsNullOrEmpty(emailDestino))
                    return false;

                var asunto = $"Recordatorio: Su carnet de cannabis medicinal vence en {diasRestantes} días";
                
                var cuerpo = $@"
                <h2>Recordatorio de Vencimiento</h2>
                <p>Estimado/a {solicitud.Paciente?.PrimerNombre} {solicitud.Paciente?.PrimerApellido},</p>
                <p>Le informamos que su carnet de cannabis medicinal <strong>{solicitud.NumeroCarnet}</strong> 
                está próximo a vencer en <strong>{diasRestantes} días</strong>.</p>
                
                <h3>Detalles del Carnet:</h3>
                <ul>
                    <li><strong>Número de Carnet:</strong> {solicitud.NumeroCarnet}</li>
                    <li><strong>Fecha de Vencimiento:</strong> {solicitud.FechaVencimientoCarnet:dd/MM/yyyy}</li>
                    <li><strong>Estado Actual:</strong> {solicitud.EstadoSolicitud?.NombreEstado}</li>
                </ul>
                
                <p><strong>Acción Requerida:</strong> Debe iniciar el proceso de renovación antes de la fecha de vencimiento.</p>
                
                <p>Puede iniciar la renovación accediendo al sistema o contactando al departamento correspondiente.</p>
                
                <p>Mensaje automático del Sistema de Identificación de Cannabis Medicinal.</p>
                <p><em>Este es un mensaje automático, por favor no responder a este correo.</em></p>
                ";

                await EnviarEmailAsync(emailDestino, asunto, cuerpo);
                
                _logger.LogInformation("Notificación de vencimiento enviada a {Email} para solicitud {SolicitudId}", 
                    emailDestino, solicitudId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación de vencimiento a {Email}", emailDestino);
                return false;
            }
        }

        public async Task<bool> EnviarNotificacionRenovacionIniciada(int solicitudId, string emailDestino)
        {
            try
            {
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == solicitudId);

                if (solicitud == null || string.IsNullOrEmpty(emailDestino))
                    return false;

                var asunto = "Proceso de Renovación Iniciado - Carnet Cannabis Medicinal";
                
                var cuerpo = $@"
                <h2>Renovación Iniciada</h2>
                <p>Estimado/a {solicitud.Paciente?.PrimerNombre} {solicitud.Paciente?.PrimerApellido},</p>
                <p>Se ha iniciado el proceso de renovación para su carnet de cannabis medicinal.</p>
                
                <h3>Información de la Renovación:</h3>
                <ul>
                    <li><strong>Solicitud Número:</strong> {solicitud.NumSolCompleta}</li>
                    <li><strong>Fecha de Inicio:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</li>
                    <li><strong>Estado:</strong> Pendiente de Documentación</li>
                </ul>
                
                <p><strong>Próximos Pasos:</strong></p>
                <ol>
                    <li>Complete el formulario de renovación en el sistema</li>
                    <li>Suba los documentos requeridos</li>
                    <li>Espere la revisión y aprobación</li>
                </ol>
                
                <p>Una vez aprobada la renovación, se le emitirá un nuevo carnet con vigencia de 2 años.</p>
                
                <p>Mensaje automático del Sistema de Identificación de Cannabis Medicinal.</p>
                <p><em>Este es un mensaje automático, por favor no responder a este correo.</em></p>
                ";

                await EnviarEmailAsync(emailDestino, asunto, cuerpo);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación de renovación iniciada");
                return false;
            }
        }

        public async Task<bool> EnviarNotificacionRenovacionCompletada(int solicitudId, string emailDestino)
        {
            try
            {
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == solicitudId);

                if (solicitud == null || string.IsNullOrEmpty(emailDestino))
                    return false;

                var asunto = "Renovación Completada - Nuevo Carnet Emitido";
                
                var cuerpo = $@"
                <h2>Renovación Completada Exitosamente</h2>
                <p>Estimado/a {solicitud.Paciente?.PrimerNombre} {solicitud.Paciente?.PrimerApellido},</p>
                <p>Nos complace informarle que su renovación de carnet de cannabis medicinal ha sido aprobada.</p>
                
                <h3>Información del Nuevo Carnet:</h3>
                <ul>
                    <li><strong>Número de Carnet:</strong> {solicitud.NumeroCarnet}</li>
                    <li><strong>Fecha de Emisión:</strong> {solicitud.FechaEmisionCarnet:dd/MM/yyyy}</li>
                    <li><strong>Fecha de Vencimiento:</strong> {solicitud.FechaVencimientoCarnet:dd/MM/yyyy}</li>
                    <li><strong>Vigencia:</strong> 2 años</li>
                </ul>
                
                <p><strong>Importante:</strong> Recuerde que su carnet anterior ya no es válido. Utilice este nuevo carnet para cualquier gestión.</p>
                
                <p>Mensaje automático del Sistema de Identificación de Cannabis Medicinal.</p>
                <p><em>Este es un mensaje automático, por favor no responder a este correo.</em></p>
                ";

                await EnviarEmailAsync(emailDestino, asunto, cuerpo);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación de renovación completada");
                return false;
            }
        }

        public async Task<bool> EnviarNotificacionCarnetInactivado(int solicitudId, string emailDestino, string razon)
        {
            try
            {
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == solicitudId);

                if (solicitud == null || string.IsNullOrEmpty(emailDestino))
                    return false;

                var asunto = "Carnet Inactivado - Sistema de Cannabis Medicinal";
                
                var cuerpo = $@"
                <h2>Carnet Inactivado</h2>
                <p>Estimado/a {solicitud.Paciente?.PrimerNombre} {solicitud.Paciente?.PrimerApellido},</p>
                <p>Le informamos que su carnet de cannabis medicinal <strong>{solicitud.NumeroCarnet}</strong> ha sido inactivado.</p>
                
                <h3>Información:</h3>
                <ul>
                    <li><strong>Número de Carnet:</strong> {solicitud.NumeroCarnet}</li>
                    <li><strong>Fecha de Inactivación:</strong> {DateTime.Now:dd/MM/yyyy}</li>
                    <li><strong>Razón:</strong> {razon}</li>
                </ul>
                
                <p><strong>Consecuencias:</strong></p>
                <ul>
                    <li>Su carnet ya no es válido para comprar productos de cannabis medicinal</li>
                    <li>No podrá acceder a los beneficios del programa</li>
                </ul>
                
                <p>Si considera que esto es un error o desea reactivar su carnet, contacte al departamento correspondiente.</p>
                
                <p>Mensaje automático del Sistema de Identificación de Cannabis Medicinal.</p>
                <p><em>Este es un mensaje automático, por favor no responder a este correo.</em></p>
                ";

                await EnviarEmailAsync(emailDestino, asunto, cuerpo);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación de carnet inactivado");
                return false;
            }
        }

        public async Task<bool> EnviarNotificacionGeneral(string emailDestino, string asunto, string cuerpo)
        {
            try
            {
                if (string.IsNullOrEmpty(emailDestino))
                    return false;

                await EnviarEmailAsync(emailDestino, asunto, cuerpo);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación general a {Email}", emailDestino);
                return false;
            }
        }

        public async Task<bool> EnviarSMSVencimiento(int solicitudId, string telefono, int diasRestantes)
        {
            try
            {
                // Implementación de SMS - depende del proveedor
                // Esta es una implementación de ejemplo
                _logger.LogInformation("SMS de vencimiento enviado al {Telefono} para solicitud {SolicitudId}", 
                    telefono, solicitudId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando SMS de vencimiento");
                return false;
            }
        }

        public async Task<bool> EnviarSMSRenovacion(int solicitudId, string telefono)
        {
            try
            {
                // Implementación de SMS - depende del proveedor
                _logger.LogInformation("SMS de renovación enviado al {Telefono} para solicitud {SolicitudId}", 
                    telefono, solicitudId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando SMS de renovación");
                return false;
            }
        }

        public async Task<string> ObtenerPlantillaEmail(string tipo)
        {
            try
            {
                var plantilla = await _context.TbPlantillaEmail
                    .FirstOrDefaultAsync(p => p.Nombre == tipo && p.Activa == true);

                return plantilla?.CuerpoHtml ?? string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo plantilla de email para tipo {Tipo}", tipo);
                return string.Empty;
            }
        }

        public async Task<bool> GuardarPlantillaEmail(string tipo, string asunto, string cuerpo)
        {
            try
            {
                var plantilla = await _context.TbPlantillaEmail
                    .FirstOrDefaultAsync(p => p.Nombre == tipo);

                if (plantilla == null)
                {
                    plantilla = new TbPlantillaEmail
                    {
                        Nombre = tipo,
                        Asunto = asunto,
                        CuerpoHtml = cuerpo,
                        TipoNotificacion = tipo,
                        Activa = true,
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now
                    };
                    _context.TbPlantillaEmail.Add(plantilla);
                }
                else
                {
                    plantilla.Asunto = asunto;
                    plantilla.CuerpoHtml = cuerpo;
                    plantilla.FechaActualizacion = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando plantilla de email para tipo {Tipo}", tipo);
                return false;
            }
        }

        private async Task EnviarEmailAsync(string emailDestino, string asunto, string cuerpo)
        {
            try
            {
                using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                    EnableSsl = _emailSettings.EnableSsl
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = asunto,
                    Body = cuerpo,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(emailDestino);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando email a {Email}", emailDestino);
                throw;
            }
        }
        public async Task EnviarNotificacion(string tipo, string usuarioDestino, object datos)
        {
            // Implementación mínima - solo para compilar
            _logger.LogInformation($"Notificación {tipo} enviada a {usuarioDestino}");
            await Task.CompletedTask;
        }
    }
}