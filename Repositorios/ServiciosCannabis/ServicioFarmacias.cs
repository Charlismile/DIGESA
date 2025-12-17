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
                    .AnyAsync(f => f.Ruc == farmacia.RUC);
                
                if (existeRUC)
                    throw new InvalidOperationException($"Ya existe una farmacia registrada con RUC: {farmacia.RUC}");

                // Generar código único de farmacia
                var codigoFarmacia = await GenerarCodigoFarmaciaUnico();

                var entidad = new TbFarmaciaAutorizada
                {
                    CodigoFarmacia = codigoFarmacia,
                    NombreFarmacia = farmacia.NombreFarmacia,
                    Ruc = farmacia.RUC,
                    Direccion = farmacia.Direccion,
                    ProvinciaId = farmacia.ProvinciaId,
                    DistritoId = farmacia.DistritoId,
                    Telefono = farmacia.Telefono,
                    Email = farmacia.Email?.ToLower(),
                    Responsable = farmacia.Responsable,
                    FechaAutorizacion = farmacia.FechaAutorizacion,
                    FechaVencimientoAutorizacion = farmacia.FechaVencimientoAutorizacion,
                    Activo = true,
                    UsuarioRegistro = usuarioId,
                    FechaRegistro = DateTime.Now
                };

                await _context.TbFarmaciaAutorizada.AddAsync(entidad);
                await _context.SaveChangesAsync();

                // Registrar en historial
                await _historial.RegistrarEvento("FARMACIA_CREADA", 
                    string.Format("Farmacia creada: {0}", entidad.NombreFarmacia), 
                    usuarioId, 
                    entidad.Id.ToString());

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
                    throw new KeyNotFoundException(string.Format("Farmacia con ID {0} no encontrada", farmaciaId));

                // Actualizar campos permitidos
                entidad.NombreFarmacia = farmacia.NombreFarmacia;
                entidad.Direccion = farmacia.Direccion;
                entidad.ProvinciaId = farmacia.ProvinciaId;
                entidad.DistritoId = farmacia.DistritoId;
                entidad.Telefono = farmacia.Telefono;
                entidad.Email = farmacia.Email?.ToLower();
                entidad.Responsable = farmacia.Responsable;
                entidad.FechaAutorizacion = farmacia.FechaAutorizacion;
                entidad.FechaVencimientoAutorizacion = farmacia.FechaVencimientoAutorizacion;
                entidad.Activo = farmacia.Activo;

                await _context.SaveChangesAsync();

                // Registrar en historial
                await _historial.RegistrarEvento("FARMACIA_ACTUALIZADA", 
                    string.Format("Farmacia actualizada: {0}", entidad.NombreFarmacia), 
                    usuarioId, 
                    entidad.Id.ToString());

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando farmacia {0}", farmaciaId);
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
                    throw new KeyNotFoundException(string.Format("Farmacia con ID {0} no encontrada", farmaciaId));

                entidad.Activo = false;

                await _context.SaveChangesAsync();

                // Registrar en historial
                await _historial.RegistrarEvento("FARMACIA_INACTIVADA", 
                    string.Format("Farmacia inactivada. Motivo: {0}", motivo), 
                    usuarioId, 
                    entidad.Id.ToString());

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
                _logger.LogError(ex, "Error inactivando farmacia {0}", farmaciaId);
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
                var resultadoValidacion = await _servicioQR.EscanearCodigoQR(codigoQR, string.Format("FARMACIA_{0}", farmacia.CodigoFarmacia));

                if (!resultadoValidacion.EsValido)
                    return false;

                // Registrar el escaneo en el historial
                await _historial.RegistrarEvento("VALIDACION_FARMACIA", 
                    string.Format("Carnet validado en farmacia {0}. Resultado: {1}", 
                        farmacia.NombreFarmacia, 
                        resultadoValidacion.EsValido ? "VÁLIDO" : "INVÁLIDO"), 
                    string.Format("FARMACIA_{0}", farmacia.CodigoFarmacia), 
                    resultadoValidacion.Id.ToString());

                return resultadoValidacion.EsValido;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando carnet en farmacia {0}", farmaciaId);
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
                var resultadoQR = await _servicioQR.EscanearCodigoQR(codigoQR, string.Format("FARMACIA_{0}", farmacia.CodigoFarmacia));
                
                if (!resultadoQR.EsValido)
                    throw new InvalidOperationException(string.Format("Código QR no válido: {0}", resultadoQR.Mensaje));

                // Obtener la solicitud desde el código QR
                var codigoQREntidad = await _context.TbCodigoQr
                    .Include(q => q.Solicitud)
                    .ThenInclude(s => s.Paciente)
                    .FirstOrDefaultAsync(q => q.CodigoQr == codigoQR);

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
                    string.Format("Dispensación registrada para paciente {0} {1} en farmacia {2}", 
                        paciente.PrimerNombre, 
                        paciente.PrimerApellido, 
                        farmacia.NombreFarmacia), 
                    usuarioId, 
                    entidad.Id.ToString());

                // Notificar al paciente (opcional)
                if (!string.IsNullOrEmpty(paciente.CorreoElectronico))
                {
                    await _notificaciones.EnviarNotificacion("DISPENSACION_REGISTRADA", usuarioId, new
                    {
                        NombrePaciente = string.Format("{0} {1}", paciente.PrimerNombre, paciente.PrimerApellido),
                        NombreFarmacia = farmacia.NombreFarmacia,
                        Producto = dispensacion.Producto,
                        Cantidad = dispensacion.Cantidad,
                        UnidadMedida = dispensacion.UnidadMedida,
                        FechaDispensacion = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                        Farmaceutico = dispensacion.FarmaceuticoResponsable
                    });
                }

                // Mapear a ViewModel
                var resultado = new RegistroDispensacionViewModel
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
                    UsuarioRegistro = entidad.UsuarioRegistro
                };

                // Agregar propiedades de navegación
                resultado.Solicitud = new SolicitudCannabisViewModel
                {
                    Paciente = new PacienteViewModel
                    {
                        PrimerNombre = paciente.PrimerNombre,
                        SegundoNombre = paciente.SegundoNombre,
                        PrimerApellido = paciente.PrimerApellido,
                        SegundoApellido = paciente.SegundoApellido,
                        DocumentoCedula = paciente.DocumentoCedula,
                        CorreoElectronico = paciente.CorreoElectronico
                    }
                };

                resultado.Farmacia = new FarmaciaAutorizadaViewModel
                {
                    NombreFarmacia = farmacia.NombreFarmacia,
                    CodigoFarmacia = farmacia.CodigoFarmacia
                };

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando dispensación en farmacia {0}", farmaciaId);
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
                _logger.LogError(ex, "Error obteniendo farmacias por provincia {0}", provinciaId);
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
                _logger.LogError(ex, "Error obteniendo farmacia por código {0}", codigoFarmacia);
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
                    throw new KeyNotFoundException(string.Format("Farmacia con ID {0} no encontrada", farmaciaId));

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

                // Detalle de dispensaciones
                var detalles = dispensaciones
                    .Select(d => new RegistroDispensacionViewModel
                    {
                        Id = d.Id,
                        FechaDispensacion = d.FechaDispensacion,
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
                    .ToList();

                var reporte = new ReporteDispensacionViewModel
                {
                    FarmaciaId = farmaciaId,
                    NombreFarmacia = farmacia.NombreFarmacia,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    FechaGeneracion = DateTime.Now,

                    TotalDispensaciones = dispensaciones.Count,
                    TotalPacientesAtendidos = dispensaciones.Select(d => d.Solicitud?.PacienteId).Distinct().Count(),
                    CantidadTotalProducto = (decimal)dispensaciones.Sum(d => d.Cantidad),

                    Productos = productos,
                    Detalles = detalles
                };

                return reporte;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de dispensación para farmacia {0}", farmaciaId);
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
                var ano = DateTime.Now.Year;
                var random = new Random().Next(1000, 9999);
                codigo = string.Format("FARM-{0}-{1}", ano, random);
                
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
            var viewModel = new FarmaciaAutorizadaViewModel
            {
                Id = entidad.Id,
                CodigoFarmacia = entidad.CodigoFarmacia,
                NombreFarmacia = entidad.NombreFarmacia,
                RUC = entidad.Ruc,
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
                FechaRegistro = entidad.FechaRegistro
            };

            // Agregar propiedades de navegación
            if (entidad.Provincia != null)
            {
                viewModel.Provincia = new ProvinciaViewModel 
                { 
                    Id = entidad.Provincia.Id, 
                    NombreProvincia = entidad.Provincia.NombreProvincia 
                };
            }

            if (entidad.Distrito != null)
            {
                viewModel.Distrito = new DistritoViewModel 
                { 
                    Id = entidad.Distrito.Id, 
                    NombreDistrito = entidad.Distrito.NombreDistrito 
                };
            }

            return viewModel;
        }

        #endregion
    }
}