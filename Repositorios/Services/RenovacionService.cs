using System.Net.Mail;
using System.Net;
using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class RenovacionService : IRenovacionService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<RenovacionService> _logger;
    private readonly IEmailService _emailService;
    private readonly ICarnetService _carnetService;

    public RenovacionService(
        DbContextDigesa context,
        ILogger<RenovacionService> logger,
        IEmailService emailService,
        ICarnetService carnetService)
    {
        _context = context;
        _logger = logger;
        _emailService = emailService;
        _carnetService = carnetService;
    }

    public async Task<ResultModel<bool>> VerificarDisponibilidadRenovacionAsync(int pacienteId)
    {
        try
        {
            var ultimaSolicitud = await _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .Where(s => s.PacienteId == pacienteId && 
                           s.EstadoSolicitud.NombreEstado == "Aprobada")
                .OrderByDescending(s => s.FechaAprobacion)
                .FirstOrDefaultAsync();

            if (ultimaSolicitud == null)
                return ResultModel<bool>.ErrorResult("No tiene solicitudes aprobadas");

            if (!ultimaSolicitud.FechaVencimientoCarnet.HasValue)
                return ResultModel<bool>.ErrorResult("No tiene fecha de vencimiento registrada");

            // Puede renovar 60 días antes del vencimiento
            var puedeRenovarDesde = ultimaSolicitud.FechaVencimientoCarnet.Value.AddDays(-60);
            var hoy = DateTime.Now;

            if (hoy < puedeRenovarDesde)
            {
                var diasRestantes = (puedeRenovarDesde - hoy).Days;
                return ResultModel<bool>.ErrorResult($"Podrá renovar en {diasRestantes} días");
            }

            // Verificar si ya tiene una renovación en proceso
            var renovacionPendiente = await _context.TbSolRegCannabis
                .AnyAsync(s => s.PacienteId == pacienteId && 
                              s.EsRenovacion == true && 
                              s.EstadoSolicitud.NombreEstado == "Pendiente");

            if (renovacionPendiente)
                return ResultModel<bool>.ErrorResult("Ya tiene una renovación en proceso");

            return ResultModel<bool>.SuccessResult(true, "Puede renovar su carnet");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error verificando renovación para paciente {pacienteId}");
            return ResultModel<bool>.ErrorResult("Error verificando renovación");
        }
    }

    public async Task<ResultModel<int>> IniciarRenovacionAsync(int solicitudOriginalId, List<int> documentosMedicosIds)
    {
        try
        {
            var solicitudOriginal = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == solicitudOriginalId);

            if (solicitudOriginal == null)
                return ResultModel<int>.ErrorResult("Solicitud original no encontrada");

            if (solicitudOriginal.PacienteId == null)
                return ResultModel<int>.ErrorResult("Paciente no asociado a la solicitud");

            // Crear nueva solicitud como renovación
            var nuevaSolicitud = new TbSolRegCannabis
            {
                PacienteId = solicitudOriginal.PacienteId,
                FechaSolicitud = DateTime.Now,
                EsRenovacion = true,
                EstadoSolicitudId = await _context.TbEstadoSolicitud
                    .Where(e => e.NombreEstado == "Pendiente")
                    .Select(e => e.IdEstado)
                    .FirstOrDefaultAsync(),
                CreadaPor = "Sistema_Renovacion",
                FechaUltimaRenovacion = DateTime.Now
            };

            // Generar número de solicitud secuencial
            var anio = DateTime.Now.Year;
            var secuencia = await _context.TbSolSecuencia
                .Where(s => s.Anio == anio && s.IdEntidad == 1)
                .FirstOrDefaultAsync();

            if (secuencia == null)
            {
                secuencia = new TbSolSecuencia
                {
                    IdEntidad = 1,
                    Anio = anio,
                    Numeracion = 1,
                    IsActivo = true
                };
                _context.TbSolSecuencia.Add(secuencia);
            }
            else
            {
                secuencia.Numeracion++;
                _context.TbSolSecuencia.Update(secuencia);
            }

            nuevaSolicitud.NumSolAnio = anio;
            nuevaSolicitud.NumSolSecuencia = secuencia.Numeracion;
            nuevaSolicitud.NumSolCompleta = $"REN-{anio}-{secuencia.Numeracion:0000}";

            _context.TbSolRegCannabis.Add(nuevaSolicitud);
            await _context.SaveChangesAsync();

            // Copiar documentos médicos seleccionados
            if (documentosMedicosIds.Any())
            {
                var documentosSeleccionados = await _context.TbDocumentoAdjunto
                    .Where(d => documentosMedicosIds.Contains(d.Id) && d.EsDocumentoMedico)
                    .ToListAsync();

                foreach (var documento in documentosSeleccionados)
                {
                    var nuevoDocumento = new TbDocumentoAdjunto
                    {
                        SolRegCannabisId = nuevaSolicitud.Id,
                        TipoDocumentoId = documento.TipoDocumentoId,
                        NombreOriginal = documento.NombreOriginal,
                        NombreGuardado = documento.NombreGuardado,
                        Url = documento.Url,
                        EsDocumentoMedico = true,
                        MedicoId = documento.MedicoId,
                        Categoria = documento.Categoria,
                        Version = (documento.Version ?? 0) + 1,
                        IsValido = true
                    };
                    _context.TbDocumentoAdjunto.Add(nuevoDocumento);
                }
            }

            await _context.SaveChangesAsync();

            // Enviar notificación al paciente
            if (solicitudOriginal.Paciente?.CorreoElectronico != null)
            {
                await _emailService.EnviarCorreoAsync(
                    solicitudOriginal.Paciente.CorreoElectronico,
                    "Renovación de Carnet de Cannabis Medicinal iniciada",
                    $"Estimado/a {solicitudOriginal.Paciente.PrimerNombre},<br><br>" +
                    $"Su proceso de renovación de carnet ha sido iniciado exitosamente.<br>" +
                    $"Número de solicitud de renovación: <strong>{nuevaSolicitud.NumSolCompleta}</strong><br><br>" +
                    $"El sistema le notificará cuando su renovación sea aprobada.<br><br>" +
                    "Saludos cordiales,<br>Equipo DIGESA"
                );
            }

            return ResultModel<int>.SuccessResult(nuevaSolicitud.Id, "Renovación iniciada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error iniciando renovación para solicitud {solicitudOriginalId}");
            return ResultModel<int>.ErrorResult("Error al iniciar la renovación");
        }
    }

    public async Task ProcesarRecordatoriosVencimientoAsync()
    {
        try
        {
            var hoy = DateTime.Now;
            
            // Carnets que vencen en 30, 15 y 7 días
            var fechasRecordatorio = new[] { 30, 15, 7 };
            
            foreach (var dias in fechasRecordatorio)
            {
                var fechaLimite = hoy.AddDays(dias);
                
                var carnetsPorVencer = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Where(s => s.CarnetActivo == true &&
                               s.FechaVencimientoCarnet.HasValue &&
                               s.FechaVencimientoCarnet.Value.Date == fechaLimite.Date &&
                               s.Paciente != null &&
                               !string.IsNullOrEmpty(s.Paciente.CorreoElectronico))
                    .ToListAsync();

                foreach (var carnet in carnetsPorVencer)
                {
                    // Verificar si ya se envió recordatorio para este día
                    var notificacionExistente = await _context.TbNotificacionVencimiento
                        .AnyAsync(n => n.SolRegCannabisId == carnet.Id &&
                                      n.DiasAntelacion == dias &&
                                      n.FechaEnvio.Value.Date == hoy.Date);

                    if (!notificacionExistente)
                    {
                        var mensaje = dias switch
                        {
                            30 => "Su carnet vencerá en 30 días. Por favor, inicie el proceso de renovación.",
                            15 => "Faltan 15 días para el vencimiento de su carnet. Recuerde renovarlo.",
                            7 => "¡Última semana! Su carnet vencerá en 7 días. Renueve lo antes posible.",
                            _ => "Recordatorio de vencimiento de carnet."
                        };

                        await _emailService.EnviarCorreoAsync(
                            carnet.Paciente.CorreoElectronico,
                            $"Recordatorio: Carnet vence en {dias} días",
                            $"Estimado/a {carnet.Paciente.PrimerNombre},<br><br>" +
                            $"{mensaje}<br><br>" +
                            $"<strong>Fecha de vencimiento:</strong> {carnet.FechaVencimientoCarnet.Value:dd/MM/yyyy}<br>" +
                            $"<strong>Número de carnet:</strong> {carnet.NumeroCarnet}<br><br>" +
                            $"Para renovar, acceda a: <a href='[URL_SISTEMA]/renovaciones'>Sistema de Renovaciones</a><br><br>" +
                            "Saludos cordiales,<br>Equipo DIGESA"
                        );

                        // Registrar notificación
                        var notificacion = new TbNotificacionVencimiento
                        {
                            SolRegCannabisId = carnet.Id,
                            DiasAntelacion = dias,
                            FechaEnvio = hoy,
                            EmailEnviado = true,
                            UsuarioNotificado = carnet.Paciente.CorreoElectronico,
                            TipoNotificacion = "RecordatorioVencimiento"
                        };

                        _context.TbNotificacionVencimiento.Add(notificacion);
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Procesados recordatorios de vencimiento");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando recordatorios de vencimiento");
        }
    }

    public async Task InactivarCarnetsVencidosAsync()
    {
        try
        {
            var hoy = DateTime.Now;
            var carnetsVencidos = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Where(s => s.CarnetActivo == true &&
                           s.FechaVencimientoCarnet.HasValue &&
                           s.FechaVencimientoCarnet.Value.Date < hoy.Date)
                .ToListAsync();

            foreach (var carnet in carnetsVencidos)
            {
                carnet.CarnetActivo = false;
                
                // Notificar al paciente
                if (carnet.Paciente?.CorreoElectronico != null)
                {
                    await _emailService.EnviarCorreoAsync(
                        carnet.Paciente.CorreoElectronico,
                        "Carnet de Cannabis Medicinal vencido",
                        $"Estimado/a {carnet.Paciente.PrimerNombre},<br><br>" +
                        $"Su carnet de cannabis medicinal ha vencido el {carnet.FechaVencimientoCarnet.Value:dd/MM/yyyy}.<br>" +
                        $"<strong>Número de carnet:</strong> {carnet.NumeroCarnet}<br><br>" +
                        $"Para volver a tener un carnet activo, deberá realizar una nueva solicitud completa.<br>" +
                        "Saludos cordiales,<br>Equipo DIGESA"
                    );
                }
            }

            if (carnetsVencidos.Any())
            {
                _context.TbSolRegCannabis.UpdateRange(carnetsVencidos);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Inactivados {carnetsVencidos.Count} carnets vencidos");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inactivando carnets vencidos");
        }
    }

    public async Task<List<DIGESA.Models.CannabisModels.RenovacionPendienteModel>> ObtenerRenovacionesPendientesAsync()
    {
        try
        {
            return await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .Where(s => s.EsRenovacion == true && 
                           s.EstadoSolicitud.NombreEstado == "Pendiente")
                .OrderBy(s => s.FechaSolicitud)
                .Select(s => new DIGESA.Models.CannabisModels.RenovacionPendienteModel
                {
                    SolicitudRenovacionId = s.Id,
                    NumeroSolicitudRenovacion = s.NumSolCompleta ?? "N/A",
                    SolicitudOriginalId = s.Id,
                    PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                    PacienteDocumento = s.Paciente.DocumentoCedula ?? s.Paciente.DocumentoPasaporte ?? "N/A",
                    FechaSolicitudRenovacion = s.FechaSolicitud ?? DateTime.MinValue,
                    DiasDesdeVencimiento = s.FechaVencimientoCarnet.HasValue ? 
                        (DateTime.Now - s.FechaVencimientoCarnet.Value).Days : 0,
                    Estado = s.EstadoSolicitud.NombreEstado
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo renovaciones pendientes");
            return new List<DIGESA.Models.CannabisModels.RenovacionPendienteModel>();
        }
    }

    public async Task<ResultModel<bool>> CompletarRenovacionAsync(int solicitudRenovacionId, bool aprobada, string comentario)
    {
        try
        {
            var solicitudRenovacion = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == solicitudRenovacionId);

            if (solicitudRenovacion == null)
                return ResultModel<bool>.ErrorResult("Solicitud de renovación no encontrada");

            if (aprobada)
            {
                // Generar nuevo número de carnet
                var nuevoCarnetResult = await _carnetService.GenerarNumeroCarnetAsync(
                    solicitudRenovacion.PacienteId.Value, 
                    false);

                if (!nuevoCarnetResult.Success)
                    return ResultModel<bool>.ErrorResult("Error generando nuevo carnet");

                // Asignar nuevo carnet
                var asignacionResult = await _carnetService.AsignarCarnetSolicitudAsync(
                    solicitudRenovacionId, 
                    nuevoCarnetResult.Data,
                    false);

                if (!asignacionResult.Success)
                    return ResultModel<bool>.ErrorResult("Error asignando nuevo carnet");

                // Notificar al paciente
                if (solicitudRenovacion.Paciente?.CorreoElectronico != null)
                {
                    await _emailService.EnviarCorreoAsync(
                        solicitudRenovacion.Paciente.CorreoElectronico,
                        "Renovación de carnet aprobada",
                        $"Estimado/a {solicitudRenovacion.Paciente.PrimerNombre},<br><br>" +
                        $"Su renovación de carnet ha sido aprobada.<br>" +
                        $"<strong>Nuevo número de carnet:</strong> {nuevoCarnetResult.Data}<br>" +
                        $"<strong>Válido hasta:</strong> {DateTime.Now.AddYears(2):dd/MM/yyyy}<br><br>" +
                        "Saludos cordiales,<br>Equipo DIGESA"
                    );
                }
            }
            else
            {
                // Notificar rechazo
                if (solicitudRenovacion.Paciente?.CorreoElectronico != null)
                {
                    await _emailService.EnviarCorreoAsync(
                        solicitudRenovacion.Paciente.CorreoElectronico,
                        "Renovación de carnet rechazada",
                        $"Estimado/a {solicitudRenovacion.Paciente.PrimerNombre},<br><br>" +
                        $"Su renovación de carnet ha sido rechazada.<br>" +
                        $"<strong>Motivo:</strong> {comentario}<br><br>" +
                        "Para más información, contacte a DIGESA.<br><br>" +
                        "Saludos cordiales,<br>Equipo DIGESA"
                    );
                }
            }

            // Actualizar estado
            var estado = await _context.TbEstadoSolicitud
                .Where(e => e.NombreEstado == (aprobada ? "Aprobada" : "Rechazada"))
                .FirstOrDefaultAsync();
            
            if (estado != null)
            {
                solicitudRenovacion.EstadoSolicitudId = estado.IdEstado;
            }
            
            solicitudRenovacion.ComentarioRevision = comentario;
            solicitudRenovacion.FechaRevision = DateOnly.FromDateTime(DateTime.Now);

            _context.TbSolRegCannabis.Update(solicitudRenovacion);
            await _context.SaveChangesAsync();

            var mensaje = aprobada ? "Renovación aprobada" : "Renovación rechazada";
            return ResultModel<bool>.SuccessResult(true, $"{mensaje} exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error completando renovación {solicitudRenovacionId}");
            return ResultModel<bool>.ErrorResult("Error al completar la renovación");
        }
    }
}