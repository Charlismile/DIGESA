using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Historial;
using DIGESA.Models.CannabisModels.Renovaciones;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioHistorial : IServicioHistorial
    {
        private readonly DbContextDigesa _context;
        private readonly ILogger<ServicioHistorial> _logger;

        public ServicioHistorial(DbContextDigesa context, ILogger<ServicioHistorial> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task RegistrarCambioEstado(int solicitudId, string estadoAnterior, 
                                               string estadoNuevo, string usuario, 
                                               string comentario)
        {
            try
            {
                var estadoId = await ObtenerEstadoId(estadoNuevo);
                
                var historial = new TbSolRegCannabisHistorial
                {
                    SolRegCannabisId = solicitudId,
                    Comentario = comentario,
                    UsuarioRevisor = usuario,
                    FechaCambio = DateOnly.FromDateTime(DateTime.Now),
                    EstadoSolicitudIdHistorial = estadoId
                };

                _context.TbSolRegCannabisHistorial.Add(historial);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando cambio de estado para solicitud {SolicitudId}", solicitudId);
                throw;
            }
        }

        public async Task RegistrarRenovacion(int solicitudAnteriorId, int solicitudNuevaId, 
                                             string usuario, string razon)
        {
            try
            {
                var historialRenovacion = new TbHistorialRenovacion
                {
                    SolicitudAnteriorId = solicitudAnteriorId,
                    SolicitudNuevaId = solicitudNuevaId,
                    FechaRenovacion = DateTime.Now,
                    RazonRenovacion = razon,
                    UsuarioRenovador = usuario,
                    Comentarios = $"Renovación registrada por {usuario}"
                };

                _context.TbHistorialRenovacion.Add(historialRenovacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando renovación de {AnteriorId} a {NuevaId}", 
                    solicitudAnteriorId, solicitudNuevaId);
                throw;
            }
        }

        public async Task RegistrarInactivacion(int solicitudId, string usuario, string razon)
        {
            try
            {
                var solicitud = await _context.TbSolRegCannabis.FindAsync(solicitudId);
                if (solicitud == null) return;

                solicitud.RazonInactivacion = razon;
                solicitud.FechaInactivacion = DateTime.Now;
                solicitud.UsuarioInactivador = usuario;
                solicitud.CarnetActivo = false;

                await _context.SaveChangesAsync();
                
                await RegistrarCambioEstado(
                    solicitudId,
                    "Activo",
                    "Inactivo",
                    usuario,
                    $"Inactivación: {razon}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando inactivación para solicitud {SolicitudId}", solicitudId);
                throw;
            }
        }

        public async Task RegistrarNotificacion(int solicitudId, string tipo, string metodo, 
                                               string destinatario, bool exitosa, string error = null)
        {
            try
            {
                var logNotificacion = new TbLogNotificaciones
                {
                    SolicitudId = solicitudId,
                    TipoNotificacion = tipo,
                    FechaEnvio = DateTime.Now,
                    MetodoEnvio = metodo,
                    Destinatario = destinatario,
                    Estado = exitosa ? "Enviado" : "Fallido",
                    Error = error
                };

                _context.TbLogNotificaciones.Add(logNotificacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando notificación para solicitud {SolicitudId}", solicitudId);
                throw;
            }
        }

        private LogNotificacionViewModel MapToLogNotificacionViewModel(TbLogNotificaciones entity)
        {
            return new LogNotificacionViewModel
            {
                Id = entity.Id,
                SolicitudId = entity.SolicitudId ?? 0,
                TipoNotificacion = entity.TipoNotificacion,
                FechaEnvio = entity.FechaEnvio,
                MetodoEnvio = entity.MetodoEnvio,
                Destinatario = entity.Destinatario,
                Estado = entity.Estado,
                Error = entity.Error
            };
        }

        public async Task<HistorialCompletoViewModel> ObtenerHistorialCompleto(int pacienteId)
        {
            try
            {
                // Obtener todas las solicitudes del paciente
                var solicitudes = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Include(s => s.EstadoSolicitud)
                    .Where(s => s.PacienteId == pacienteId)
                    .OrderByDescending(s => s.FechaSolicitud)
                    .ToListAsync();

                // Obtener IDs de solicitudes
                var solicitudIds = solicitudes.Select(s => s.Id).ToList();

                // Obtener renovaciones
                var renovaciones = await _context.TbHistorialRenovacion
                    .Where(r => solicitudIds.Contains(r.SolicitudAnteriorId) || 
                               solicitudIds.Contains(r.SolicitudNuevaId))
                    .ToListAsync();

                // Obtener cambios de estado
                var cambiosEstado = await _context.TbSolRegCannabisHistorial
                    .Include(h => h.EstadoSolicitudIdHistorialNavigation)
                    .Where(h => h.SolRegCannabisId.HasValue && 
                               solicitudIds.Contains(h.SolRegCannabisId.Value))
                    .ToListAsync();

                // Obtener notificaciones - CORREGIDO
                var notificaciones = await _context.TbLogNotificaciones
                    .Where(n => n.SolicitudId.HasValue && 
                                solicitudIds.Contains(n.SolicitudId.Value))
                    .ToListAsync();

                // Construir línea de tiempo
                var eventos = new List<EventoHistorialViewModel>();

                foreach (var solicitud in solicitudes)
                {
                    eventos.Add(new EventoHistorialViewModel
                    {
                        Fecha = solicitud.FechaSolicitud ?? DateTime.Now,
                        Tipo = "Solicitud",
                        Titulo = solicitud.EsRenovacion == true ? "Renovación" : "Solicitud Inicial",
                        Descripcion = $"Solicitud {solicitud.NumSolCompleta} - {solicitud.EstadoSolicitud?.NombreEstado}",
                        Icono = "file-text",
                        Color = "primary",
                        Usuario = solicitud.CreadaPor
                    });

                    if (solicitud.FechaAprobacion.HasValue)
                    {
                        eventos.Add(new EventoHistorialViewModel
                        {
                            Fecha = solicitud.FechaAprobacion.Value,
                            Tipo = "Aprobación",
                            Titulo = "Carnet Aprobado",
                            Descripcion = $"Carnet {solicitud.NumeroCarnet} aprobado",
                            Icono = "check-circle",
                            Color = "success",
                            Usuario = solicitud.UsuarioRevisor
                        });
                    }
                }

                // Agregar renovaciones a la línea de tiempo
                foreach (var renovacion in renovaciones)
                {
                    eventos.Add(new EventoHistorialViewModel
                    {
                        Fecha = renovacion.FechaRenovacion,
                        Tipo = "Renovación",
                        Titulo = "Renovación de Carnet",
                        Descripcion = renovacion.RazonRenovacion ?? "Renovación",
                        Icono = "refresh-cw",
                        Color = "info",
                        Usuario = renovacion.UsuarioRenovador
                    });
                }

                // Agregar cambios de estado
                foreach (var cambio in cambiosEstado)
                {
                    eventos.Add(new EventoHistorialViewModel
                    {
                        Fecha = cambio.FechaCambio.HasValue ? 
                            cambio.FechaCambio.Value.ToDateTime(TimeOnly.MinValue) : 
                            DateTime.Now,
                        Tipo = "Cambio Estado",
                        Titulo = $"Estado: {cambio.EstadoSolicitudIdHistorialNavigation?.NombreEstado}",
                        Descripcion = cambio.Comentario ?? "",
                        Icono = "tag",
                        Color = "warning",
                        Usuario = cambio.UsuarioRevisor
                    });
                }

                // Ordenar por fecha
                eventos = eventos.OrderByDescending(e => e.Fecha).ToList();

                // Mapear a ViewModels
                var solicitudesVm = solicitudes.Select(MapToSolicitudViewModel).ToList();
                var renovacionesVm = renovaciones.Select(MapToRenovacionViewModel).ToList();
                var cambiosEstadoVm = cambiosEstado.Select(MapToSolicitudHistorialViewModel).ToList();
                var notificacionesVm = notificaciones.Select(MapToLogNotificacionViewModel).ToList();

                return new HistorialCompletoViewModel
                {
                    PacienteId = pacienteId,
                    Solicitudes = solicitudesVm,
                    Renovaciones = renovacionesVm,
                    CambiosEstado = cambiosEstadoVm,
                    Notificaciones = notificacionesVm,
                    LineaTiempo = eventos,
                    TotalSolicitudes = solicitudes.Count,
                    TotalRenovaciones = renovaciones.Count,
                    CarnetActual = solicitudesVm.FirstOrDefault(s => s.CarnetActivo)
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo historial completo para paciente {PacienteId}", pacienteId);
                throw;
            }
        }

        public async Task<HistorialCompletoViewModel> ObtenerHistorialCompletoPorCarnet(string numeroCarnet)
        {
            try
            {
                var solicitud = await _context.TbSolRegCannabis
                    .FirstOrDefaultAsync(s => s.NumeroCarnet == numeroCarnet);

                if (solicitud == null || !solicitud.PacienteId.HasValue)
                    return null;

                return await ObtenerHistorialCompleto(solicitud.PacienteId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo historial por carnet {NumeroCarnet}", numeroCarnet);
                throw;
            }
        }

        public async Task<List<EventoHistorialViewModel>> ObtenerLineaTiempo(int pacienteId)
        {
            try
            {
                var historial = await ObtenerHistorialCompleto(pacienteId);
                return historial?.LineaTiempo ?? new List<EventoHistorialViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo línea de tiempo para paciente {PacienteId}", pacienteId);
                return new List<EventoHistorialViewModel>();
            }
        }

        public async Task<List<SolicitudHistorialViewModel>> ObtenerCambiosEstado(int solicitudId)
        {
            try
            {
                var cambios = await _context.TbSolRegCannabisHistorial
                    .Include(h => h.EstadoSolicitudIdHistorialNavigation)
                    .Where(h => h.SolRegCannabisId == solicitudId)
                    .OrderByDescending(h => h.FechaCambio)
                    .ToListAsync();

                return cambios.Select(MapToSolicitudHistorialViewModel).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cambios de estado para solicitud {SolicitudId}", solicitudId);
                return new List<SolicitudHistorialViewModel>();
            }
        }

        public async Task<List<HistorialRenovacionViewModel>> ObtenerRenovaciones(int pacienteId)
        {
            try
            {
                // Obtener solicitudes del paciente
                var solicitudIds = await _context.TbSolRegCannabis
                    .Where(s => s.PacienteId == pacienteId)
                    .Select(s => s.Id)
                    .ToListAsync();

                var renovaciones = await _context.TbHistorialRenovacion
                    .Include(r => r.SolicitudAnterior)
                    .Include(r => r.SolicitudNueva)
                    .Where(r => solicitudIds.Contains(r.SolicitudAnteriorId) || 
                               solicitudIds.Contains(r.SolicitudNuevaId))
                    .OrderByDescending(r => r.FechaRenovacion)
                    .ToListAsync();

                return renovaciones.Select(MapToRenovacionViewModel).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo renovaciones para paciente {PacienteId}", pacienteId);
                return new List<HistorialRenovacionViewModel>();
            }
        }

        public async Task<ReporteHistorialViewModel> GenerarReporteHistorial(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var reporte = new ReporteHistorialViewModel
                {
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    FechaGeneracion = DateTime.Now
                };

                // Obtener eventos en el período
                var solicitudes = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Where(s => s.FechaSolicitud >= fechaInicio && s.FechaSolicitud <= fechaFin)
                    .ToListAsync();

                var renovaciones = await _context.TbHistorialRenovacion
                    .Include(r => r.SolicitudAnterior)
                    .ThenInclude(s => s.Paciente)
                    .Include(r => r.SolicitudNueva)
                    .ThenInclude(s => s.Paciente)
                    .Where(r => r.FechaRenovacion >= fechaInicio && r.FechaRenovacion <= fechaFin)
                    .ToListAsync();

                var cambiosEstado = await _context.TbSolRegCannabisHistorial
                    .Include(h => h.SolRegCannabis)
                    .ThenInclude(s => s.Paciente)
                    .Where(h => h.FechaCambio.HasValue && 
                               h.FechaCambio.Value.ToDateTime(TimeOnly.MinValue) >= fechaInicio && 
                               h.FechaCambio.Value.ToDateTime(TimeOnly.MinValue) <= fechaFin)
                    .ToListAsync();

                // Estadísticas
                reporte.TotalEventos = solicitudes.Count + renovaciones.Count + cambiosEstado.Count;
                reporte.TotalPacientes = solicitudes.Select(s => s.PacienteId).Distinct().Count();
                reporte.TotalSolicitudes = solicitudes.Count;
                reporte.TotalRenovaciones = renovaciones.Count;

                // Eventos combinados
                var eventos = new List<EventoHistorialViewModel>();

                foreach (var solicitud in solicitudes)
                {
                    eventos.Add(new EventoHistorialViewModel
                    {
                        Fecha = solicitud.FechaSolicitud ?? DateTime.Now,
                        Tipo = "Solicitud",
                        Titulo = solicitud.EsRenovacion == true ? "Renovación" : "Solicitud Inicial",
                        Descripcion = $"Paciente: {solicitud.Paciente?.PrimerNombre} {solicitud.Paciente?.PrimerApellido} - {solicitud.NumSolCompleta}",
                        Icono = "file-text",
                        Color = "primary",
                        Usuario = solicitud.CreadaPor,
                        Metadata = new Dictionary<string, string>
                        {
                            { "PacienteId", solicitud.PacienteId?.ToString() ?? "" },
                            { "SolicitudId", solicitud.Id.ToString() }
                        }
                    });
                }

                // Agregar al reporte
                reporte.Eventos = eventos.OrderByDescending(e => e.Fecha).ToList();

                // Resumen por paciente
                var pacientesResumen = solicitudes
                    .GroupBy(s => s.PacienteId)
                    .Select(g => new ResumenPacienteViewModel
                    {
                        PacienteId = g.Key ?? 0,
                        NombreCompleto = g.First().Paciente != null ? 
                            $"{g.First().Paciente.PrimerNombre} {g.First().Paciente.PrimerApellido}" : "Desconocido",
                        DocumentoIdentidad = g.First().Paciente?.DocumentoCedula ?? g.First().Paciente?.DocumentoPasaporte ?? "",
                        FechaPrimeraSolicitud = g.Min(s => s.FechaSolicitud) ?? DateTime.Now,
                        TotalSolicitudes = g.Count(),
                        TotalRenovaciones = g.Count(s => s.EsRenovacion == true),
                        CarnetActual = g.OrderByDescending(s => s.FechaSolicitud).FirstOrDefault()?.NumeroCarnet,
                        FechaVencimiento = g.OrderByDescending(s => s.FechaSolicitud).FirstOrDefault()?.FechaVencimientoCarnet,
                        Estado = g.OrderByDescending(s => s.FechaSolicitud).FirstOrDefault()?.EstadoSolicitud?.NombreEstado ?? ""
                    })
                    .ToList();

                reporte.ResumenPacientes = pacientesResumen;

                return reporte;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de historial");
                return new ReporteHistorialViewModel();
            }
        }

        // Métodos de mapeo privados
        private SolicitudCannabisViewModel MapToSolicitudViewModel(TbSolRegCannabis entity)
        {
            return new SolicitudCannabisViewModel
            {
                Id = entity.Id,
                FechaSolicitud = entity.FechaSolicitud ?? DateTime.Now,
                PacienteId = entity.PacienteId,
                FechaRevision = entity.FechaRevision.HasValue ? 
                    entity.FechaRevision.Value.ToDateTime(TimeOnly.MinValue) : 
                    (DateTime?)null,
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

        private HistorialRenovacionViewModel MapToRenovacionViewModel(TbHistorialRenovacion entity)
        {
            return new HistorialRenovacionViewModel
            {
                Id = entity.Id,
                SolicitudAnteriorId = entity.SolicitudAnteriorId,
                SolicitudNuevaId = entity.SolicitudNuevaId,
                FechaRenovacion = entity.FechaRenovacion,
                RazonRenovacion = entity.RazonRenovacion,
                UsuarioRenovador = entity.UsuarioRenovador,
                Comentarios = entity.Comentarios
            };
        }

        private SolicitudHistorialViewModel MapToSolicitudHistorialViewModel(TbSolRegCannabisHistorial entity)
        {
            return new SolicitudHistorialViewModel
            {
                Id = entity.Id,
                SolRegCannabisId = entity.SolRegCannabisId,
                Comentario = entity.Comentario,
                UsuarioRevisor = entity.UsuarioRevisor,
                FechaCambio = entity.FechaCambio.HasValue ? 
                    entity.FechaCambio.Value.ToDateTime(TimeOnly.MinValue) : 
                    (DateTime?)null,
                EstadoSolicitudIdHistorial = entity.EstadoSolicitudIdHistorial ?? 0,
                EstadoSolicitud = entity.EstadoSolicitudIdHistorialNavigation != null ? 
                    new EstadoSolicitudViewModel
                    {
                        IdEstado = entity.EstadoSolicitudIdHistorialNavigation.IdEstado,
                        NombreEstado = entity.EstadoSolicitudIdHistorialNavigation.NombreEstado
                    } : null
            };
        }

        private async Task<int> ObtenerEstadoId(string nombreEstado)
        {
            var estado = await _context.TbEstadoSolicitud
                .FirstOrDefaultAsync(e => e.NombreEstado == nombreEstado);
            return estado?.IdEstado ?? 1;
        }
        
        public async Task RegistrarEvento(string tipo, string descripcion, string usuario, string entidadId)
        {
            try
            {
                var evento = new TbHistorialUsuario
                {
                    TipoCambio = tipo,
                    Comentario = $"{descripcion} (Entidad: {entidadId})",
                    UsuarioId = usuario,
                    CambioPor = usuario,
                    FechaCambio = DateTime.Now
                };

                await _context.TbHistorialUsuario.AddAsync(evento);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando evento {Tipo}", tipo);
            }
        }

        public async Task RegistrarError(string origen, string mensaje, string usuario)
        {
            try
            {
                var error = new TbHistorialUsuario
                {
                    TipoCambio = "ERROR",
                    Comentario = $"{origen}: {mensaje}",
                    UsuarioId = usuario,
                    CambioPor = "Sistema",
                    FechaCambio = DateTime.Now
                };

                await _context.TbHistorialUsuario.AddAsync(error);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando error desde {Origen}", origen);
            }
        }
    }
}