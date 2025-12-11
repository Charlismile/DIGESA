using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioRenovaciones : IServicioRenovaciones
    {
        private readonly DbContextDigesa _context;
        private readonly IServicioConfiguracion _servicioConfiguracion;
        private readonly IServicioHistorial _servicioHistorial;
        private readonly IServicioNotificaciones _servicioNotificaciones;
        private readonly ILogger<ServicioRenovaciones> _logger;

        public ServicioRenovaciones(
            DbContextDigesa context,
            IServicioConfiguracion servicioConfiguracion,
            IServicioHistorial servicioHistorial,
            IServicioNotificaciones servicioNotificaciones,
            ILogger<ServicioRenovaciones> logger)
        {
            _context = context;
            _servicioConfiguracion = servicioConfiguracion;
            _servicioHistorial = servicioHistorial;
            _servicioNotificaciones = servicioNotificaciones;
            _logger = logger;
        }

        public async Task<ResultadoRenovacionViewModel> IniciarRenovacion(int solicitudId, string usuarioId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Validar solicitud
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == solicitudId);

                if (solicitud == null)
                    return ResultadoRenovacionViewModel.Error("Solicitud no encontrada");

                var resultadoValidacion = await ValidarPuedeRenovar(solicitud);
                if (!resultadoValidacion.Exitoso)
                    return resultadoValidacion;

                // 2. Obtener configuración
                var config = await _servicioConfiguracion.ObtenerConfiguracionCompleta();

                // 3. Crear nueva solicitud de renovación
                var nuevaSolicitud = new TbSolRegCannabis
                {
                    PacienteId = solicitud.PacienteId,
                    FechaSolicitud = DateTime.Now,
                    EstadoSolicitudId = await ObtenerEstadoId("Pendiente Renovación"),
                    EsRenovacion = true,
                    SolicitudPadreId = solicitud.SolicitudPadreId ?? solicitud.Id,
                    VersionCarnet = solicitud.VersionCarnet + 1,
                    CreadaPor = usuarioId,
                    FotoCarnetUrl = solicitud.FotoCarnetUrl,
                    FirmaDigitalUrl = solicitud.FirmaDigitalUrl,
                    FechaVencimientoCarnet = solicitud.FechaVencimientoCarnet,
                    CarnetActivo = true
                };

                _context.TbSolRegCannabis.Add(nuevaSolicitud);
                await _context.SaveChangesAsync();

                // 4. Registrar en historial
                await _servicioHistorial.RegistrarRenovacion(
                    solicitud.Id,
                    nuevaSolicitud.Id,
                    usuarioId,
                    "Renovación por vencimiento");

                // 5. Cambiar estado de la solicitud anterior
                solicitud.EstadoSolicitudId = await ObtenerEstadoId("En Renovación");
                await _context.SaveChangesAsync();

                // 6. Enviar notificación si está configurado
                if (config.NotificarPorEmail && !string.IsNullOrEmpty(solicitud.Paciente.CorreoElectronico))
                {
                    await _servicioNotificaciones.EnviarNotificacionRenovacionIniciada(
                        nuevaSolicitud.Id,
                        solicitud.Paciente.CorreoElectronico);
                }

                await transaction.CommitAsync();

                var solicitudViewModel = await MapToViewModelConHistorial(nuevaSolicitud.Id);

                return ResultadoRenovacionViewModel.Exito(
                    nuevaSolicitud.Id,
                    "Renovación iniciada exitosamente. Complete los documentos requeridos.",
                    solicitudViewModel);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error al iniciar renovación para solicitud {SolicitudId}", solicitudId);
                return ResultadoRenovacionViewModel.Error($"Error del sistema: {ex.Message}");
            }
        }

        public async Task<ResultadoRenovacionViewModel> CompletarRenovacion(int solicitudNuevaId, string usuarioId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Validar solicitud de renovación
                var solicitudNueva = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == solicitudNuevaId && s.EsRenovacion == true);

                if (solicitudNueva == null)
                    return ResultadoRenovacionViewModel.Error("Solicitud de renovación no encontrada");

                // 2. Buscar solicitud anterior
                var solicitudAnterior = await _context.TbSolRegCannabis
                    .FirstOrDefaultAsync(s => s.Id == solicitudNueva.SolicitudPadreId);

                if (solicitudAnterior == null)
                    return ResultadoRenovacionViewModel.Error("No se encontró la solicitud anterior");

                // 3. Verificar que tenga todos los documentos requeridos
                var documentosCompletos = await VerificarDocumentosCompletos(solicitudNuevaId);
                if (!documentosCompletos)
                    return ResultadoRenovacionViewModel.Error("Faltan documentos requeridos para la renovación");

                // 4. Aprobar renovación
                solicitudNueva.EstadoSolicitudId = await ObtenerEstadoId("Aprobado");
                solicitudNueva.FechaAprobacion = DateTime.Now;
                solicitudNueva.UsuarioRevisor = usuarioId;
                solicitudNueva.FechaEmisionCarnet = DateTime.Now;

                // Calcular nueva fecha de vencimiento
                var diasVigencia = await _servicioConfiguracion.ObtenerValorEntero("DiasVigenciaCarnet", 730);
                solicitudNueva.FechaVencimientoCarnet = DateTime.Now.AddDays(diasVigencia);

                // Generar nuevo número de carnet
                solicitudNueva.NumeroCarnet = await GenerarNumeroCarnet(solicitudNueva.PacienteId.Value,
                    solicitudNueva.VersionCarnet);

                // 5. Inactivar carnet anterior
                solicitudAnterior.CarnetActivo = false;
                solicitudAnterior.FechaInactivacion = DateTime.Now;
                solicitudAnterior.RazonInactivacion = "Renovado por nuevo carnet";
                solicitudAnterior.UsuarioInactivador = usuarioId;

                // 6. Registrar historial
                await _servicioHistorial.RegistrarCambioEstado(
                    solicitudNueva.Id,
                    "Pendiente Renovación",
                    "Aprobado",
                    usuarioId,
                    "Renovación completada exitosamente");

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // 7. Enviar notificación
                var config = await _servicioConfiguracion.ObtenerConfiguracionCompleta();
                if (config.NotificarPorEmail && !string.IsNullOrEmpty(solicitudNueva.Paciente.CorreoElectronico))
                {
                    await _servicioNotificaciones.EnviarNotificacionRenovacionCompletada(
                        solicitudNueva.Id,
                        solicitudNueva.Paciente.CorreoElectronico);
                }

                var solicitudViewModel = await MapToViewModelConHistorial(solicitudNueva.Id);

                return ResultadoRenovacionViewModel.Exito(
                    solicitudNueva.Id,
                    "Renovación completada exitosamente. Nuevo carnet emitido.",
                    solicitudViewModel);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error al completar renovación {SolicitudId}", solicitudNuevaId);
                return ResultadoRenovacionViewModel.Error($"Error del sistema: {ex.Message}");
            }
        }

        public async Task<bool> CancelarRenovacion(int solicitudId, string usuarioId, string motivo)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == solicitudId && s.EsRenovacion == true);

                if (solicitud == null)
                    return false;

                // Buscar solicitud anterior para reactivarla
                if (solicitud.SolicitudPadreId.HasValue)
                {
                    var solicitudAnterior = await _context.TbSolRegCannabis
                        .FirstOrDefaultAsync(s => s.Id == solicitud.SolicitudPadreId);

                    if (solicitudAnterior != null)
                    {
                        solicitudAnterior.EstadoSolicitudId = await ObtenerEstadoId("Aprobado");
                        solicitudAnterior.CarnetActivo = true;
                    }
                }

                // Cancelar la renovación
                solicitud.EstadoSolicitudId = await ObtenerEstadoId("Cancelado");
                solicitud.ComentarioRevision = $"Renovación cancelada: {motivo}";
                solicitud.UsuarioRevisor = usuarioId;
                solicitud.FechaRevision = DateTime.Now;

                await _servicioHistorial.RegistrarCambioEstado(
                    solicitud.Id,
                    "Pendiente Renovación",
                    "Cancelado",
                    usuarioId,
                    $"Renovación cancelada: {motivo}");

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error cancelando renovación {SolicitudId}", solicitudId);
                return false;
            }
        }

        public async Task<List<SolicitudCannabisViewModel>> ObtenerSolicitudesPorVencer(int dias = 30)
        {
            try
            {
                var fechaLimite = DateTime.Now.AddDays(dias);

                var solicitudes = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Include(s => s.EstadoSolicitud)
                    .Where(s => s.CarnetActivo == true &&
                                s.FechaVencimientoCarnet.HasValue &&
                                s.FechaVencimientoCarnet.Value <= fechaLimite &&
                                s.FechaVencimientoCarnet.Value > DateTime.Now &&
                                s.EstadoSolicitud.NombreEstado == "Aprobado")
                    .OrderBy(s => s.FechaVencimientoCarnet)
                    .ToListAsync();

                return solicitudes.Select(s => MapToViewModel(s)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitudes por vencer");
                return new List<SolicitudCannabisViewModel>();
            }
        }

        public async Task<List<SolicitudCannabisViewModel>> ObtenerSolicitudesVencidas()
        {
            try
            {
                var solicitudes = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Include(s => s.EstadoSolicitud)
                    .Where(s => s.CarnetActivo == true &&
                                s.FechaVencimientoCarnet.HasValue &&
                                s.FechaVencimientoCarnet.Value < DateTime.Now &&
                                s.EstadoSolicitud.NombreEstado == "Aprobado")
                    .OrderByDescending(s => s.FechaVencimientoCarnet)
                    .ToListAsync();

                return solicitudes.Select(s => MapToViewModel(s)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitudes vencidas");
                return new List<SolicitudCannabisViewModel>();
            }
        }

        public async Task<List<SolicitudCannabisViewModel>> ObtenerSolicitudesParaInactivar()
        {
            try
            {
                var autoInactivarDias = await _servicioConfiguracion.ObtenerValorEntero("AutoInactivarDias", 0);

                if (autoInactivarDias <= 0)
                    return new List<SolicitudCannabisViewModel>();

                var fechaLimite = DateTime.Now.AddDays(-autoInactivarDias);

                var solicitudes = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Include(s => s.EstadoSolicitud)
                    .Where(s => s.CarnetActivo == true &&
                                s.FechaVencimientoCarnet.HasValue &&
                                s.FechaVencimientoCarnet.Value <= fechaLimite &&
                                s.EstadoSolicitud.NombreEstado == "Aprobado")
                    .OrderByDescending(s => s.FechaVencimientoCarnet)
                    .ToListAsync();

                return solicitudes.Select(s => MapToViewModel(s)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitudes para inactivar");
                return new List<SolicitudCannabisViewModel>();
            }
        }

        public async Task<bool> ProcesarRenovacionesAutomaticas()
        {
            try
            {
                var config = await _servicioConfiguracion.ObtenerConfiguracionCompleta();

                // Obtener solicitudes que pueden renovarse automáticamente
                var fechaLimite = DateTime.Now.AddDays(config.DiasAntesNotificar);

                var solicitudesParaRenovar = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Where(s => s.CarnetActivo == true &&
                                s.FechaVencimientoCarnet.HasValue &&
                                s.FechaVencimientoCarnet.Value <= fechaLimite &&
                                s.FechaVencimientoCarnet.Value > DateTime.Now &&
                                s.EsRenovacion == false)
                    .ToListAsync();

                var renovacionesProcesadas = 0;

                foreach (var solicitud in solicitudesParaRenovar)
                {
                    try
                    {
                        // Verificar si ya tiene una renovación en proceso
                        var renovacionExistente = await _context.TbSolRegCannabis
                            .AnyAsync(s => s.SolicitudPadreId == solicitud.Id &&
                                           s.EsRenovacion == true &&
                                           s.EstadoSolicitudId != await ObtenerEstadoId("Cancelado"));

                        if (renovacionExistente)
                            continue;

                        // Iniciar renovación automática
                        var resultado = await IniciarRenovacion(solicitud.Id, "Sistema");
                        if (resultado.Exitoso)
                            renovacionesProcesadas++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando renovación automática para solicitud {SolicitudId}",
                            solicitud.Id);
                    }
                }

                _logger.LogInformation("Procesadas {Count} renovaciones automáticas", renovacionesProcesadas);
                return renovacionesProcesadas > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando renovaciones automáticas");
                return false;
            }
        }

        public async Task<bool> EnviarNotificacionesVencimiento()
        {
            try
            {
                var config = await _servicioConfiguracion.ObtenerConfiguracionCompleta();

                if (!config.NotificarPorEmail && !config.NotificarPorSMS)
                    return false;

                var solicitudesPorVencer = await ObtenerSolicitudesPorVencer(config.DiasAntesNotificar);
                var notificacionesEnviadas = 0;

                foreach (var solicitud in solicitudesPorVencer)
                {
                    try
                    {
                        var diasRestantes = solicitud.FechaVencimientoCarnet.HasValue
                            ? (int)(solicitud.FechaVencimientoCarnet.Value - DateTime.Now).TotalDays
                            : 0;

                        // Obtener paciente para contacto
                        var paciente = await _context.TbPaciente
                            .FirstOrDefaultAsync(p => p.Id == solicitud.PacienteId);

                        if (paciente == null) continue;

                        // Enviar por email
                        if (config.NotificarPorEmail && !string.IsNullOrEmpty(paciente.CorreoElectronico))
                        {
                            var enviado = await _servicioNotificaciones.EnviarNotificacionVencimiento(
                                solicitud.Id,
                                paciente.CorreoElectronico,
                                diasRestantes);

                            if (enviado)
                            {
                                await _servicioHistorial.RegistrarNotificacion(
                                    solicitud.Id,
                                    $"Vencimiento_{diasRestantes}dias",
                                    "Email",
                                    paciente.CorreoElectronico,
                                    true);
                                notificacionesEnviadas++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error enviando notificación para solicitud {SolicitudId}", solicitud.Id);

                        await _servicioHistorial.RegistrarNotificacion(
                            solicitud.Id,
                            "Vencimiento",
                            "Sistema",
                            null,
                            false,
                            ex.Message);
                    }
                }

                _logger.LogInformation("Se enviaron {Count} notificaciones de vencimiento", notificacionesEnviadas);
                return notificacionesEnviadas > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el servicio de notificaciones de vencimiento");
                return false;
            }
        }

        public async Task<bool> InactivarCarnetsVencidos()
        {
            try
            {
                var autoInactivarDias = await _servicioConfiguracion.ObtenerValorEntero("AutoInactivarDias", 0);

                if (autoInactivarDias <= 0)
                    return false;

                var fechaLimite = DateTime.Now.AddDays(-autoInactivarDias);

                var carnetVencidos = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Where(s => s.CarnetActivo == true &&
                                s.FechaVencimientoCarnet.HasValue &&
                                s.FechaVencimientoCarnet.Value <= fechaLimite)
                    .ToListAsync();

                foreach (var carnet in carnetVencidos)
                {
                    using var transaction = await _context.Database.BeginTransactionAsync();

                    try
                    {
                        carnet.CarnetActivo = false;
                        carnet.FechaInactivacion = DateTime.Now;
                        carnet.RazonInactivacion = "Inactivación automática por vencimiento";
                        carnet.UsuarioInactivador = "Sistema";

                        await _servicioHistorial.RegistrarInactivacion(
                            carnet.Id,
                            "Sistema",
                            "Inactivación automática por vencimiento");

                        // Enviar notificación
                        var config = await _servicioConfiguracion.ObtenerConfiguracionCompleta();
                        if (config.NotificarPorEmail && !string.IsNullOrEmpty(carnet.Paciente.CorreoElectronico))
                        {
                            await _servicioNotificaciones.EnviarNotificacionCarnetInactivado(
                                carnet.Id,
                                carnet.Paciente.CorreoElectronico,
                                "Inactivación automática por vencimiento");
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Error inactivando carnet {CarnetId}", carnet.Id);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inactivando carnets vencidos");
                return false;
            }
        }

        public async Task<SolicitudConHistorialViewModel> ObtenerSolicitudConHistorial(int solicitudId)
        {
            try
            {
                return await MapToViewModelConHistorial(solicitudId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitud con historial {SolicitudId}", solicitudId);
                return null;
            }
        }

        public async Task<ReporteRenovacionesViewModel> GenerarReporteRenovaciones(DateTime fechaInicio,
            DateTime fechaFin)
        {
            try
            {
                var reporte = new ReporteRenovacionesViewModel
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                };

                // Obtener renovaciones en el período
                var renovaciones = await _context.TbHistorialRenovacion
                    .Include(r => r.SolicitudAnterior)
                    .Include(r => r.SolicitudNueva)
                    .ThenInclude(s => s.Paciente)
                    .Where(r => r.FechaRenovacion >= fechaInicio && r.FechaRenovacion <= fechaFin)
                    .ToListAsync();

                reporte.TotalRenovaciones = renovaciones.Count;
                reporte.RenovacionesExitosas = renovaciones.Count(r =>
                    r.SolicitudNueva.EstadoSolicitudId == await ObtenerEstadoId("Aprobado"));
                reporte.RenovacionesFallidas = renovaciones.Count(r =>
                    r.SolicitudNueva.EstadoSolicitudId == await ObtenerEstadoId("Rechazado"));
                reporte.RenovacionesPendientes = renovaciones.Count(r =>
                    r.SolicitudNueva.EstadoSolicitudId == await ObtenerEstadoId("Pendiente Renovación"));

                // Agrupar por mes
                var renovacionesPorMes = renovaciones
                    .GroupBy(r => new { r.FechaRenovacion.Year, r.FechaRenovacion.Month })
                    .Select(g => new RenovacionMesViewModel
                    {
                        Mes = $"{g.Key.Year}-{g.Key.Month:00}",
                        Cantidad = g.Count(),
                        Porcentaje = reporte.TotalRenovaciones > 0
                            ? (decimal)g.Count() / reporte.TotalRenovaciones * 100
                            : 0
                    })
                    .ToList();

                reporte.RenovacionesPorMes = renovacionesPorMes;

                // Detalles
                reporte.Detalles = renovaciones.Select(r => new RenovacionDetalleViewModel
                {
                    SolicitudId = r.SolicitudNuevaId,
                    NumeroCarnet = r.SolicitudNueva.NumeroCarnet,
                    Paciente = $"{r.SolicitudNueva.Paciente?.PrimerNombre} {r.SolicitudNueva.Paciente?.PrimerApellido}",
                    FechaRenovacion = r.FechaRenovacion,
                    Estado = r.SolicitudNueva.EstadoSolicitudId.ToString(),
                    Usuario = r.UsuarioRenovador,
                    Comentarios = r.Comentarios
                }).ToList();

                return reporte;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de renovaciones");
                return new ReporteRenovacionesViewModel();
            }
        }

        public async Task<byte[]> ExportarReporteRenovaciones(DateTime fechaInicio, DateTime fechaFin, string formato)
        {
            try
            {
                var reporte = await GenerarReporteRenovaciones(fechaInicio, fechaFin);

                if (formato.ToLower() == "excel")
                {
                    return ExportarAExcel(reporte);
                }
                else if (formato.ToLower() == "pdf")
                {
                    return ExportarAPDF(reporte);
                }
                else
                {
                    return Array.Empty<byte>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando reporte de renovaciones");
                return Array.Empty<byte>();
            }
        }

        // Métodos auxiliares privados
        private async Task<ResultadoRenovacionViewModel> ValidarPuedeRenovar(TbSolRegCannabis solicitud)
        {
            if (!solicitud.CarnetActivo.HasValue || solicitud.CarnetActivo.Value == false)
                return ResultadoRenovacionViewModel.Error("El carnet no está activo");

            if (!solicitud.FechaVencimientoCarnet.HasValue)
                return ResultadoRenovacionViewModel.Error("No hay fecha de vencimiento registrada");

            var config = await _servicioConfiguracion.ObtenerConfiguracionCompleta();
            var fechaVencimiento = solicitud.FechaVencimientoCarnet.Value;
            var diasHastaVencimiento = (fechaVencimiento - DateTime.Now).Days;

            // Puede renovar hasta X días antes del vencimiento
            if (diasHastaVencimiento > config.DiasAntesNotificar && diasHastaVencimiento > 0)
                return ResultadoRenovacionViewModel.Error(
                    $"Solo puede renovar {config.DiasAntesNotificar} días antes del vencimiento. Faltan {diasHastaVencimiento} días.");

            // Verificar período de gracia
            var diasDesdeVencimiento = (DateTime.Now - fechaVencimiento).Days;
            if (diasDesdeVencimiento > config.DiasGraciaRenovacion)
                return ResultadoRenovacionViewModel.Error(
                    $"Período de gracia excedido ({config.DiasGraciaRenovacion} días). El carnet venció hace {diasDesdeVencimiento} días.");

            // Verificar máximo de renovaciones
            if (solicitud.VersionCarnet >= config.MaximoRenovaciones)
                return ResultadoRenovacionViewModel.Error(
                    $"Límite de renovaciones alcanzado ({config.MaximoRenovaciones}). Contacte al administrador.");

            return ResultadoRenovacionViewModel.Exito(0, "Validación exitosa");
        }

        private async Task<bool> VerificarDocumentosCompletos(int solicitudId)
        {
            var documentosRequeridos = await _context.TbTipoDocumentoAdjunto
                .Where(t => t.EsParaRenovacion == true && t.EsObligatorio == true)
                .CountAsync();

            var documentosSubidos = await _context.TbDocumentoAdjunto
                .Where(d => d.SolRegCannabisId == solicitudId && d.IsValido == true)
                .CountAsync();

            return documentosSubidos >= documentosRequeridos;
        }

        private async Task<string> GenerarNumeroCarnet(int pacienteId, int version)
        {
            var anio = DateTime.Now.Year;
            return $"CAN-{anio}-{pacienteId:00000}-{version:00}";
        }

        private async Task<int> ObtenerEstadoId(string nombreEstado)
        {
            var estado = await _context.TbEstadoSolicitud
                .FirstOrDefaultAsync(e => e.NombreEstado == nombreEstado);
            return estado?.IdEstado ?? 1;
        }

        private async Task<SolicitudConHistorialViewModel> MapToViewModelConHistorial(int solicitudId)
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud == null)
                return null;

            var viewModel = new SolicitudConHistorialViewModel
            {
                Id = solicitud.Id,
                FechaSolicitud = solicitud.FechaSolicitud ?? DateTime.Now,
                PacienteId = solicitud.PacienteId,
                FechaRevision = solicitud.FechaRevision,
                UsuarioRevisor = solicitud.UsuarioRevisor,
                ComentarioRevision = solicitud.ComentarioRevision,
                NumSolCompleta = solicitud.NumSolCompleta,
                CreadaPor = solicitud.CreadaPor,
                FechaAprobacion = solicitud.FechaAprobacion,
                EstadoSolicitudId = solicitud.EstadoSolicitudId ?? 0,
                EsRenovacion = solicitud.EsRenovacion ?? false,
                FotoCarnetUrl = solicitud.FotoCarnetUrl,
                FirmaDigitalUrl = solicitud.FirmaDigitalUrl,
                CarnetActivo = solicitud.CarnetActivo ?? false,
                NumeroCarnet = solicitud.NumeroCarnet,
                FechaEmisionCarnet = solicitud.FechaEmisionCarnet,
                FechaVencimientoCarnet = solicitud.FechaVencimientoCarnet,
                FechaUltimaRenovacion = solicitud.FechaUltimaRenovacion,
                SolicitudPadreId = solicitud.SolicitudPadreId,
                VersionCarnet = solicitud.VersionCarnet,
                RazonInactivacion = solicitud.RazonInactivacion,
                FechaInactivacion = solicitud.FechaInactivacion,
                UsuarioInactivador = solicitud.UsuarioInactivador
            };

            // Mapear paciente si existe
            if (solicitud.Paciente != null)
            {
                viewModel.Paciente = new PacienteViewModel
                {
                    Id = solicitud.Paciente.Id,
                    PrimerNombre = solicitud.Paciente.PrimerNombre,
                    SegundoNombre = solicitud.Paciente.SegundoNombre,
                    PrimerApellido = solicitud.Paciente.PrimerApellido,
                    SegundoApellido = solicitud.Paciente.SegundoApellido,
                    TipoDocumento = solicitud.Paciente.TipoDocumento,
                    DocumentoCedula = solicitud.Paciente.DocumentoCedula,
                    DocumentoPasaporte = solicitud.Paciente.DocumentoPasaporte,
                    Nacionalidad = solicitud.Paciente.Nacionalidad,
                    FechaNacimiento = solicitud.Paciente.FechaNacimiento ?? DateTime.MinValue,
                    Sexo = solicitud.Paciente.Sexo,
                    CorreoElectronico = solicitud.Paciente.CorreoElectronico
                };
            }

            // Mapear estado
            if (solicitud.EstadoSolicitud != null)
            {
                viewModel.EstadoSolicitud = new EstadoSolicitudViewModel
                {
                    IdEstado = solicitud.EstadoSolicitud.IdEstado,
                    NombreEstado = solicitud.EstadoSolicitud.NombreEstado
                };
            }

            return viewModel;
        }

        private SolicitudCannabisViewModel MapToViewModel(TbSolRegCannabis entity)
        {
            return new SolicitudCannabisViewModel
            {
                Id = entity.Id,
                FechaSolicitud = entity.FechaSolicitud ?? DateTime.Now,
                PacienteId = entity.PacienteId,
                FechaRevision = entity.FechaRevision,
                UsuarioRevisor = entity.UsuarioRevisor,
                ComentarioRevision = entity.ComentarioRevision,
                NumSolCompleta = entity.NumSolCompleta,
                CreadaPor = entity.CreadaPor,
                FechaAprobacion = entity.FechaAprobacion,
                EstadoSolicitudId = entity.EstadoSolicitudId ?? 0,
                EsRenovacion = entity.EsRenovacion ?? false,
                FotoCarnetUrl = entity.FotoCarnetUrl,
                FirmaDigitalUrl = entity.FirmaDigitalUrl,
                CarnetActivo = entity.CarnetActivo ?? false,
                NumeroCarnet = entity.NumeroCarnet,
                FechaEmisionCarnet = entity.FechaEmisionCarnet,
                FechaVencimientoCarnet = entity.FechaVencimientoCarnet,
                FechaUltimaRenovacion = entity.FechaUltimaRenovacion
            };
        }

        private byte[] ExportarAExcel(ReporteRenovacionesViewModel reporte)
        {
            // Implementación simplificada
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);

            writer.WriteLine("Reporte de Renovaciones");
            writer.WriteLine($"Período: {reporte.FechaInicio:dd/MM/yyyy} - {reporte.FechaFin:dd/MM/yyyy}");
            writer.WriteLine();
            writer.WriteLine($"Total Renovaciones: {reporte.TotalRenovaciones}");
            writer.WriteLine($"Renovaciones Exitosas: {reporte.RenovacionesExitosas}");
            writer.WriteLine($"Renovaciones Fallidas: {reporte.RenovacionesFallidas}");
            writer.WriteLine($"Renovaciones Pendientes: {reporte.RenovacionesPendientes}");
            writer.WriteLine();

            // Encabezados
            writer.WriteLine("Número Carnet,Paciente,Fecha Renovación,Estado,Usuario,Comentarios");

            // Datos
            foreach (var detalle in reporte.Detalles)
            {
                writer.WriteLine(
                    $"{detalle.NumeroCarnet},{detalle.Paciente},{detalle.FechaRenovacion:dd/MM/yyyy},{detalle.Estado},{detalle.Usuario},{detalle.Comentarios}");
            }

            writer.Flush();
            return memoryStream.ToArray();
        }

        private byte[] ExportarAPDF(ReporteRenovacionesViewModel reporte)
        {
            // Implementación simplificada
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);

            writer.WriteLine("REPORTE DE RENOVACIONES");
            writer.WriteLine("========================");
            writer.WriteLine($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}");
            writer.WriteLine($"Período: {reporte.FechaInicio:dd/MM/yyyy} - {reporte.FechaFin:dd/MM/yyyy}");
            writer.WriteLine();
            writer.WriteLine("ESTADÍSTICAS");
            writer.WriteLine($"Total Renovaciones: {reporte.TotalRenovaciones}");
            writer.WriteLine($"Renovaciones Exitosas: {reporte.RenovacionesExitosas}");
            writer.WriteLine($"Renovaciones Fallidas: {reporte.RenovacionesFallidas}");
            writer.WriteLine($"Renovaciones Pendientes: {reporte.RenovacionesPendientes}");
            writer.WriteLine();
            writer.WriteLine("DETALLES DE RENOVACIONES");
            writer.WriteLine(
                "=====================================================================================================");

            foreach (var detalle in reporte.Detalles)
            {
                writer.WriteLine($"Carnet: {detalle.NumeroCarnet}");
                writer.WriteLine($"Paciente: {detalle.Paciente}");
                writer.WriteLine($"Fecha: {detalle.FechaRenovacion:dd/MM/yyyy}");
                writer.WriteLine($"Estado: {detalle.Estado}");
                writer.WriteLine($"Usuario: {detalle.Usuario}");
                writer.WriteLine($"Comentarios: {detalle.Comentarios}");
                writer.WriteLine(
                    "-----------------------------------------------------------------------------------------------------");
            }

            writer.Flush();
            return memoryStream.ToArray();
        }
    }
}