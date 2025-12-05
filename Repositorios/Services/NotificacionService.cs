using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DIGESA.Repositorios.Services;

public class NotificacionService : INotificacionService
{
    private readonly DbContextDigesa _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<NotificacionService> _logger;
    private readonly IConfiguration _configuration;

    public NotificacionService(
        DbContextDigesa context,
        IEmailService emailService,
        ILogger<NotificacionService> logger,
        IConfiguration configuration)
    {
        _context = context;
        _emailService = emailService;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<ResultModel<bool>> ProgramarNotificacionVencimientoAsync(int solicitudId, int diasAntelacion)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == solicitudId && 
                                         s.CarnetActivo == true && 
                                         s.FechaVencimientoCarnet.HasValue);

            if (solicitud == null)
                return ResultModel<bool>.ErrorResult("Solicitud no encontrada o carnet no activo");

            if (solicitud.Paciente == null || string.IsNullOrEmpty(solicitud.Paciente.CorreoElectronico))
                return ResultModel<bool>.ErrorResult("Paciente no tiene correo electrónico registrado");

            // Verificar si ya existe una notificación programada para estos días
            var notificacionExistente = await _context.TbNotificacionVencimiento
                .AnyAsync(n => n.SolRegCannabisId == solicitudId && 
                              n.DiasAntelacion == diasAntelacion && 
                              n.EmailEnviado == false);

            if (notificacionExistente)
                return ResultModel<bool>.ErrorResult($"Ya existe una notificación programada para {diasAntelacion} días antes");

            var notificacion = new TbNotificacionVencimiento
            {
                SolRegCannabisId = solicitudId,
                DiasAntelacion = diasAntelacion,
                FechaEnvio = null,
                EmailEnviado = false,
                UsuarioNotificado = solicitud.Paciente.CorreoElectronico,
                TipoNotificacion = "VencimientoCarnet"
            };

