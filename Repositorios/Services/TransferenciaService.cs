using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class TransferenciaService : ITransferenciaService
{
    private readonly DbContextDigesa _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<TransferenciaService> _logger;

    public TransferenciaService(
        DbContextDigesa context,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ILogger<TransferenciaService> logger)
    {
        _context = context;
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<ResultModel<int>> SolicitarTransferenciaAsync(SolicitudTransferenciaModel solicitud, string usuarioOrigenId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var solicitudExistente = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == solicitud.SolicitudId);

            if (solicitudExistente == null)
                return ResultModel<int>.ErrorResult("Solicitud no encontrada");

            var usuarioDestino = await _userManager.FindByIdAsync(solicitud.UsuarioDestinoId);
            if (usuarioDestino == null)
                return ResultModel<int>.ErrorResult("Usuario destino no encontrado");

            var transferenciaPendiente = await _context.TbTransferenciaResponsabilidad
                .AnyAsync(t => t.SolRegCannabisId == solicitud.SolicitudId && t.Estado == "Pendiente");

            if (transferenciaPendiente)
                return ResultModel<int>.ErrorResult("Ya existe una transferencia pendiente para esta solicitud");

            var transferencia = new TbTransferenciaResponsabilidad
            {
                SolRegCannabisId = solicitud.SolicitudId,
                UsuarioOrigenId = usuarioOrigenId,
                UsuarioDestinoId = solicitud.UsuarioDestinoId,
                FechaSolicitud = DateTime.Now,
                Estado = "Pendiente",
                Comentario = solicitud.Motivo
            };

            _context.TbTransferenciaResponsabilidad.Add(transferencia);
            await _context.SaveChangesAsync();

            // Enviar notificación si se requiere
            await EnviarNotificacionSolicitudAsync(transferencia, solicitudExistente, usuarioDestino, usuarioOrigenId);

            await transaction.CommitAsync();
            return ResultModel<int>.SuccessResult(transferencia.Id, "Transferencia solicitada exitosamente");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, $"Error solicitando transferencia");
            return ResultModel<int>.ErrorResult("Error al solicitar la transferencia", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<bool>> AprobarTransferenciaAsync(AprobacionTransferenciaInputModel aprobacion, string usuarioAprobador)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var transferencia = await _context.TbTransferenciaResponsabilidad
                .FirstOrDefaultAsync(t => t.Id == aprobacion.TransferenciaId);

            if (transferencia == null)
                return ResultModel<bool>.ErrorResult("Transferencia no encontrada");

            if (transferencia.Estado != "Pendiente")
                return ResultModel<bool>.ErrorResult("La transferencia ya fue procesada");

            // Registrar aprobación
            var aprobacionEntity = new TbAprobacionTransferencia
            {
                TransferenciaId = transferencia.Id,
                UsuarioId = usuarioAprobador,
                Aprobada = aprobacion.Aprobada,
                FechaAprobacion = DateTime.Now,
                Comentario = aprobacion.Comentario,
                NivelAprobacion = 1
            };

            _context.TbAprobacionTransferencia.Add(aprobacionEntity);

            transferencia.Estado = aprobacion.Aprobada ? "Aprobada" : "Rechazada";
            transferencia.FechaAprobacion = DateTime.Now;
            transferencia.AprobadoPor = usuarioAprobador;

            _context.TbTransferenciaResponsabilidad.Update(transferencia);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            var mensaje = aprobacion.Aprobada ? "Transferencia aprobada" : "Transferencia rechazada";
            return ResultModel<bool>.SuccessResult(true, $"{mensaje} exitosamente");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, $"Error aprobando transferencia");
            return ResultModel<bool>.ErrorResult("Error al procesar la aprobación", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<bool>> RechazarTransferenciaAsync(int transferenciaId, string motivo, string usuarioAprobador)
    {
        try
        {
            var transferencia = await _context.TbTransferenciaResponsabilidad
                .FirstOrDefaultAsync(t => t.Id == transferenciaId);

            if (transferencia == null)
                return ResultModel<bool>.ErrorResult("Transferencia no encontrada");

            if (transferencia.Estado != "Pendiente")
                return ResultModel<bool>.ErrorResult("La transferencia ya fue procesada");

            transferencia.Estado = "Rechazada";
            transferencia.FechaAprobacion = DateTime.Now;
            transferencia.AprobadoPor = usuarioAprobador;
            transferencia.Comentario = $"{transferencia.Comentario} | Rechazado: {motivo}";

            _context.TbTransferenciaResponsabilidad.Update(transferencia);
            await _context.SaveChangesAsync();

            return ResultModel<bool>.SuccessResult(true, "Transferencia rechazada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error rechazando transferencia");
            return ResultModel<bool>.ErrorResult("Error al rechazar la transferencia", new List<string> { ex.Message });
        }
    }

    public async Task<List<TransferenciaModel>> ObtenerTransferenciasPendientesAsync(string? usuarioId = null)
    {
        try
        {
            var query = _context.TbTransferenciaResponsabilidad
                .AsQueryable();

            if (!string.IsNullOrEmpty(usuarioId))
            {
                query = query.Where(t => t.UsuarioDestinoId == usuarioId);
            }

            var transferencias = await query
                .Where(t => t.Estado == "Pendiente")
                .OrderByDescending(t => t.FechaSolicitud)
                .ToListAsync();

            var resultado = new List<TransferenciaModel>();

            foreach (var transferencia in transferencias)
            {
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == transferencia.SolRegCannabisId);

                var usuarioOrigen = await _userManager.FindByIdAsync(transferencia.UsuarioOrigenId);
                var usuarioDestino = await _userManager.FindByIdAsync(transferencia.UsuarioDestinoId);

                resultado.Add(new TransferenciaModel
                {
                    Id = transferencia.Id,
                    SolicitudId = transferencia.SolRegCannabisId,
                    NumeroSolicitud = solicitud?.NumSolCompleta ?? "N/A",
                    PacienteNombre = $"{solicitud?.Paciente?.PrimerNombre} {solicitud?.Paciente?.PrimerApellido}",
                    UsuarioOrigenId = transferencia.UsuarioOrigenId,
                    UsuarioOrigenNombre = $"{usuarioOrigen?.FirstName} {usuarioOrigen?.LastName}",
                    UsuarioDestinoId = transferencia.UsuarioDestinoId,
                    UsuarioDestinoNombre = $"{usuarioDestino?.FirstName} {usuarioDestino?.LastName}",
                    FechaSolicitud = transferencia.FechaSolicitud ?? DateTime.MinValue,
                    FechaAprobacion = transferencia.FechaAprobacion,
                    Estado = transferencia.Estado ?? "Desconocido",
                    Comentario = transferencia.Comentario ?? string.Empty,
                    AprobadoPor = transferencia.AprobadoPor ?? string.Empty
                });
            }

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo transferencias pendientes");
            return new List<TransferenciaModel>();
        }
    }

    public async Task<List<TransferenciaModel>> ObtenerTransferenciasPorSolicitudAsync(int solicitudId)
    {
        try
        {
            var transferencias = await _context.TbTransferenciaResponsabilidad
                .Where(t => t.SolRegCannabisId == solicitudId)
                .OrderByDescending(t => t.FechaSolicitud)
                .ToListAsync();

            var resultado = new List<TransferenciaModel>();

            foreach (var transferencia in transferencias)
            {
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == transferencia.SolRegCannabisId);

                var usuarioOrigen = await _userManager.FindByIdAsync(transferencia.UsuarioOrigenId);
                var usuarioDestino = await _userManager.FindByIdAsync(transferencia.UsuarioDestinoId);

                resultado.Add(new TransferenciaModel
                {
                    Id = transferencia.Id,
                    SolicitudId = transferencia.SolRegCannabisId,
                    NumeroSolicitud = solicitud?.NumSolCompleta ?? "N/A",
                    PacienteNombre = $"{solicitud?.Paciente?.PrimerNombre} {solicitud?.Paciente?.PrimerApellido}",
                    UsuarioOrigenId = transferencia.UsuarioOrigenId,
                    UsuarioOrigenNombre = $"{usuarioOrigen?.FirstName} {usuarioOrigen?.LastName}",
                    UsuarioDestinoId = transferencia.UsuarioDestinoId,
                    UsuarioDestinoNombre = $"{usuarioDestino?.FirstName} {usuarioDestino?.LastName}",
                    FechaSolicitud = transferencia.FechaSolicitud ?? DateTime.MinValue,
                    FechaAprobacion = transferencia.FechaAprobacion,
                    Estado = transferencia.Estado ?? "Desconocido",
                    Comentario = transferencia.Comentario ?? string.Empty,
                    AprobadoPor = transferencia.AprobadoPor ?? string.Empty
                });
            }

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo transferencias para solicitud {solicitudId}");
            return new List<TransferenciaModel>();
        }
    }

    public async Task<TransferenciaModel?> ObtenerTransferenciaDetalleAsync(int transferenciaId)
    {
        try
        {
            var transferencia = await _context.TbTransferenciaResponsabilidad
                .FirstOrDefaultAsync(t => t.Id == transferenciaId);

            if (transferencia == null)
                return null;

            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == transferencia.SolRegCannabisId);

            var usuarioOrigen = await _userManager.FindByIdAsync(transferencia.UsuarioOrigenId);
            var usuarioDestino = await _userManager.FindByIdAsync(transferencia.UsuarioDestinoId);

            return new TransferenciaModel
            {
                Id = transferencia.Id,
                SolicitudId = transferencia.SolRegCannabisId,
                NumeroSolicitud = solicitud?.NumSolCompleta ?? "N/A",
                PacienteNombre = $"{solicitud?.Paciente?.PrimerNombre} {solicitud?.Paciente?.PrimerApellido}",
                UsuarioOrigenId = transferencia.UsuarioOrigenId,
                UsuarioOrigenNombre = $"{usuarioOrigen?.FirstName} {usuarioOrigen?.LastName}",
                UsuarioDestinoId = transferencia.UsuarioDestinoId,
                UsuarioDestinoNombre = $"{usuarioDestino?.FirstName} {usuarioDestino?.LastName}",
                FechaSolicitud = transferencia.FechaSolicitud ?? DateTime.MinValue,
                FechaAprobacion = transferencia.FechaAprobacion,
                Estado = transferencia.Estado ?? "Desconocido",
                Comentario = transferencia.Comentario ?? string.Empty,
                AprobadoPor = transferencia.AprobadoPor ?? string.Empty
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo detalle de transferencia {transferenciaId}");
            return null;
        }
    }

    public async Task<ResultModel<bool>> ProcesarNivelAprobacionAsync(int transferenciaId, int nivel, bool aprobada, string comentario, string usuarioAprobador)
    {
        try
        {
            var transferencia = await _context.TbTransferenciaResponsabilidad
                .FirstOrDefaultAsync(t => t.Id == transferenciaId);

            if (transferencia == null)
                return ResultModel<bool>.ErrorResult("Transferencia no encontrada");

            if (transferencia.Estado != "Pendiente")
                return ResultModel<bool>.ErrorResult("La transferencia ya fue procesada");

            var aprobacion = new TbAprobacionTransferencia
            {
                TransferenciaId = transferenciaId,
                UsuarioId = usuarioAprobador,
                Aprobada = aprobada,
                FechaAprobacion = DateTime.Now,
                Comentario = comentario,
                NivelAprobacion = nivel
            };

            _context.TbAprobacionTransferencia.Add(aprobacion);
            await _context.SaveChangesAsync();

            var mensaje = aprobada ? "Nivel aprobado" : "Nivel rechazado";
            return ResultModel<bool>.SuccessResult(true, $"{mensaje} exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error procesando nivel de aprobación");
            return ResultModel<bool>.ErrorResult("Error al procesar el nivel de aprobación", new List<string> { ex.Message });
        }
    }

    // Método auxiliar
    private async Task EnviarNotificacionSolicitudAsync(
        TbTransferenciaResponsabilidad transferencia,
        TbSolRegCannabis solicitud,
        ApplicationUser usuarioDestino,
        string usuarioOrigenId)
    {
        try
        {
            var usuarioOrigen = await _userManager.FindByIdAsync(usuarioOrigenId);
            
            if (usuarioDestino.Email != null && usuarioOrigen != null)
            {
                var asunto = "Solicitud de Transferencia de Responsabilidad";
                var cuerpo = $"Estimado/a {usuarioDestino.FirstName},<br><br>" +
                            $"El usuario {usuarioOrigen.FirstName} {usuarioOrigen.LastName} le ha solicitado transferir la responsabilidad de la siguiente solicitud:<br><br>" +
                            $"<strong>Solicitud:</strong> {solicitud.NumSolCompleta}<br>" +
                            $"<strong>Paciente:</strong> {solicitud.Paciente?.PrimerNombre} {solicitud.Paciente?.PrimerApellido}<br>" +
                            $"<strong>Motivo:</strong> {transferencia.Comentario}<br><br>" +
                            $"Por favor, acceda al sistema para revisar esta transferencia.<br><br>" +
                            $"Atentamente,<br>Sistema DIGESA";

                await _emailService.EnviarCorreoAsync(usuarioDestino.Email, asunto, cuerpo);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error enviando notificación de transferencia");
        }
    }
}