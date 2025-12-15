using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioFarmacias : IServicioFarmacias
    {
        private readonly DbContextDigesa _context;
        private readonly ILogger<ServicioFarmacias> _logger;
        private readonly IServicioNotificaciones _notificaciones;
        private readonly IServicioHistorial _historial;
        private readonly IServicioQr _servicioQR;

        public ServicioFarmacias(DbContextDigesa context, 
                               ILogger<ServicioFarmacias> logger,
                               IServicioNotificaciones notificaciones,
                               IServicioHistorial historial,
                               IServicioQr servicioQR)
        {
            _context = context;
            _logger = logger;
            _notificaciones = notificaciones;
            _historial = historial;
            _servicioQR = servicioQR;
        }

        public async Task<FarmaciaAutorizadaViewModel> CrearFarmacia(FarmaciaAutorizadaViewModel farmacia, string usuarioId)
        {
            try
            {
                // Validar que no exista farmacia con mismo RUC
                var existeRUC = await _context.TbFarmaciaAutorizada
                    .AnyAsync(f => f.Ruc == farmacia.Ruc);
                
                if (existeRUC)
                    throw new InvalidOperationException($"Ya existe una farmacia registrada con RUC: {farmacia.Ruc}");

                // Generar código único de farmacia
                var codigoFarmacia = await GenerarCodigoFarmaciaUnico();

                var entidad = new TbFarmaciaAutorizada
                {
                    CodigoFarmacia = codigoFarmacia,
                    NombreFarmacia = farmacia.NombreFarmacia,
                    Ruc = farmacia.Ruc,
                    Direccion = farmacia.Direccion,
                    ProvinciaId = farmacia.ProvinciaId,
                    DistritoId = farmacia.DistritoId,
                    Telefono = farmacia.Telefono,
                    Email = farmacia.Email?.ToLower(),
                    Responsable = farmacia.Responsable,
                    FechaAutorizacion = farmacia.FechaAutorizacion ?? DateTime.Now,
                    FechaVencimientoAutorizacion = farmacia.FechaVencimientoAutorizacion ?? DateTime.Now.AddYears(1),
                    Activo = true,
                    UsuarioRegistro = usuarioId,
                    FechaRegistro = DateTime.Now
                };

                await _context.TbFarmaciaAutorizada.AddAsync(entidad);
                await _context.SaveChangesAsync();

                // Registrar en historial
                await _historial.RegistrarEvento("FARMACIA_CREADA", $"Farmacia creada: {entidad.NombreFarmacia}", usuarioId, entidad.Id.ToString());

                // Notificar a los administradores
                await _notificaciones.EnviarNotificacion("NUEVA_FARMACIA_REGISTRADA", usuarioId, new
                {
                    NombreFarmacia = entidad.NombreFarmacia,
                    CodigoFarmacia = codigoFarmacia,
                    RUC = entidad.Ruc,
                    Direccion = entidad.Direccion,
                    Responsable = entidad.Responsable
                });

                // Mapear de vuelta al ViewModel
                farmacia.Id = entidad.Id;
                farmacia.CodigoFarmacia = codigoFarmacia;
                farmacia.FechaRegistro = entidad.FechaRegistro;
                farmacia.Activo = entidad.Activo;

                return farmacia;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando farmacia");
                await _historial.RegistrarError("ServicioFarmacias.CrearFarmacia", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<bool> ActualizarFarmacia(int farmaciaId, FarmaciaAutorizadaViewModel farmacia, string usuarioId)
        {
            try
            {
                var entidad = await _context.TbFarmaciaAutorizada.FindAsync(farmaciaId);
                if (entidad == null)
                    throw new KeyNotFoundException($"Farmacia con ID {farmaciaId} no encontrada");

                // Actualizar campos permitidos
                entidad.NombreFarmacia = farmacia.NombreFarmacia;
                entidad.Direccion = farmacia.Direccion;
                entidad.ProvinciaId = farmacia.ProvinciaId;
                entidad.DistritoId = farmacia.DistritoId;
                entidad.Telefono = farmacia.Telefono;
                entidad.Email = farmacia.Email?.ToLower();
                entidad.Responsable = farmacia.Responsable;
                entidad.FechaAutorizacion = farmacia.FechaAutorizacion ?? entidad.FechaAutorizacion;
                entidad.FechaVencimientoAutorizacion = farmacia.FechaVencimientoAutorizacion ?? entidad.FechaVencimientoAutorizacion;
                entidad.Activo = farmacia.Activo;

                await _context.SaveChangesAsync();

                // Registrar en historial
                await _historial.RegistrarEvento("FARMACIA_ACTUALIZADA", $"Farmacia actualizada: {entidad.NombreFarmacia}", usuarioId, entidad.Id.ToString());

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando farmacia {FarmaciaId}", farmaciaId);
                await _historial.RegistrarError("ServicioFarmacias.ActualizarFarmacia", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<bool> InactivarFarmacia(int farmaciaId, string usuarioId, string motivo)
        {
            try
            {
                var entidad = await _context.TbFarmaciaAutorizada.FindAsync(farmaciaId);
                if (entidad == null)
                    throw new KeyNotFoundException($"Farmacia con ID {farmaciaId} no encontrada");

                entidad.Activo = false;

                await _context.SaveChangesAsync();

                // Registrar en historial
                await _historial.RegistrarEvento("FARMACIA_INACTIVADA", $"Farmacia inactivada. Motivo: {motivo}", usuarioId, entidad.Id.ToString());

                // Notificar a la farmacia
                if (!string.IsNullOrEmpty(entidad.Email))
                {
                    await _notificaciones.EnviarNotificacion("FARMACIA_INACTIVADA", usuarioId, new
                    {
                        NombreFarmacia = entidad.NombreFarmacia,
                        CodigoFarmacia = entidad.CodigoFarmacia,
                        Motivo = motivo,
                        FechaInactivacion = DateTime.Now.ToString("dd/MM/yyyy")
                    });
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inactivando farmacia {FarmaciaId}", farmaciaId);
                await _historial.RegistrarError("ServicioFarmacias.InactivarFarmacia", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<bool> ValidarCarnetEnFarmacia(int farmaciaId, string codigoQR)
        {
            try
            {
                // Verificar que la farmacia esté activa y autorizada
                var farmacia = await _context.TbFarmaciaAutorizada
                    .FirstOrDefaultAsync(f => f.Id == farmaciaId && 
                                             f.Activo &&
                                             f.FechaVencimientoAutorizacion >= DateTime.Now);

                if (farmacia == null)
                    throw new UnauthorizedAccessException("Farmacia no autorizada o autorización vencida");

                // Validar el código QR
                var resultadoValidacion = await _servicioQR.EscanearCodigoQR(codigoQR, $"FARMACIA_{farmacia.CodigoFarmacia}");

                if (!resultadoValidacion.EsValido)
                    return false;

                // Registrar el escaneo en el historial
                await _historial.RegistrarEvento("VALIDACION_FARMACIA", 
                    $"Carnet validado en farmacia {farmacia.NombreFarmacia}. Resultado: {(resultadoValidacion.EsValido ? "VÁLIDO" : "INVÁLIDO")}", 
                    $"FARMACIA_{farmacia.CodigoFarmacia}", 
                    resultadoValidacion.Id.ToString());

                return resultadoValidacion.EsValido;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando carnet en farmacia {FarmaciaId}", farmaciaId);
                return false;
            }
        }

        public async Task<RegistroDispensacionViewModel> RegistrarDispensacion(int farmaciaId, string codigoQR, RegistroDispensacionViewModel dispensacion, string usuarioId)
        {
            try
            {
                // Verificar que la farmacia esté activa
                var farmacia = await _context.TbFarmaciaAutorizada
                    .FirstOrDefaultAsync(f => f.Id == farmaciaId && f.Activo);

                if (farmacia == null)
                    throw new UnauthorizedAccessException("Farmacia no autorizada para dispensar");

                // Validar el código QR
                var resultadoQR = await _servicioQR.EscanearCodigoQR(codigoQR, $"FARMACIA_{farmacia.CodigoFarmacia}");
                
                if (!resultadoQR.EsValido)
                    throw new InvalidOperationException($"Código QR no válido: {resultadoQR.Mensaje}");

                // Obtener la solicitud desde el código QR
                var codigoQREntidad = await _context.TbCodigoQR
                    .Include(q => q.Solicitud)
                    .ThenInclude(s => s.Paciente)
                    .FirstOrDefaultAsync(q => q.CodigoQR == codigoQR);

                if (codigoQREntidad == null)
                    throw new KeyNotFoundException("Código QR no encontrado");

                var solicitud = codigoQREntidad.Solicitud;
                var paciente = solicitud?.Paciente;

                if (paciente == null)
                    throw new InvalidOperationException("No se encontró información del paciente");

                // Crear registro de dispensación
                var entidad = new TbRegistroDispensacion
                {
                    SolicitudId = codigoQREntidad.SolicitudId,
                    FarmaciaId = farmaciaId,
                    FechaDispensacion = DateTime.Now,
                    Producto = dispensacion.Producto,
                    Cantidad = dispensacion.Cantidad,
                    UnidadMedida = dispensacion.UnidadMedida,
                    LoteProducto = dispensacion.LoteProducto,
                    FechaVencimientoProducto = dispensacion.FechaVencimientoProducto,
                    FarmaceuticoResponsable = dispensacion.FarmaceuticoResponsable,
                    NumeroFactura = dispensacion.NumeroFactura,
                    Comentarios = dispensacion.Comentarios,
                    UsuarioRegistro = usuarioId,
                    FechaRegistro = DateTime.Now
                };

                await _context.TbRegistroDispensacion.AddAsync(entidad);
                await _context.SaveChangesAsync();

                // Registrar en historial
                await _historial.RegistrarEvento("DISPENSACION_REGISTRADA", 
                    $"Dispensación registrada para paciente {paciente.PrimerNombre} {paciente.PrimerApellido} en farmacia {farmacia.NombreFarmacia}", 
                    usuarioId, 
                    entidad.Id.ToString());

                // Notificar al paciente (opcional)
                if (!string.IsNullOrEmpty(paciente.CorreoElectronico))
                {
                    await _notificaciones.EnviarNotificacion("DISPENSACION_REGISTRADA", usuarioId, new
                    {
                        NombrePaciente = $"{paciente.PrimerNombre} {paciente.PrimerApellido}",
                        NombreFarmacia = farmacia.NombreFarmacia,
                        Producto = dispensacion.Producto,
                        Cantidad = dispensacion.Cantidad,
                        UnidadMedida = dispensacion.UnidadMedida,
                        FechaDispensacion = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                        Farmaceutico = dispensacion.FarmaceuticoResponsable
                    });
                }

                // Mapear a ViewModel
                return new RegistroDispensacionViewModel
                {
                    Id = entidad.Id,
                    SolicitudId = entidad.SolicitudId,
                    FarmaciaId = entidad.FarmaciaId,
                    FechaDispensacion = entidad.FechaDispensacion,
                    Producto = entidad.Producto,
                    Cantidad = entidad.Cantidad,
                    UnidadMedida = entidad.UnidadMedida,
                    LoteProducto = entidad.LoteProducto,
                    FechaVencimientoProducto = entidad.FechaVencimientoProducto,
                    FarmaceuticoResponsable = entidad.FarmaceuticoResponsable,
                    NumeroFactura = entidad.NumeroFactura,
                    Comentarios = entidad.Comentarios,
                    FechaRegistro = entidad.FechaRegistro,
                    UsuarioRegistro = entidad.UsuarioRegistro,

                    // Información adicional
                    PacienteNombre = $"{paciente.PrimerNombre} {paciente.PrimerApellido}",
                    PacienteCedula = paciente.DocumentoCedula,
                    FarmaciaNombre = farmacia.NombreFarmacia,
                    CodigoQR = codigoQR
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando dispensación en farmacia {FarmaciaId}", farmaciaId);
                await _historial.RegistrarError("ServicioFarmacias.RegistrarDispensacion", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<List<FarmaciaAutorizadaViewModel>> ObtenerFarmaciasPorProvincia(int provinciaId)
        {
            try
            {
                var farmacias = await _context.TbFarmaciaAutorizada
                    .Include(f => f.Provincia)
                    .Include(f => f.Distrito)
                    .Where(f => f.ProvinciaId == provinciaId && f.Activo)
                    .OrderBy(f => f.NombreFarmacia)
                    .ToListAsync();

                return farmacias.Select(MapToViewModel).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo farmacias por provincia {ProvinciaId}", provinciaId);
                throw;
            }
        }

        public async Task<List<FarmaciaAutorizadaViewModel>> ObtenerFarmaciasActivas()
        {
            try
            {
                var farmacias = await _context.TbFarmaciaAutorizada
                    .Include(f => f.Provincia)
                    .Include(f => f.Distrito)
                    .Where(f => f.Activo && f.FechaVencimientoAutorizacion >= DateTime.Now)
                    .OrderBy(f => f.NombreFarmacia)
                    .ToListAsync();

                return farmacias.Select(MapToViewModel).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo farmacias activas");
                throw;
            }
        }

        public async Task<FarmaciaAutorizadaViewModel> ObtenerFarmaciaPorCodigo(string codigoFarmacia)
        {
            try
            {
                var entidad = await _context.TbFarmaciaAutorizada
                    .Include(f => f.Provincia)
                    .Include(f => f.Distrito)
                    .FirstOrDefaultAsync(f => f.CodigoFarmacia == codigoFarmacia && f.Activo);

                if (entidad == null)
                    return null;

                return MapToViewModel(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo farmacia por código {CodigoFarmacia}", codigoFarmacia);
                throw;
            }
        }

        public async Task<ReporteDispensacionViewModel> GenerarReporteDispensacion(int farmaciaId, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                // Verificar fechas
                if (fechaFin < fechaInicio)
                    throw new ArgumentException("La fecha fin debe ser mayor o igual a la fecha inicio");

                var farmacia = await _context.TbFarmaciaAutorizada.FindAsync(farmaciaId);
                if (farmacia == null)
                    throw new KeyNotFoundException($"Farmacia con ID {farmaciaId} no encontrada");

                // Obtener dispensaciones en el período
                var dispensaciones = await _context.TbRegistroDispensacion
                    .Include(d => d.Solicitud)
                    .ThenInclude(s => s.Paciente)
                    .Where(d => d.FarmaciaId == farmaciaId &&
                               d.FechaDispensacion >= fechaInicio &&
                               d.FechaDispensacion <= fechaFin)
                    .ToListAsync();

                // Agrupar por producto
                var productos = dispensaciones
                    .GroupBy(d => d.Producto)
                    .Select(g => new ProductoDispensacionViewModel
                    {
                        Producto = g.Key,
                        CantidadTotal = (decimal)g.Sum(d => d.Cantidad),
                        CantidadDispensaciones = g.Count()
                    })
                    .OrderByDescending(p => p.CantidadTotal)
                    .ToList();

                // Dispensaciones por día
                var dispensacionesPorDia = dispensaciones
                    .GroupBy(d => d.FechaDispensacion.Date)
                    .Select(g => new DispensacionDiariaViewModel
                    {
                        Fecha = g.Key,
                        CantidadDispensaciones = g.Count(),
                        CantidadTotal = (decimal)g.Sum(d => d.Cantidad),
                        PacientesUnicos = g.Select(d => d.Solicitud?.PacienteId).Distinct().Count()
                    })
                    .OrderBy(d => d.Fecha)
                    .ToList();

                // Top pacientes
                var topPacientes = dispensaciones
                    .Where(d => d.Solicitud?.Paciente != null)
                    .GroupBy(d => d.Solicitud.Paciente)
                    .Select(g => new TopPacienteViewModel
                    {
                        PacienteId = g.Key.Id,
                        PacienteNombre = $"{g.Key.PrimerNombre} {g.Key.PrimerApellido}",
                        PacienteCedula = g.Key.DocumentoCedula,
                        CantidadDispensaciones = g.Count(),
                        CantidadTotal = (decimal)g.Sum(d => d.Cantidad)
                    })
                    .OrderByDescending(p => p.CantidadDispensaciones)
                    .Take(10)
                    .ToList();

                var reporte = new ReporteDispensacionViewModel
                {
                    FarmaciaId = farmaciaId,
                    FarmaciaNombre = farmacia.NombreFarmacia,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    FechaGeneracion = DateTime.Now,

                    TotalDispensaciones = dispensaciones.Count,
                    TotalPacientesAtendidos = dispensaciones.Select(d => d.Solicitud?.PacienteId).Distinct().Count(),
                    TotalCantidadVendida = (decimal)dispensaciones.Sum(d => d.Cantidad),

                    Productos = productos,
                    DispensacionesPorDia = dispensacionesPorDia,
                    TopPacientes = topPacientes,

                    DetalleDispensaciones = dispensaciones
                        .Select(d => new DetalleDispensacionViewModel
                        {
                            Id = d.Id,
                            FechaDispensacion = d.FechaDispensacion,
                            PacienteNombre = d.Solicitud?.Paciente != null ? 
                                $"{d.Solicitud.Paciente.PrimerNombre} {d.Solicitud.Paciente.PrimerApellido}" : null,
                            PacienteCedula = d.Solicitud?.Paciente?.DocumentoCedula,
                            Producto = d.Producto,
                            Cantidad = d.Cantidad,
                            UnidadMedida = d.UnidadMedida,
                            LoteProducto = d.LoteProducto,
                            FechaVencimientoProducto = d.FechaVencimientoProducto,
                            FarmaceuticoResponsable = d.FarmaceuticoResponsable,
                            NumeroFactura = d.NumeroFactura,
                            Comentarios = d.Comentarios
                        })
                        .OrderByDescending(d => d.FechaDispensacion)
                        .ToList()
                };

                return reporte;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de dispensación para farmacia {FarmaciaId}", farmaciaId);
                throw;
            }
        }

        #region Métodos Privados

        private async Task<string> GenerarCodigoFarmaciaUnico()
        {
            string codigo;
            bool existe;
            int intentos = 0;
            
            do
            {
                // Formato: FARM-YYYY-XXXX donde X es numérico
                var ano = DateTime.Now.Year;
                var random = new Random().Next(1000, 9999);
                codigo = $"FARM-{ano}-{random}";
                
                existe = await _context.TbFarmaciaAutorizada.AnyAsync(f => f.CodigoFarmacia == codigo);
                intentos++;
            }
            while (existe && intentos < 10);

            if (existe)
                throw new InvalidOperationException("No se pudo generar un código único de farmacia");

            return codigo;
        }

        private FarmaciaAutorizadaViewModel MapToViewModel(TbFarmaciaAutorizada entidad)
        {
            var diasVencimiento = entidad.FechaVencimientoAutorizacion.HasValue ? 
                (entidad.FechaVencimientoAutorizacion.Value - DateTime.Now).Days : 0;

            return new FarmaciaAutorizadaViewModel
            {
                Id = entidad.Id,
                CodigoFarmacia = entidad.CodigoFarmacia,
                NombreFarmacia = entidad.NombreFarmacia,
                Ruc = entidad.Ruc,
                Direccion = entidad.Direccion,
                ProvinciaId = entidad.ProvinciaId,
                DistritoId = entidad.DistritoId,
                Telefono = entidad.Telefono,
                Email = entidad.Email,
                Responsable = entidad.Responsable,
                FechaAutorizacion = entidad.FechaAutorizacion,
                FechaVencimientoAutorizacion = entidad.FechaVencimientoAutorizacion,
                Activo = entidad.Activo,
                UsuarioRegistro = entidad.UsuarioRegistro,
                FechaRegistro = entidad.FechaRegistro,

                // Propiedades calculadas
                DiasVencimientoAutorizacion = diasVencimiento,
                AutorizacionVencida = diasVencimiento < 0,
                AutorizacionPorVencer = diasVencimiento >= 0 && diasVencimiento <= 30,

                // Propiedades de navegación
                ProvinciaNombre = entidad.Provincia?.NombreProvincia,
                DistritoNombre = entidad.Distrito?.NombreDistrito
            };
        }

        #endregion
    }
}