            _context.TbNotificacionVencimiento.Add(notificacion);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Notificación programada para solicitud {solicitudId} ({diasAntelacion} días antes)");
            return ResultModel<bool>.SuccessResult(true, "Notificación programada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error programando notificación para solicitud {solicitudId}");
            return ResultModel<bool>.ErrorResult("Error al programar la notificación", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<bool>> EnviarNotificacionesPendientesAsync()
    {
        var notificacionesEnviadas = 0;
        var errores = new List<string>();

        try
        {
            var hoy = DateTime.Now;
            
            // Obtener notificaciones pendientes con sus datos relacionados
            var notificacionesPendientes = await _context.TbNotificacionVencimiento
                .Where(n => n.EmailEnviado == false)
                .Join(_context.TbSolRegCannabis,
                    n => n.SolRegCannabisId,
                    s => s.Id,
                    (n, s) => new { Notificacion = n, Solicitud = s })
                .Join(_context.TbPaciente,
                    ns => ns.Solicitud.PacienteId,
                    p => p.Id,
                    (ns, p) => new { ns.Notificacion, ns.Solicitud, Paciente = p })
                .Where(nsp => nsp.Solicitud.CarnetActivo == true &&
                             nsp.Solicitud.FechaVencimientoCarnet.HasValue)
                .ToListAsync();

            foreach (var item in notificacionesPendientes)
            {
                try
                {
                    // Calcular días restantes
                    var diasRestantes = (item.Solicitud.FechaVencimientoCarnet.Value - hoy).Days;

                    // Verificar si es el momento de enviar la notificación
                    if (diasRestantes <= item.Notificacion.DiasAntelacion && diasRestantes > 0)
                    {
                        var notificacionModel = new NotificacionVencimientoModel
                        {
                            SolicitudId = item.Notificacion.SolRegCannabisId,
                            NumeroSolicitud = item.Solicitud.NumSolCompleta ?? "N/A",
                            NumeroCarnet = item.Solicitud.NumeroCarnet ?? "N/A",
                            PacienteNombre = $"{item.Paciente.PrimerNombre} {item.Paciente.PrimerApellido}",
                            PacienteCorreo = item.Paciente.CorreoElectronico ?? string.Empty,
                            FechaVencimiento = item.Solicitud.FechaVencimientoCarnet.Value,
                            DiasRestantes = diasRestantes,
                            DiasAntelacion = item.Notificacion.DiasAntelacion ?? 0,
                            FechaEnvio = hoy,
                            EmailEnviado = false,
                            TipoNotificacion = item.Notificacion.TipoNotificacion ?? "VencimientoCarnet"
                        };

                        // Enviar notificación
                        var enviado = await _emailService.EnviarNotificacionVencimientoAsync(notificacionModel);
                        
                        if (enviado)
                        {
                            item.Notificacion.EmailEnviado = true;
                            item.Notificacion.FechaEnvio = hoy;
                            notificacionesEnviadas++;
                            _logger.LogInformation($"Notificación enviada para solicitud {item.Notificacion.SolRegCannabisId}");
                        }
                        else
                        {
                            errores.Add($"Error enviando notificación para solicitud {item.Notificacion.SolRegCannabisId}");
                        }
                    }
                    else if (diasRestantes <= 0)
                    {
                        // Carnet ya venció, marcar como enviada para no reintentar
                        item.Notificacion.EmailEnviado = true;
                        item.Notificacion.FechaEnvio = hoy;
                        _logger.LogInformation($"Carnet vencido, notificación descartada para solicitud {item.Notificacion.SolRegCannabisId}");
                    }
                }
                catch (Exception ex)
                {
                    errores.Add($"Error procesando notificación {item.Notificacion.Id}: {ex.Message}");
                    _logger.LogError(ex, $"Error procesando notificación {item.Notificacion.Id}");
                }
            }

            // Guardar cambios en la base de datos
            if (notificacionesPendientes.Any(x => x.Notificacion.EmailEnviado == true))
            {
                await _context.SaveChangesAsync();
            }

            var mensaje = notificacionesEnviadas > 0 
                ? $"{notificacionesEnviadas} notificaciones enviadas exitosamente" 
                : "No había notificaciones pendientes para enviar";

            if (errores.Any())
            {
                return ResultModel<bool>.ErrorResult($"Proceso completado con errores. {mensaje}", errores);
            }

            return ResultModel<bool>.SuccessResult(true, mensaje);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enviando notificaciones pendientes");
            return ResultModel<bool>.ErrorResult("Error en el proceso de notificaciones", new List<string> { ex.Message });
        }
    }

    public async Task<DashboardVencimientosModel> ObtenerDashboardVencimientosAsync()
    {
        var dashboard = new DashboardVencimientosModel();
        var hoy = DateTime.Now;

        try
        {
            // Obtener todas las solicitudes con carnet activo y fecha de vencimiento
            var carnets = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Where(s => s.CarnetActivo == true && s.FechaVencimientoCarnet.HasValue)
                .ToListAsync();

            foreach (var carnet in carnets)
            {
                var diasRestantes = (carnet.FechaVencimientoCarnet.Value - hoy).Days;

                if (diasRestantes <= 0)
                {
                    dashboard.CarnetsVencidos++;
                }
                else if (diasRestantes <= 7)
                {
                    dashboard.CarnetsPorVencer7Dias++;
                }
                else if (diasRestantes <= 15)
                {
                    dashboard.CarnetsPorVencer15Dias++;
                }
                else if (diasRestantes <= 30)
                {
                    dashboard.CarnetsPorVencer30Dias++;
                }
                else
                {
                    dashboard.CarnetsVigentes++;
                }
            }

            // Obtener próximos vencimientos (próximos 30 días)
            dashboard.ProximosVencimientos = carnets
                .Where(c => c.FechaVencimientoCarnet.HasValue && 
                           c.FechaVencimientoCarnet.Value > hoy && 
                           c.FechaVencimientoCarnet.Value <= hoy.AddDays(30))
                .OrderBy(c => c.FechaVencimientoCarnet)
                .Take(10)
                .Select(c => new NotificacionVencimientoModel
                {
                    SolicitudId = c.Id,
                    NumeroSolicitud = c.NumSolCompleta ?? "N/A",
                    NumeroCarnet = c.NumeroCarnet ?? "N/A",
                    PacienteNombre = $"{c.Paciente?.PrimerNombre} {c.Paciente?.PrimerApellido}",
                    PacienteCorreo = c.Paciente?.CorreoElectronico ?? string.Empty,
                    FechaVencimiento = c.FechaVencimientoCarnet.Value,
                    DiasRestantes = (c.FechaVencimientoCarnet.Value - hoy).Days
                })
                .ToList();

            // Obtener notificaciones recientes (últimos 7 días)
            var fechaInicio = hoy.AddDays(-7);
            var notificacionesRecientes = await _context.TbNotificacionVencimiento
                .Where(n => n.FechaEnvio >= fechaInicio && n.EmailEnviado == true)
                .Join(_context.TbSolRegCannabis,
                    n => n.SolRegCannabisId,
                    s => s.Id,
                    (n, s) => new { Notificacion = n, Solicitud = s })
                .Join(_context.TbPaciente,
                    ns => ns.Solicitud.PacienteId,
                    p => p.Id,
                    (ns, p) => new NotificacionVencimientoModel
                    {
                        Id = ns.Notificacion.Id,
                        SolicitudId = ns.Notificacion.SolRegCannabisId,
                        NumeroSolicitud = ns.Solicitud.NumSolCompleta ?? "N/A",
                        NumeroCarnet = ns.Solicitud.NumeroCarnet ?? "N/A",
                        PacienteNombre = $"{p.PrimerNombre} {p.PrimerApellido}",
                        PacienteCorreo = p.CorreoElectronico ?? string.Empty,
                        FechaVencimiento = ns.Solicitud.FechaVencimientoCarnet ?? DateTime.MinValue,
                        DiasRestantes = ns.Solicitud.FechaVencimientoCarnet.HasValue ? 
                            (ns.Solicitud.FechaVencimientoCarnet.Value - hoy).Days : 0,
                        DiasAntelacion = ns.Notificacion.DiasAntelacion ?? 0,
                        FechaEnvio = ns.Notificacion.FechaEnvio ?? DateTime.MinValue,
                        EmailEnviado = ns.Notificacion.EmailEnviado ?? false,
                        TipoNotificacion = ns.Notificacion.TipoNotificacion ?? "VencimientoCarnet"
                    })
                .OrderByDescending(n => n.FechaEnvio)
                .Take(10)
                .ToListAsync();

            dashboard.NotificacionesRecientes = notificacionesRecientes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo dashboard de vencimientos");
        }

        return dashboard;
    }

    public async Task<List<NotificacionVencimientoModel>> ObtenerNotificacionesPorSolicitudAsync(int solicitudId)
    {
        try
        {
            return await _context.TbNotificacionVencimiento
                .Where(n => n.SolRegCannabisId == solicitudId)
                .Join(_context.TbSolRegCannabis,
                    n => n.SolRegCannabisId,
                    s => s.Id,
                    (n, s) => new { Notificacion = n, Solicitud = s })
                .Join(_context.TbPaciente,
                    ns => ns.Solicitud.PacienteId,
                    p => p.Id,
                    (ns, p) => new NotificacionVencimientoModel
                    {
                        Id = ns.Notificacion.Id,
                        SolicitudId = ns.Notificacion.SolRegCannabisId,
                        NumeroSolicitud = ns.Solicitud.NumSolCompleta ?? "N/A",
                        NumeroCarnet = ns.Solicitud.NumeroCarnet ?? "N/A",
                        PacienteNombre = $"{p.PrimerNombre} {p.PrimerApellido}",
                        PacienteCorreo = p.CorreoElectronico ?? string.Empty,
                        FechaVencimiento = ns.Solicitud.FechaVencimientoCarnet ?? DateTime.MinValue,
                        DiasRestantes = ns.Solicitud.FechaVencimientoCarnet.HasValue ? 
                            (ns.Solicitud.FechaVencimientoCarnet.Value - DateTime.Now).Days : 0,
                        DiasAntelacion = ns.Notificacion.DiasAntelacion ?? 0,
                        FechaEnvio = ns.Notificacion.FechaEnvio ?? DateTime.MinValue,
                        EmailEnviado = ns.Notificacion.EmailEnviado ?? false,
                        TipoNotificacion = ns.Notificacion.TipoNotificacion ?? "VencimientoCarnet"
                    })
                .OrderByDescending(n => n.FechaEnvio)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo notificaciones para solicitud {solicitudId}");
            return new List<NotificacionVencimientoModel>();
        }
    }

    public async Task<List<NotificacionVencimientoModel>> ObtenerNotificacionesPendientesAsync()
    {
        try
        {
            return await _context.TbNotificacionVencimiento
                .Where(n => n.EmailEnviado == false)
                .Join(_context.TbSolRegCannabis,
                    n => n.SolRegCannabisId,
                    s => s.Id,
                    (n, s) => new { Notificacion = n, Solicitud = s })
                .Join(_context.TbPaciente,
                    ns => ns.Solicitud.PacienteId,
                    p => p.Id,
                    (ns, p) => new NotificacionVencimientoModel
                    {
                        Id = ns.Notificacion.Id,
                        SolicitudId = ns.Notificacion.SolRegCannabisId,
                        NumeroSolicitud = ns.Solicitud.NumSolCompleta ?? "N/A",
                        NumeroCarnet = ns.Solicitud.NumeroCarnet ?? "N/A",
                        PacienteNombre = $"{p.PrimerNombre} {p.PrimerApellido}",
                        PacienteCorreo = p.CorreoElectronico ?? string.Empty,
                        FechaVencimiento = ns.Solicitud.FechaVencimientoCarnet ?? DateTime.MinValue,
                        DiasRestantes = ns.Solicitud.FechaVencimientoCarnet.HasValue ? 
                            (ns.Solicitud.FechaVencimientoCarnet.Value - DateTime.Now).Days : 0,
                        DiasAntelacion = ns.Notificacion.DiasAntelacion ?? 0,
                        FechaEnvio = ns.Notificacion.FechaEnvio,
                        EmailEnviado = ns.Notificacion.EmailEnviado ?? false,
                        TipoNotificacion = ns.Notificacion.TipoNotificacion ?? "VencimientoCarnet"
                    })
                .OrderBy(n => n.SolicitudId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo notificaciones pendientes");
            return new List<NotificacionVencimientoModel>();
        }
    }

    public async Task<ResultModel<PlantillaEmailModel>> ObtenerPlantillaAsync(string tipoNotificacion)
    {
        try
        {
            var plantilla = await _context.TbPlantillaEmail
                .FirstOrDefaultAsync(p => p.TipoNotificacion == tipoNotificacion && p.Activa == true);

            if (plantilla == null)
            {
                // Crear plantilla por defecto si no existe
                plantilla = await CrearPlantillaPorDefectoAsync(tipoNotificacion);
            }

            if (plantilla == null)
                return ResultModel<PlantillaEmailModel>.ErrorResult($"No se encontró plantilla para {tipoNotificacion}");

            var plantillaModel = new PlantillaEmailModel
            {
                Id = plantilla.Id,
                Nombre = plantilla.Nombre ?? string.Empty,
                Asunto = plantilla.Asunto ?? string.Empty,
                CuerpoHtml = plantilla.CuerpoHtml ?? string.Empty,
                TipoNotificacion = plantilla.TipoNotificacion ?? string.Empty,
                Activa = plantilla.Activa ?? true,
                VariablesDisponibles = !string.IsNullOrEmpty(plantilla.VariablesDisponibles) ? 
                    plantilla.VariablesDisponibles.Split(',').ToList() : new List<string>(),
                FechaCreacion = plantilla.FechaCreacion ?? DateTime.MinValue,
                FechaActualizacion = plantilla.FechaActualizacion
            };

            return ResultModel<PlantillaEmailModel>.SuccessResult(plantillaModel, "Plantilla obtenida exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo plantilla {tipoNotificacion}");
            return ResultModel<PlantillaEmailModel>.ErrorResult("Error al obtener la plantilla", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<PlantillaEmailModel>> ActualizarPlantillaAsync(PlantillaEmailModel plantilla)
    {
        try
        {
            var plantillaEntity = await _context.TbPlantillaEmail
                .FirstOrDefaultAsync(p => p.Id == plantilla.Id);

            if (plantillaEntity == null)
                return ResultModel<PlantillaEmailModel>.ErrorResult("Plantilla no encontrada");

            plantillaEntity.Nombre = plantilla.Nombre;
            plantillaEntity.Asunto = plantilla.Asunto;
            plantillaEntity.CuerpoHtml = plantilla.CuerpoHtml;
            plantillaEntity.TipoNotificacion = plantilla.TipoNotificacion;
            plantillaEntity.Activa = plantilla.Activa;
            plantillaEntity.VariablesDisponibles = plantilla.VariablesDisponibles != null ? 
                string.Join(",", plantilla.VariablesDisponibles) : null;
            plantillaEntity.FechaActualizacion = DateTime.Now;

            _context.TbPlantillaEmail.Update(plantillaEntity);
            await _context.SaveChangesAsync();

            plantilla.FechaActualizacion = plantillaEntity.FechaActualizacion;
            return ResultModel<PlantillaEmailModel>.SuccessResult(plantilla, "Plantilla actualizada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error actualizando plantilla {plantilla.Id}");
            return ResultModel<PlantillaEmailModel>.ErrorResult("Error al actualizar la plantilla", new List<string> { ex.Message });
        }
    }

    private async Task<TbPlantillaEmail?> CrearPlantillaPorDefectoAsync(string tipoNotificacion)
    {
        try
        {
            var plantilla = new TbPlantillaEmail();

            switch (tipoNotificacion)
            {
                case "VencimientoCarnet":
                    plantilla.Nombre = "Notificación de Vencimiento de Carnet";
                    plantilla.Asunto = "Recordatorio: Su carnet de cannabis medicinal está próximo a vencer";
                    plantilla.CuerpoHtml = await ObtenerPlantillaVencimientoPorDefectoAsync();
                    plantilla.VariablesDisponibles = "{NombrePaciente},{NumeroCarnet},{FechaVencimiento},{DiasRestantes},{NumeroSolicitud}";
                    break;

                case "Recordatorio":
                    plantilla.Nombre = "Recordatorio General";
                    plantilla.Asunto = "Recordatorio importante - DIGESA Cannabis Medicinal";
                    plantilla.CuerpoHtml = "<p>Estimado/a {NombrePaciente},<br>Este es un recordatorio importante.<br>Atentamente,<br>DIGESA</p>";
                    plantilla.VariablesDisponibles = "{NombrePaciente},{Detalle}";
                    break;

                case "CambioEstado":
                    plantilla.Nombre = "Notificación de Cambio de Estado";
                    plantilla.Asunto = "Actualización de estado de su solicitud";
                    plantilla.CuerpoHtml = "<p>Estimado/a {NombrePaciente},<br>Su solicitud {NumeroSolicitud} ha cambiado de estado a: {Estado}.<br>Comentario: {Comentario}<br>Atentamente,<br>DIGESA</p>";
                    plantilla.VariablesDisponibles = "{NombrePaciente},{NumeroSolicitud},{Estado},{Comentario}";
                    break;

                case "Transferencia":
                    plantilla.Nombre = "Notificación de Transferencia";
                    plantilla.Asunto = "Transferencia de responsabilidad";
                    plantilla.CuerpoHtml = "<p>Estimado/a {NombreUsuario},<br>Se le ha transferido la responsabilidad de la solicitud {NumeroSolicitud}.<br>Motivo: {Motivo}<br>Atentamente,<br>DIGESA</p>";
                    plantilla.VariablesDisponibles = "{NombreUsuario},{NumeroSolicitud},{Motivo},{UsuarioOrigen}";
                    break;

                default:
                    return null;
            }

            plantilla.TipoNotificacion = tipoNotificacion;
            plantilla.Activa = true;
            plantilla.FechaCreacion = DateTime.Now;
            plantilla.FechaActualizacion = DateTime.Now;

            _context.TbPlantillaEmail.Add(plantilla);
            await _context.SaveChangesAsync();

            return plantilla;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creando plantilla por defecto para {tipoNotificacion}");
            return null;
        }
    }

    private async Task<string> ObtenerPlantillaVencimientoPorDefectoAsync()
    {
        var appUrl = _configuration["AppUrl"] ?? "https://digesa.gob.pa";
        
        return $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                <h2 style='color: #2c3e50;'>Recordatorio de Vencimiento - Carnet de Cannabis Medicinal</h2>
                
                <p>Estimado/a {{NombrePaciente}},</p>
                
                <div style='background-color: #fff3cd; padding: 15px; border-radius: 5px; border-left: 4px solid #ffc107; margin: 20px 0;'>
                    <p style='margin: 0;'><strong>⚠️ IMPORTANTE:</strong> Su carnet de cannabis medicinal está próximo a vencer.</p>
                </div>
                
                <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                    <p><strong>Número de Carnet:</strong> {{NumeroCarnet}}</p>
                    <p><strong>Fecha de Vencimiento:</strong> {{FechaVencimiento}}</p>
                    <p><strong>Días restantes:</strong> {{DiasRestantes}} días</p>
                    <p><strong>Solicitud asociada:</strong> {{NumeroSolicitud}}</p>
                </div>
                
                <p>Por favor, proceda a realizar la renovación de su carnet antes de la fecha de vencimiento para evitar la inactivación del mismo.</p>
                
                <div style='margin: 25px 0; text-align: center;'>
                    <a href='{appUrl}/renovaciones' 
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
    }
}