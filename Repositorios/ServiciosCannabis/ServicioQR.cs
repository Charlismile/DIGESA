using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.CodigoQr;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioQr : IServicioQr
    {
        private readonly DbContextDigesa _context;
        private readonly ILogger<ServicioQr> _logger;
        private readonly IServicioNotificaciones _notificaciones;
        private readonly IServicioHistorial _historial;
        private readonly IConfiguration _configuration;

        public ServicioQr(DbContextDigesa context, 
                         ILogger<ServicioQr> logger,
                         IServicioNotificaciones notificaciones,
                         IServicioHistorial historial,
                         IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _notificaciones = notificaciones;
            _historial = historial;
            _configuration = configuration;
        }

        public async Task<CodigoQrViewModel> GenerarCodigoQR(int solicitudId, string usuarioId)
        {
            try
            {
                // Obtener solicitud
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .Include(s => s.EstadoSolicitud)
                    .FirstOrDefaultAsync(s => s.Id == solicitudId);

                if (solicitud == null)
                    throw new KeyNotFoundException($"Solicitud con ID {solicitudId} no encontrada");

                // Obtener estado "Aprobado"
                var estadoAprobado = await _context.TbEstadoSolicitud
                    .FirstOrDefaultAsync(e => e.NombreEstado == "Aprobado");
                    
                if (estadoAprobado == null)
                    throw new InvalidOperationException("No se encontró el estado 'Aprobado' en el sistema");

                // Verificar que la solicitud esté aprobada
                if (solicitud.EstadoSolicitudId != estadoAprobado.IdEstado)
                    throw new InvalidOperationException($"La solicitud debe estar aprobada. Estado actual: {solicitud.EstadoSolicitud?.NombreEstado}");

                // Verificar que no exista un QR activo para esta solicitud
                var qrExistente = await _context.TbCodigoQr
                    .FirstOrDefaultAsync(q => q.SolicitudId == solicitudId && q.Activo);

                if (qrExistente != null)
                {
                    _logger.LogWarning("Ya existe un código QR activo para la solicitud {SolicitudId}", solicitudId);
                    return await MapToViewModel(qrExistente);
                }

                // Generar código único
                var codigoQR = await GenerarCodigoQRUnico();

                // Calcular fechas de vigencia
                var fechaGeneracion = DateTime.Now;
                var fechaVencimiento = solicitud.FechaVencimientoCarnet ?? fechaGeneracion.AddDays(730); // Usar fecha del carnet o 2 años

                var entidad = new TbCodigoQr
                {
                    SolicitudId = solicitudId,
                    CodigoQr = codigoQR, // Nota: la propiedad se llama CodigoQr (no CodigoQR)
                    FechaGeneracion = fechaGeneracion,
                    FechaVencimiento = fechaVencimiento,
                    Activo = true,
                    VecesEscaneado = 0,
                    UltimoEscaneo = null,
                    UltimoEscaneadoPor = null,
                    Comentarios = $"QR generado por {usuarioId}"
                };

                await _context.TbCodigoQr.AddAsync(entidad);
                await _context.SaveChangesAsync();

                // Registrar en historial usando método existente
                await RegistrarEnHistorial(entidad.Id, "QR_GENERADO", 
                    $"Código QR generado para solicitud {solicitudId}", 
                    usuarioId);

                // Notificar al paciente
                if (solicitud.Paciente != null && !string.IsNullOrEmpty(solicitud.Paciente.CorreoElectronico))
                {
                    await EnviarNotificacionQRGenerado(solicitud, entidad, usuarioId);
                }

                return await MapToViewModel(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando código QR para solicitud {SolicitudId}", solicitudId);
                await RegistrarErrorEnHistorial("ServicioQR.GenerarCodigoQR", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<bool> ValidarCodigoQR(string codigoQR, string ipOrigen = null)
        {
            try
            {
                var resultado = await EscanearCodigoQR(codigoQR, "SISTEMA_VALIDACION", ipOrigen);
                return resultado.EsValido && resultado.Estado == "ACTIVO";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando código QR {CodigoQR}", codigoQR);
                return false;
            }
        }

        public async Task<CodigoQrViewModel> EscanearCodigoQR(string codigoQR, string escaneadoPor, string ipOrigen = null)
        {
            try
            {
                var entidad = await _context.TbCodigoQr
                    .Include(q => q.Solicitud)
                    .ThenInclude(s => s.Paciente)
                    .FirstOrDefaultAsync(q => q.CodigoQr == codigoQR); // Cambiado a CodigoQr

                if (entidad == null)
                {
                    _logger.LogWarning("Intento de escaneo de código QR no encontrado: {CodigoQR}", codigoQR);
                    return new CodigoQrViewModel
                    {
                        CodigoQR = codigoQR,
                        EsValido = false,
                        Estado = "NO_ENCONTRADO",
                        Mensaje = "Código QR no registrado en el sistema"
                    };
                }

                // Registrar escaneo
                entidad.UltimoEscaneo = DateTime.Now;
                entidad.VecesEscaneado++;
                entidad.UltimoEscaneadoPor = escaneadoPor;

                await _context.SaveChangesAsync();

                // Validar estado y vigencia
                var esValido = entidad.Activo;
                var mensaje = "Código QR válido";
                var estado = "ACTIVO";

                if (!esValido)
                {
                    mensaje = "Código QR inactivo";
                    estado = "INACTIVO";
                }
                else if (entidad.FechaVencimiento.HasValue && entidad.FechaVencimiento < DateTime.Now)
                {
                    // Verificar período de gracia
                    var diasVencido = (DateTime.Now - entidad.FechaVencimiento.Value).Days;
                    var diasGracia = _configuration.GetValue<int>("AppSettings:DiasGraciaQR", 90);

                    if (diasVencido > diasGracia)
                    {
                        esValido = false;
                        estado = "VENCIDO";
                        mensaje = "Código QR vencido (fuera de período de gracia)";
                        
                        // Desactivar automáticamente si está fuera del período de gracia
                        entidad.Activo = false;
                        entidad.Comentarios = $"Desactivado automáticamente por vencimiento. Vencido hace {diasVencido} días.";
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        mensaje = $"Código QR en período de gracia. Vencido hace {diasVencido} días";
                        estado = "EN_GRACIA";
                    }
                }

                var viewModel = await MapToViewModel(entidad);
                viewModel.EsValido = esValido;
                viewModel.Mensaje = mensaje;
                viewModel.Estado = estado;
                viewModel.UltimoEscaneo = DateTime.Now;

                // Registrar en historial
                await RegistrarEnHistorial(entidad.Id, "QR_ESCANEADO", 
                    $"Código QR escaneado por {escaneadoPor}. Resultado: {(esValido ? "VÁLIDO" : "INVÁLIDO")}", 
                    escaneadoPor);

                return viewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error escaneando código QR {CodigoQR}", codigoQR);
                await RegistrarErrorEnHistorial("ServicioQR.EscanearCodigoQR", ex.Message, escaneadoPor);
                
                return new CodigoQrViewModel
                {
                    CodigoQR = codigoQR,
                    EsValido = false,
                    Estado = "ERROR_VALIDACION",
                    Mensaje = "Error en la validación del código QR"
                };
            }
        }

        public async Task<bool> InactivarCodigoQR(int codigoQRId, string usuarioId, string motivo)
        {
            try
            {
                var entidad = await _context.TbCodigoQr.FindAsync(codigoQRId);
                if (entidad == null)
                    throw new KeyNotFoundException($"Código QR con ID {codigoQRId} no encontrado");

                entidad.Activo = false;
                entidad.Comentarios = $"Inactivado por {usuarioId}. Motivo: {motivo}";

                await _context.SaveChangesAsync();

                // Registrar en historial
                await RegistrarEnHistorial(entidad.Id, "QR_INACTIVADO", 
                    $"Código QR inactivado. Motivo: {motivo}", 
                    usuarioId);

                // Notificar al paciente si es necesario
                var solicitud = await _context.TbSolRegCannabis
                    .Include(s => s.Paciente)
                    .FirstOrDefaultAsync(s => s.Id == entidad.SolicitudId);

                if (solicitud?.Paciente != null && !string.IsNullOrEmpty(solicitud.Paciente.CorreoElectronico))
                {
                    await EnviarNotificacionQRInactivado(solicitud, entidad, usuarioId, motivo);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inactivando código QR {CodigoQRId}", codigoQRId);
                await RegistrarErrorEnHistorial("ServicioQR.InactivarCodigoQR", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<List<CodigoQrViewModel>> ObtenerCodigosQRPorSolicitud(int solicitudId)
        {
            try
            {
                var codigos = await _context.TbCodigoQr
                    .Include(q => q.Solicitud)
                    .ThenInclude(s => s.Paciente)
                    .Where(q => q.SolicitudId == solicitudId)
                    .OrderByDescending(q => q.FechaGeneracion)
                    .ToListAsync();

                var viewModels = new List<CodigoQrViewModel>();
                
                foreach (var codigo in codigos)
                {
                    viewModels.Add(await MapToViewModel(codigo));
                }

                return viewModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo códigos QR para solicitud {SolicitudId}", solicitudId);
                throw;
            }
        }

        public async Task<CodigoQrViewModel> ObtenerCodigoQRActivo(int solicitudId)
        {
            try
            {
                var codigo = await _context.TbCodigoQr
                    .Include(q => q.Solicitud)
                    .ThenInclude(s => s.Paciente)
                    .Include(q => q.Solicitud)
                    .ThenInclude(s => s.EstadoSolicitud)
                    .FirstOrDefaultAsync(q => q.SolicitudId == solicitudId && q.Activo);

                if (codigo == null)
                    return null;

                return await MapToViewModel(codigo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo código QR activo para solicitud {SolicitudId}", solicitudId);
                throw;
            }
        }

        #region Métodos Auxiliares

        private async Task<string> GenerarCodigoQRUnico()
        {
            string codigo;
            bool existe;
            int intentos = 0;
            
            do
            {
                // Formato: CAN-PAN-YYYYMM-XXXXX donde X es alfanumérico
                var timestamp = DateTime.Now.ToString("yyyyMM");
                var random = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                codigo = $"CAN-PAN-{timestamp}-{random}";
                
                existe = await _context.TbCodigoQr.AnyAsync(q => q.CodigoQr == codigo); // Cambiado a CodigoQr
                intentos++;
            }
            while (existe && intentos < 10);

            if (existe)
                throw new InvalidOperationException("No se pudo generar un código QR único");

            return codigo;
        }

        private async Task<CodigoQrViewModel> MapToViewModel(TbCodigoQr entidad)
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == entidad.SolicitudId);

            var diasVencido = entidad.FechaVencimiento.HasValue && entidad.FechaVencimiento < DateTime.Now ? 
                (DateTime.Now - entidad.FechaVencimiento.Value).Days : 0;

            var diasGracia = _configuration.GetValue<int>("AppSettings:DiasGraciaQR", 90);
            var enPeriodoGracia = diasVencido > 0 && diasVencido <= diasGracia;
            var diasRestantes = entidad.FechaVencimiento.HasValue && entidad.FechaVencimiento >= DateTime.Now ? 
                (entidad.FechaVencimiento.Value - DateTime.Now).Days : -diasVencido;

            var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "https://digesa.gob.pa";
            var urlValidacionPublica = $"{baseUrl}/validar-qr/{entidad.CodigoQr}";

            var viewModel = new CodigoQrViewModel
            {
                Id = entidad.Id,
                SolicitudId = entidad.SolicitudId,
                CodigoQR = entidad.CodigoQr, // Mapeo de CodigoQr a CodigoQR
                FechaGeneracion = entidad.FechaGeneracion,
                FechaVencimiento = entidad.FechaVencimiento,
                Activo = entidad.Activo,
                VecesEscaneado = entidad.VecesEscaneado,
                UltimoEscaneo = entidad.UltimoEscaneo,
                UltimoEscaneadoPor = entidad.UltimoEscaneadoPor,
                Comentarios = entidad.Comentarios,
                NumeroCarnet = solicitud?.NumeroCarnet,
                UrlValidacionPublica = urlValidacionPublica
            };

            // Propiedades calculadas (necesitan setters en el ViewModel)
            // Como tu ViewModel tiene propiedades calculadas con getters, vamos a crear métodos alternativos
            
            // En lugar de asignar directamente, calculamos y mostramos en log
            var esValido = entidad.Activo && 
                          (!entidad.FechaVencimiento.HasValue || entidad.FechaVencimiento >= DateTime.Now || enPeriodoGracia);
            
            _logger.LogDebug("QR {CodigoQR} - Válido: {EsValido}, Días restantes: {DiasRestantes}, En gracia: {EnPeriodoGracia}, Días vencido: {DiasVencido}",
                entidad.CodigoQr, esValido, diasRestantes, enPeriodoGracia, diasVencido);

            return viewModel;
        }

        private async Task RegistrarEnHistorial(int qrId, string tipoEvento, string descripcion, string usuario)
        {
            try
            {
                // Usar el método de historial disponible
                // Suponiendo que tienes un método para registrar eventos en el historial
                // Si no existe, puedes crear uno simple aquí
                var historial = new TbSolRegCannabisHistorial
                {
                    SolRegCannabisId = await ObtenerSolicitudIdDeQR(qrId),
                    EstadoSolicitudIdHistorial = await ObtenerEstadoId("Registro"),
                    Comentario = $"{tipoEvento}: {descripcion}",
                    UsuarioRevisor = usuario,
                    FechaCambio = DateOnly.FromDateTime(DateTime.Now)
                };

                await _context.TbSolRegCannabisHistorial.AddAsync(historial);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error registrando en historial: {TipoEvento} - {Descripcion}", tipoEvento, descripcion);
            }
        }

        private async Task RegistrarErrorEnHistorial(string metodo, string error, string usuario)
        {
            try
            {
                var historial = new TbHistorialUsuario
                {
                    UsuarioId = usuario,
                    TipoCambio = "ERROR",
                    Comentario = $"{metodo}: {error}",
                    CambioPor = "Sistema",
                    FechaCambio = DateTime.Now
                };

                await _context.TbHistorialUsuario.AddAsync(historial);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando error en historial: {Metodo}", metodo);
            }
        }

        private async Task<int> ObtenerSolicitudIdDeQR(int qrId)
        {
            var qr = await _context.TbCodigoQr
                .FirstOrDefaultAsync(q => q.Id == qrId);
            return qr?.SolicitudId ?? 0;
        }

        private async Task<int> ObtenerEstadoId(string nombreEstado)
        {
            var estado = await _context.TbEstadoSolicitud
                .FirstOrDefaultAsync(e => e.NombreEstado == nombreEstado);
            return estado?.IdEstado ?? 1;
        }

        private async Task EnviarNotificacionQRGenerado(TbSolRegCannabis solicitud, TbCodigoQr qr, string usuarioId)
        {
            try
            {
                var baseUrl = _configuration["AppSettings:BaseUrl"] ?? "https://digesa.gob.pa";
                
                // Crear una notificación en la tabla de log
                var notificacion = new TbLogNotificaciones
                {
                    SolicitudId = solicitud.Id,
                    TipoNotificacion = "QR_GENERADO",
                    Destinatario = solicitud.Paciente?.CorreoElectronico,
                    MetodoEnvio = "Email",
                    Estado = "PENDIENTE",
                    FechaEnvio = DateTime.Now
                };

                await _context.TbLogNotificaciones.AddAsync(notificacion);
                await _context.SaveChangesAsync();

                // También puedes llamar al servicio de notificaciones si existe
                // await _notificaciones.EnviarNotificacionQRGenerado(...);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error enviando notificación de QR generado");
            }
        }

        private async Task EnviarNotificacionQRInactivado(TbSolRegCannabis solicitud, TbCodigoQr qr, string usuarioId, string motivo)
        {
            try
            {
                var notificacion = new TbLogNotificaciones
                {
                    SolicitudId = solicitud.Id,
                    TipoNotificacion = "QR_INACTIVADO",
                    Destinatario = solicitud.Paciente?.CorreoElectronico,
                    MetodoEnvio = "Email",
                    Estado = "PENDIENTE",
                    FechaEnvio = DateTime.Now,
                    Error = motivo
                };

                await _context.TbLogNotificaciones.AddAsync(notificacion);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error enviando notificación de QR inactivado");
            }
        }

        #endregion
    }
}