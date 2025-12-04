using System.Net;
using System.Net.Mail;
using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Interfaces;
using Microsoft.Extensions.Options;

namespace DIGESA.Repositorios.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(destinatario))
            {
                _logger.LogWarning("No se puede enviar correo: destinatario vacío");
                return false;
            }

            var host = _config["SMTP:Host"];
            var port = int.Parse(_config["SMTP:Port"] ?? "587");
            var user = _config["SMTP:User"];
            var pass = _config["SMTP:Pass"];
            var fromName = _config["SMTP:FromName"] ?? "DIGESA - Cannabis Medicinal";

            using var smtp = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true,
                Timeout = 30000
            };

            using var mail = new MailMessage
            {
                From = new MailAddress(user ?? "noreply@digesa.gob.pa", fromName),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };

            mail.To.Add(destinatario);

            await smtp.SendMailAsync(mail);
            _logger.LogInformation($"Correo enviado exitosamente a: {destinatario}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error enviando correo a: {destinatario}");
            return false;
        }
    }

    public async Task<bool> EnviarNotificacionVencimientoAsync(NotificacionVencimientoModel notificacion)
    {
        try
        {
            var plantilla = await GenerarPlantillaVencimientoAsync(notificacion);
            if (plantilla == null)
            {
                _logger.LogWarning($"No se pudo generar plantilla para notificación de vencimiento: {notificacion.NumeroCarnet}");
                return false;
            }

            return await EnviarCorreoAsync(
                notificacion.PacienteCorreo,
                plantilla.Asunto,
                plantilla.CuerpoHtml
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error enviando notificación de vencimiento: {notificacion.NumeroCarnet}");
            return false;
        }
    }

    public async Task<bool> EnviarNotificacionEstadoAsync(string destinatario, string numeroSolicitud, string estado, string comentario)
    {
        try
        {
            var asunto = $"Actualización de estado - Solicitud {numeroSolicitud}";
            var cuerpo = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h2 style='color: #2c3e50;'>Actualización de Estado - DIGESA Cannabis Medicinal</h2>
                    <p>Estimado/a paciente,</p>
                    <p>Le informamos que su solicitud <strong>{numeroSolicitud}</strong> ha cambiado de estado.</p>
                    
                    <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                        <p><strong>Nuevo Estado:</strong> <span style='color: #3498db;'>{estado}</span></p>
                        <p><strong>Comentario:</strong> {comentario}</p>
                    </div>
                    
                    <p>Puede consultar el estado de su solicitud en cualquier momento en nuestra plataforma.</p>
                    
                    <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                    
                    <p style='color: #7f8c8d; font-size: 12px;'>
                        Este es un mensaje automático, por favor no responda a este correo.<br>
                        DIGESA - Dirección General de Salud
                    </p>
                </div>";

            return await EnviarCorreoAsync(destinatario, asunto, cuerpo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error enviando notificación de estado: {numeroSolicitud}");
            return false;
        }
    }

    private async Task<PlantillaEmailModel?> GenerarPlantillaVencimientoAsync(NotificacionVencimientoModel notificacion)
    {
        var diasAntelacion = notificacion.DiasAntelacion switch
        {
            30 => "30 días",
            15 => "15 días",
            7 => "7 días",
            _ => "próximo vencimiento"
        };

        var asunto = $"[Recordatorio] Carnet de Cannabis Medicinal vence en {diasAntelacion}";
        
        var cuerpo = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                <h2 style='color: #2c3e50;'>Recordatorio de Vencimiento - Carnet de Cannabis Medicinal</h2>
                
                <p>Estimado/a <strong>{notificacion.PacienteNombre}</strong>,</p>
                
                <div style='background-color: #fff3cd; padding: 15px; border-radius: 5px; border-left: 4px solid #ffc107; margin: 20px 0;'>
                    <p style='margin: 0;'><strong>⚠️ IMPORTANTE:</strong> Su carnet de cannabis medicinal está próximo a vencer.</p>
                </div>
                
                <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                    <p><strong>Número de Carnet:</strong> {notificacion.NumeroCarnet}</p>
                    <p><strong>Fecha de Vencimiento:</strong> {notificacion.FechaVencimiento:dd/MM/yyyy}</p>
                    <p><strong>Días restantes:</strong> {notificacion.DiasRestantes} días</p>
                    <p><strong>Solicitud asociada:</strong> {notificacion.NumeroSolicitud}</p>
                </div>
                
                <p>Por favor, proceda a realizar la renovación de su carnet antes de la fecha de vencimiento para evitar la inactivación del mismo.</p>
                
                <div style='margin: 25px 0; text-align: center;'>
                    <a href='{_config["AppUrl"]}/renovaciones' 
                       style='background-color: #3498db; color: white; padding: 12px 25px; 
                              text-decoration: none; border-radius: 5px; display: inline-block;'>
                        📋 Iniciar Renovación
                    </a>
                </div>
                
                <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                
                <p style='color: #7f8c8d; font-size: 12px;'>
                    <strong>Nota:</strong> Si su carnet vence, deberá realizar una nueva solicitud completa.<br>
                    Este es un mensaje automático, por favor no responda a este correo.<br>
                    DIGESA - Dirección General de Salud
                </p>
            </div>";

        return new PlantillaEmailModel
        {
            Asunto = asunto,
            CuerpoHtml = cuerpo,
            TipoNotificacion = "VencimientoCarnet"
        };
    }
}