using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DIGESA.Repositorios.ServiciosCannabis
{
    public class ServicioMedicos : IServicioMedicos
    {
        private readonly DbContextDigesa _context;
        private readonly ILogger<ServicioMedicos> _logger;
        private readonly IServicioNotificaciones _notificaciones;
        private readonly IServicioHistorial _historial;

        public ServicioMedicos(DbContextDigesa context, 
                             ILogger<ServicioMedicos> logger,
                             IServicioNotificaciones notificaciones,
                             IServicioHistorial historial)
        {
            _context = context;
            _logger = logger;
            _notificaciones = notificaciones;
            _historial = historial;
        }

        public async Task<MedicoViewModel> CrearMedico(MedicoViewModel medico, string usuarioId)
        {
            try
            {
                // Validar que no exista médico con mismo número de colegiatura
                var existeColegiatura = await _context.TbMedico
                    .AnyAsync(m => m.NumeroColegiatura == medico.NumeroColegiatura);
                
                if (existeColegiatura)
                    throw new InvalidOperationException($"Ya existe un médico registrado con número de colegiatura: {medico.NumeroColegiatura}");

                // Validar que no exista médico con mismo documento
                var existeDocumento = await _context.TbMedico
                    .AnyAsync(m => m.TipoDocumento == medico.TipoDocumento && 
                                  m.NumeroDocumento == medico.NumeroDocumento);
                
                if (existeDocumento)
                    throw new InvalidOperationException($"Ya existe un médico registrado con {medico.TipoDocumento}: {medico.NumeroDocumento}");

                // Generar código único de médico
                var codigoMedico = await GenerarCodigoMedicoUnico();

                var entidad = new TbMedico
                {
                    CodigoMedico = codigoMedico,
                    PrimerNombre = medico.PrimerNombre,
                    SegundoNombre = medico.SegundoNombre,
                    PrimerApellido = medico.PrimerApellido,
                    SegundoApellido = medico.SegundoApellido,
                    TipoDocumento = medico.TipoDocumento,
                    NumeroDocumento = medico.NumeroDocumento,
                    Especialidad = medico.Especialidad,
                    Subespecialidad = medico.Subespecialidad,
                    NumeroColegiatura = medico.NumeroColegiatura,
                    TelefonoConsultorio = medico.TelefonoConsultorio,
                    TelefonoMovil = medico.TelefonoMovil,
                    Email = medico.Email?.ToLower(),
                    DireccionConsultorio = medico.DireccionConsultorio,
                    ProvinciaId = medico.ProvinciaId,
                    DistritoId = medico.DistritoId,
                    RegionSaludId = medico.RegionSaludId,
                    InstalacionSaludId = medico.InstalacionSaludId,
                    InstalacionPersonalizada = medico.InstalacionPersonalizada,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = usuarioId,
                    Activo = true,
                    Verificado = false,
                    FechaVerificacion = null,
                    UsuarioVerificador = null,
                    Observaciones = null
                };

                await _context.TbMedico.AddAsync(entidad);
                await _context.SaveChangesAsync();

                // Registrar auditoría específica para médico
                var auditoriaMedico = new TbAuditoriaMedico
                {
                    MedicoId = entidad.Id,
                    TipoAccion = "CREACION",
                    UsuarioAccion = usuarioId,
                    FechaAccion = DateTime.Now,
                    DetallesAnteriores = null,
                    DetallesNuevos = System.Text.Json.JsonSerializer.Serialize(entidad),
                    Comentarios = $"Médico creado por {usuarioId}",
                    IpOrigen = null
                };

                await _context.TbAuditoriaMedico.AddAsync(auditoriaMedico);
                await _context.SaveChangesAsync();

                // Registrar en historial usando método disponible
                await RegistrarEnHistorial("MEDICO_CREADO", 
                    $"Médico creado: {entidad.PrimerNombre} {entidad.PrimerApellido}", 
                    usuarioId, entidad.Id);

                // Notificar al administrador para verificación
                await EnviarNotificacionNuevoMedico(entidad, usuarioId);

                // Mapear de vuelta al ViewModel
                medico.Id = entidad.Id;
                medico.CodigoMedico = codigoMedico;
                medico.FechaRegistro = entidad.FechaRegistro;
                medico.Activo = entidad.Activo;
                medico.Verificado = entidad.Verificado;

                return medico;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando médico");
                await RegistrarErrorEnHistorial("ServicioMedicos.CrearMedico", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<MedicoViewModel> ActualizarMedico(int medicoId, MedicoViewModel medico, string usuarioId)
        {
            try
            {
                var entidad = await _context.TbMedico.FindAsync(medicoId);
                if (entidad == null)
                    throw new KeyNotFoundException($"Médico con ID {medicoId} no encontrado");

                // Guardar datos anteriores para auditoría
                var datosAnteriores = System.Text.Json.JsonSerializer.Serialize(entidad);

                // Actualizar campos permitidos
                entidad.PrimerNombre = medico.PrimerNombre;
                entidad.SegundoNombre = medico.SegundoNombre;
                entidad.PrimerApellido = medico.PrimerApellido;
                entidad.SegundoApellido = medico.SegundoApellido;
                entidad.Especialidad = medico.Especialidad;
                entidad.Subespecialidad = medico.Subespecialidad;
                entidad.NumeroColegiatura = medico.NumeroColegiatura;
                entidad.TelefonoConsultorio = medico.TelefonoConsultorio;
                entidad.TelefonoMovil = medico.TelefonoMovil;
                entidad.Email = medico.Email?.ToLower();
                entidad.DireccionConsultorio = medico.DireccionConsultorio;
                entidad.ProvinciaId = medico.ProvinciaId;
                entidad.DistritoId = medico.DistritoId;
                entidad.RegionSaludId = medico.RegionSaludId;
                entidad.InstalacionSaludId = medico.InstalacionSaludId;
                entidad.InstalacionPersonalizada = medico.InstalacionPersonalizada;
                entidad.FechaActualizacion = DateTime.Now;
                entidad.Activo = medico.Activo;

                // Si se desactiva un médico verificado, quitar verificación
                if (!medico.Activo && entidad.Verificado)
                {
                    entidad.Verificado = false;
                    entidad.FechaVerificacion = null;
                    entidad.UsuarioVerificador = null;
                    entidad.Observaciones = $"Verificación revocada al desactivar. Usuario: {usuarioId}";
                }

                await _context.SaveChangesAsync();

                // Registrar auditoría
                var auditoriaMedico = new TbAuditoriaMedico
                {
                    MedicoId = entidad.Id,
                    TipoAccion = "ACTUALIZACION",
                    UsuarioAccion = usuarioId,
                    FechaAccion = DateTime.Now,
                    DetallesAnteriores = datosAnteriores,
                    DetallesNuevos = System.Text.Json.JsonSerializer.Serialize(entidad),
                    Comentarios = $"Médico actualizado por {usuarioId}",
                    IpOrigen = null
                };

                await _context.TbAuditoriaMedico.AddAsync(auditoriaMedico);
                await _context.SaveChangesAsync();

                await RegistrarEnHistorial("MEDICO_ACTUALIZADO", 
                    $"Médico actualizado: {entidad.PrimerNombre} {entidad.PrimerApellido}", 
                    usuarioId, entidad.Id);

                return MapToViewModel(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando médico {MedicoId}", medicoId);
                await RegistrarErrorEnHistorial("ServicioMedicos.ActualizarMedico", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<bool> EliminarMedico(int medicoId, string usuarioId, string motivo)
        {
            try
            {
                var entidad = await _context.TbMedico.FindAsync(medicoId);
                if (entidad == null)
                    throw new KeyNotFoundException($"Médico con ID {medicoId} no encontrado");

                // Verificar que no tenga pacientes activos
                var tienePacientes = await _context.TbMedicoPaciente
                    .AnyAsync(mp => mp.MedicoId == medicoId);
                
                if (tienePacientes)
                    throw new InvalidOperationException("No se puede eliminar médico con pacientes asignados");

                // Marcar como inactivo (soft delete)
                var datosAnteriores = System.Text.Json.JsonSerializer.Serialize(entidad);
                
                entidad.Activo = false;
                entidad.FechaActualizacion = DateTime.Now;
                entidad.Observaciones = $"Eliminado por {usuarioId}. Motivo: {motivo}";

                await _context.SaveChangesAsync();

                // Registrar auditoría
                var auditoriaMedico = new TbAuditoriaMedico
                {
                    MedicoId = entidad.Id,
                    TipoAccion = "ELIMINACION",
                    UsuarioAccion = usuarioId,
                    FechaAccion = DateTime.Now,
                    DetallesAnteriores = datosAnteriores,
                    DetallesNuevos = System.Text.Json.JsonSerializer.Serialize(entidad),
                    Comentarios = $"Médico eliminado. Motivo: {motivo}",
                    IpOrigen = null
                };

                await _context.TbAuditoriaMedico.AddAsync(auditoriaMedico);
                await _context.SaveChangesAsync();

                await RegistrarEnHistorial("MEDICO_ELIMINADO", 
                    $"Médico eliminado: {entidad.PrimerNombre} {entidad.PrimerApellido}. Motivo: {motivo}", 
                    usuarioId, entidad.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando médico {MedicoId}", medicoId);
                await RegistrarErrorEnHistorial("ServicioMedicos.EliminarMedico", ex.Message, usuarioId);
                throw;
            }
        }

        public async Task<MedicoViewModel> ObtenerMedicoPorId(int medicoId)
        {
            try
            {
                var entidad = await _context.TbMedico
                    .Include(m => m.Provincia)
                    .Include(m => m.Distrito)
                    .Include(m => m.RegionSalud)
                    .Include(m => m.InstalacionSalud)
                    .FirstOrDefaultAsync(m => m.Id == medicoId);

                if (entidad == null)
                    return null;

                return MapToViewModel(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo médico por ID {MedicoId}", medicoId);
                throw;
            }
        }

        public async Task<MedicoViewModel> ObtenerMedicoPorCodigo(string codigoMedico)
        {
            try
            {
                var entidad = await _context.TbMedico
                    .Include(m => m.Provincia)
                    .Include(m => m.Distrito)
                    .Include(m => m.RegionSalud)
                    .Include(m => m.InstalacionSalud)
                    .FirstOrDefaultAsync(m => m.CodigoMedico == codigoMedico && m.Activo);

                if (entidad == null)
                    return null;

                return MapToViewModel(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo médico por código {CodigoMedico}", codigoMedico);
                throw;
            }
        }

        public async Task<MedicoViewModel> ObtenerMedicoPorDocumento(string tipoDocumento, string numeroDocumento)
        {
            try
            {
                var entidad = await _context.TbMedico
                    .Include(m => m.Provincia)
                    .Include(m => m.Distrito)
                    .Include(m => m.RegionSalud)
                    .Include(m => m.InstalacionSalud)
                    .FirstOrDefaultAsync(m => m.TipoDocumento == tipoDocumento && 
                                            m.NumeroDocumento == numeroDocumento && 
                                            m.Activo);

                if (entidad == null)
                    return null;

                return MapToViewModel(entidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo médico por documento {Tipo}/{Numero}", tipoDocumento, numeroDocumento);
                throw;
            }
        }

        public async Task<List<MedicoViewModel>> BuscarMedicos(string criterio, bool soloActivos = true)
        {
            try
            {
                var query = _context.TbMedico
                    .Include(m => m.Provincia)
                    .Include(m => m.Distrito)
                    .Include(m => m.RegionSalud)
                    .Include(m => m.InstalacionSalud)
                    .AsQueryable();

                if (soloActivos)
                    query = query.Where(m => m.Activo);

                if (!string.IsNullOrEmpty(criterio))
                {
                    criterio = criterio.ToLower();
                    query = query.Where(m =>
                        m.CodigoMedico.ToLower().Contains(criterio) ||
                        (m.PrimerNombre + " " + m.PrimerApellido).ToLower().Contains(criterio) ||
                        m.NumeroDocumento.Contains(criterio) ||
                        m.Email.ToLower().Contains(criterio) ||
                        m.Especialidad.ToLower().Contains(criterio) ||
                        m.NumeroColegiatura.Contains(criterio));
                }

                var medicos = await query
                    .OrderBy(m => m.PrimerApellido)
                    .ThenBy(m => m.PrimerNombre)
                    .ToListAsync();

                return medicos.Select(MapToViewModel).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buscando médicos con criterio: {Criterio}", criterio);
                throw;
            }
        }

        public async Task<bool> VerificarMedico(int medicoId, string usuarioVerificador, string observaciones)
        {
            try
            {
                var entidad = await _context.TbMedico.FindAsync(medicoId);
                if (entidad == null)
                    throw new KeyNotFoundException($"Médico con ID {medicoId} no encontrado");

                // Validar requisitos previos
                if (!entidad.Activo)
                    throw new InvalidOperationException("No se puede verificar un médico inactivo");

                if (string.IsNullOrEmpty(entidad.NumeroColegiatura))
                    throw new InvalidOperationException("El médico debe tener número de colegiatura");

                if (string.IsNullOrEmpty(entidad.Email))
                    throw new InvalidOperationException("El médico debe tener email registrado");

                // Guardar datos anteriores
                var datosAnteriores = System.Text.Json.JsonSerializer.Serialize(entidad);

                // Actualizar estado
                entidad.Verificado = true;
                entidad.FechaVerificacion = DateTime.Now;
                entidad.UsuarioVerificador = usuarioVerificador;
                entidad.Observaciones = observaciones;
                entidad.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                // Crear registro en auditoría de médico
                var auditoriaMedico = new TbAuditoriaMedico
                {
                    MedicoId = medicoId,
                    TipoAccion = "VERIFICACION",
                    UsuarioAccion = usuarioVerificador,
                    FechaAccion = DateTime.Now,
                    DetallesAnteriores = datosAnteriores,
                    DetallesNuevos = System.Text.Json.JsonSerializer.Serialize(entidad),
                    Comentarios = observaciones,
                    IpOrigen = null
                };

                await _context.TbAuditoriaMedico.AddAsync(auditoriaMedico);
                await _context.SaveChangesAsync();

                // Registrar en historial
                await RegistrarEnHistorial("MEDICO_VERIFICADO", 
                    $"Médico verificado: {entidad.PrimerNombre} {entidad.PrimerApellido}", 
                    usuarioVerificador, entidad.Id);

                // Notificar al médico
                await EnviarNotificacionMedicoVerificado(entidad, usuarioVerificador, observaciones);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando médico {MedicoId}", medicoId);
                await RegistrarErrorEnHistorial("ServicioMedicos.VerificarMedico", ex.Message, usuarioVerificador);
                throw;
            }
        }

        public async Task<bool> RevocarVerificacion(int medicoId, string usuario, string motivo)
        {
            try
            {
                var entidad = await _context.TbMedico.FindAsync(medicoId);
                if (entidad == null)
                    throw new KeyNotFoundException($"Médico con ID {medicoId} no encontrado");

                if (!entidad.Verificado)
                    throw new InvalidOperationException("El médico no está verificado");

                // Guardar datos anteriores
                var datosAnteriores = System.Text.Json.JsonSerializer.Serialize(entidad);

                // Revocar verificación
                entidad.Verificado = false;
                entidad.FechaVerificacion = null;
                entidad.UsuarioVerificador = null;
                entidad.Observaciones = $"Verificación revocada por {usuario}. Motivo: {motivo}";
                entidad.FechaActualizacion = DateTime.Now;

                await _context.SaveChangesAsync();

                // Crear registro en auditoría de médico
                var auditoriaMedico = new TbAuditoriaMedico
                {
                    MedicoId = medicoId,
                    TipoAccion = "REVOCACION_VERIFICACION",
                    UsuarioAccion = usuario,
                    FechaAccion = DateTime.Now,
                    DetallesAnteriores = datosAnteriores,
                    DetallesNuevos = System.Text.Json.JsonSerializer.Serialize(entidad),
                    Comentarios = motivo,
                    IpOrigen = null
                };

                await _context.TbAuditoriaMedico.AddAsync(auditoriaMedico);
                await _context.SaveChangesAsync();

                // Registrar en historial
                await RegistrarEnHistorial("MEDICO_VERIFICACION_REVOCADA", 
                    $"Verificación revocada a médico: {entidad.PrimerNombre} {entidad.PrimerApellido}. Motivo: {motivo}", 
                    usuario, entidad.Id);

                // Notificar al médico
                await EnviarNotificacionVerificacionRevocada(entidad, usuario, motivo);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revocando verificación médico {MedicoId}", medicoId);
                await RegistrarErrorEnHistorial("ServicioMedicos.RevocarVerificacion", ex.Message, usuario);
                throw;
            }
        }

        public async Task<ReporteMedicosViewModel> GenerarReporteMedicos(DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                var query = _context.TbMedico.AsQueryable();

                if (fechaInicio.HasValue)
                    query = query.Where(m => m.FechaRegistro >= fechaInicio.Value);

                if (fechaFin.HasValue)
                    query = query.Where(m => m.FechaRegistro <= fechaFin.Value);

                var medicos = await query.ToListAsync();

                // Calcular distribuciones
                var distribucionEspecialidad = medicos
                    .Where(m => !string.IsNullOrEmpty(m.Especialidad))
                    .GroupBy(m => m.Especialidad)
                    .Select(g => new EspecialidadCountViewModel
                    {
                        Especialidad = g.Key,
                        Cantidad = g.Count(),
                        Porcentaje = Math.Round((g.Count() * 100.0) / medicos.Count, 2)
                    })
                    .OrderByDescending(d => d.Cantidad)
                    .ToList();

                var distribucionProvincia = medicos
                    .Where(m => m.ProvinciaId.HasValue)
                    .GroupBy(m => m.ProvinciaId)
                    .Select(g => new ProvinciaCountViewModel
                    {
                        ProvinciaId = g.Key ?? 0,
                        Cantidad = g.Count(),
                        Porcentaje = Math.Round((g.Count() * 100.0) / medicos.Count, 2)
                    })
                    .OrderByDescending(d => d.Cantidad)
                    .ToList();

                var registrosMensuales = medicos
                    .GroupBy(m => new { m.FechaRegistro.Year, m.FechaRegistro.Month })
                    .Select(g => new RegistroMensualViewModel
                    {
                        Ano = g.Key.Year,
                        Mes = g.Key.Month,
                        Cantidad = g.Count()
                    })
                    .OrderBy(r => r.Ano)
                    .ThenBy(r => r.Mes)
                    .ToList();

                // Contar médicos especialistas en cannabis
                var medicosEspecialistasCannabis = medicos
                    .Count(m => !string.IsNullOrEmpty(m.Especialidad) && 
                               m.Especialidad.Contains("Cannabis", StringComparison.OrdinalIgnoreCase));

                var reporte = new ReporteMedicosViewModel
                {
                    FechaGeneracion = DateTime.Now,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    TotalMedicos = medicos.Count,
                    MedicosActivos = medicos.Count(m => m.Activo),
                    MedicosVerificados = medicos.Count(m => m.Verificado),
                    MedicosEspecialistasCannabis = medicosEspecialistasCannabis,
                    DistribucionEspecialidad = distribucionEspecialidad,
                    DistribucionProvincia = distribucionProvincia,
                    RegistrosMensuales = registrosMensuales,
                    Medicos = medicos.Select(m => new MedicoReporteViewModel
                    {
                        Id = m.Id,
                        CodigoMedico = m.CodigoMedico,
                        Nombre = $"{m.PrimerNombre} {m.PrimerApellido}",
                        Especialidad = m.Especialidad,
                        Email = m.Email,
                        Telefono = m.TelefonoMovil ?? m.TelefonoConsultorio,
                        Activo = m.Activo,
                        Verificado = m.Verificado,
                        FechaRegistro = m.FechaRegistro,
                        FechaVerificacion = m.FechaVerificacion
                    }).ToList()
                };

                return reporte;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte de médicos");
                throw;
            }
        }

        public async Task<EstadisticasMedicosViewModel> ObtenerEstadisticasMedicos()
        {
            try
            {
                var totalMedicos = await _context.TbMedico.CountAsync();
                var medicosActivos = await _context.TbMedico.CountAsync(m => m.Activo);
                var medicosVerificados = await _context.TbMedico.CountAsync(m => m.Verificado);
                var medicosInactivos = totalMedicos - medicosActivos;
                
                // Contar médicos especialistas en cannabis
                var medicosEspecialistasCannabis = await _context.TbMedico
                    .CountAsync(m => !string.IsNullOrEmpty(m.Especialidad) && 
                                    m.Especialidad.Contains("Cannabis", StringComparison.OrdinalIgnoreCase));

                // Obtener distribución por especialidad
                var neurologos = await _context.TbMedico.CountAsync(m => 
                    m.Especialidad.Contains("Neurología", StringComparison.OrdinalIgnoreCase) ||
                    m.Especialidad.Contains("Neurologo", StringComparison.OrdinalIgnoreCase));

                var oncologos = await _context.TbMedico.CountAsync(m => 
                    m.Especialidad.Contains("Oncología", StringComparison.OrdinalIgnoreCase) ||
                    m.Especialidad.Contains("Oncologo", StringComparison.OrdinalIgnoreCase));

                var psiquiatras = await _context.TbMedico.CountAsync(m => 
                    m.Especialidad.Contains("Psiquiatría", StringComparison.OrdinalIgnoreCase) ||
                    m.Especialidad.Contains("Psiquiatra", StringComparison.OrdinalIgnoreCase));

                var medDolor = await _context.TbMedico.CountAsync(m => 
                    m.Especialidad.Contains("Dolor", StringComparison.OrdinalIgnoreCase) ||
                    m.Especialidad.Contains("Algiología", StringComparison.OrdinalIgnoreCase));

                var pediatras = await _context.TbMedico.CountAsync(m => 
                    m.Especialidad.Contains("Pediatría", StringComparison.OrdinalIgnoreCase) ||
                    m.Especialidad.Contains("Pediatra", StringComparison.OrdinalIgnoreCase));

                // Obtener distribución por provincia
                var panama = await _context.TbMedico.CountAsync(m => m.ProvinciaId == 1); // ID de Panamá
                var colon = await _context.TbMedico.CountAsync(m => m.ProvinciaId == 2); // ID de Colón
                // ... completar con otras provincias según IDs

                // Obtener médicos registrados este mes y mes anterior
                var inicioEsteMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var inicioMesAnterior = inicioEsteMes.AddMonths(-1);
                var finMesAnterior = inicioEsteMes.AddDays(-1);

                var registrosEsteMes = await _context.TbMedico
                    .CountAsync(m => m.FechaRegistro >= inicioEsteMes);

                var registrosMesAnterior = await _context.TbMedico
                    .CountAsync(m => m.FechaRegistro >= inicioMesAnterior && m.FechaRegistro <= finMesAnterior);

                // Calcular variación porcentual
                decimal variacionPorcentual = 0;
                if (registrosMesAnterior > 0)
                {
                    variacionPorcentual = Math.Round(((registrosEsteMes - (decimal)registrosMesAnterior) / registrosMesAnterior) * 100, 2);
                }

                // Obtener total de pacientes atendidos
                var totalPacientesAtendidos = await _context.TbMedicoPaciente.CountAsync();
                var promedioPacientesPorMedico = totalMedicos > 0 ? 
                    Math.Round((decimal)totalPacientesAtendidos / totalMedicos, 2) : 0;

                // Obtener médico con más pacientes
                var topMedico = await _context.TbMedicoPaciente
                    .GroupBy(mp => mp.MedicoId)
                    .Select(g => new 
                    { 
                        MedicoId = g.Key, 
                        Cantidad = g.Count() 
                    })
                    .OrderByDescending(x => x.Cantidad)
                    .FirstOrDefaultAsync();

                var topMedicoNombre = "N/A";
                if (topMedico != null)
                {
                    var medico = await _context.TbMedico
                        .FirstOrDefaultAsync(m => m.Id == topMedico.MedicoId);
                    if (medico != null)
                    {
                        topMedicoNombre = $"{medico.PrimerNombre} {medico.PrimerApellido}";
                    }
                }

                var estadisticas = new EstadisticasMedicosViewModel
                {
                    TotalMedicos = totalMedicos,
                    MedicosVerificados = medicosVerificados,
                    MedicosActivos = medicosActivos,
                    MedicosInactivos = medicosInactivos,
                    MedicosEspecialistasCannabis = medicosEspecialistasCannabis,
                    
                    // Distribución por especialidad
                    Neurologos = neurologos,
                    Oncologos = oncologos,
                    Psiquiatras = psiquiatras,
                    MedDolor = medDolor,
                    Pediatras = pediatras,
                    Otros = totalMedicos - (neurologos + oncologos + psiquiatras + medDolor + pediatras),
                    
                    // Distribución por provincia (ejemplo, completar según IDs reales)
                    Panama = panama,
                    Colon = colon,
                    Chiriqui = 0, // Completar según IDs
                    Veraguas = 0,
                    Cocle = 0,
                    BocasDelToro = 0,
                    Herrera = 0,
                    LosSantos = 0,
                    Darien = 0,
                    GunaYala = 0,
                    NgabeBugle = 0,
                    EmberaWounaan = 0,
                    
                    // Tendencias
                    RegistrosEsteMes = registrosEsteMes,
                    RegistrosMesAnterior = registrosMesAnterior,
                    VariacionPorcentual = variacionPorcentual,
                    
                    // Performance
                    TotalPacientesAtendidos = totalPacientesAtendidos,
                    PromedioPacientesPorMedico = promedioPacientesPorMedico,
                    TopMedicoPacientes = topMedico?.Cantidad ?? 0,
                    TopMedicoNombre = topMedicoNombre
                };

                return estadisticas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estadísticas de médicos");
                throw;
            }
        }

        public async Task<List<AuditoriaMedicoViewModel>> ObtenerAuditoriaMedico(int medicoId)
        {
            try
            {
                var auditorias = await _context.TbAuditoriaMedico
                    .Where(a => a.MedicoId == medicoId)
                    .OrderByDescending(a => a.FechaAccion)
                    .ToListAsync();

                return auditorias.Select(a => new AuditoriaMedicoViewModel
                {
                    Id = a.Id,
                    MedicoId = a.MedicoId,
                    Accion = a.TipoAccion,
                    Usuario = a.UsuarioAccion,
                    Fecha = a.FechaAccion,
                    Observaciones = a.Comentarios,
                    IpOrigen = a.IpOrigen
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo auditoría de médico {MedicoId}", medicoId);
                throw;
            }
        }

        public async Task<List<AuditoriaMedicoViewModel>> ObtenerAuditoriaPorUsuario(string usuarioId)
        {
            try
            {
                var auditorias = await _context.TbAuditoriaMedico
                    .Where(a => a.UsuarioAccion == usuarioId)
                    .OrderByDescending(a => a.FechaAccion)
                    .ToListAsync();

                return auditorias.Select(a => new AuditoriaMedicoViewModel
                {
                    Id = a.Id,
                    MedicoId = a.MedicoId,
                    Accion = a.TipoAccion,
                    Usuario = a.UsuarioAccion,
                    Fecha = a.FechaAccion,
                    Observaciones = a.Comentarios,
                    IpOrigen = a.IpOrigen
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo auditoría por usuario {UsuarioId}", usuarioId);
                throw;
            }
        }

        #region Métodos Auxiliares

        private async Task<string> GenerarCodigoMedicoUnico()
        {
            string codigo;
            bool existe;
            int intentos = 0;
            
            do
            {
                // Formato: MED-YYYYMM-XXXXX donde X es numérico
                var timestamp = DateTime.Now.ToString("yyyyMM");
                var random = new Random().Next(10000, 99999);
                codigo = $"MED-{timestamp}-{random}";
                
                existe = await _context.TbMedico.AnyAsync(m => m.CodigoMedico == codigo);
                intentos++;
            }
            while (existe && intentos < 10);

            if (existe)
                throw new InvalidOperationException("No se pudo generar un código único de médico");

            return codigo;
        }

        private MedicoViewModel MapToViewModel(TbMedico entidad)
        {
            var viewModel = new MedicoViewModel
            {
                Id = entidad.Id,
                CodigoMedico = entidad.CodigoMedico,
                PrimerNombre = entidad.PrimerNombre,
                SegundoNombre = entidad.SegundoNombre,
                PrimerApellido = entidad.PrimerApellido,
                SegundoApellido = entidad.SegundoApellido,
                TipoDocumento = entidad.TipoDocumento,
                NumeroDocumento = entidad.NumeroDocumento,
                Especialidad = entidad.Especialidad,
                Subespecialidad = entidad.Subespecialidad,
                NumeroColegiatura = entidad.NumeroColegiatura,
                TelefonoConsultorio = entidad.TelefonoConsultorio,
                TelefonoMovil = entidad.TelefonoMovil,
                Email = entidad.Email,
                DireccionConsultorio = entidad.DireccionConsultorio,
                ProvinciaId = entidad.ProvinciaId,
                DistritoId = entidad.DistritoId,
                RegionSaludId = entidad.RegionSaludId,
                InstalacionSaludId = entidad.InstalacionSaludId,
                InstalacionPersonalizada = entidad.InstalacionPersonalizada,
                FechaRegistro = entidad.FechaRegistro,
                UsuarioRegistro = entidad.UsuarioRegistro,
                FechaActualizacion = entidad.FechaActualizacion,
                Activo = entidad.Activo,
                Verificado = entidad.Verificado,
                FechaVerificacion = entidad.FechaVerificacion,
                UsuarioVerificador = entidad.UsuarioVerificador,
                Observaciones = entidad.Observaciones,
                // TotalPacientesAtendidos se calcula por separado
                TotalPacientesAtendidos = 0
            };

            // Asignar propiedades de navegación si existen
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

            if (entidad.RegionSalud != null)
            {
                viewModel.RegionSalud = new RegionSaludViewModel
                {
                    Id = entidad.RegionSalud.Id,
                    Nombre = entidad.RegionSalud.Nombre
                };
            }

            if (entidad.InstalacionSalud != null)
            {
                viewModel.InstalacionSalud = new InstalacionSaludViewModel
                {
                    Id = entidad.InstalacionSalud.Id,
                    Nombre = entidad.InstalacionSalud.Nombre
                };
            }

            return viewModel;
        }

        private async Task RegistrarEnHistorial(string tipoEvento, string descripcion, string usuario, int referenciaId)
        {
            try
            {
                // Usar el historial de usuario existente
                var historial = new TbHistorialUsuario
                {
                    UsuarioId = usuario,
                    TipoCambio = tipoEvento,
                    Comentario = descripcion,
                    CambioPor = usuario,
                    FechaCambio = DateTime.Now
                };

                await _context.TbHistorialUsuario.AddAsync(historial);
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

        private async Task EnviarNotificacionNuevoMedico(TbMedico medico, string usuarioId)
        {
            try
            {
                // Crear registro en log de notificaciones
                var notificacion = new TbLogNotificaciones
                {
                    SolicitudId = null, // No asociado a solicitud
                    TipoNotificacion = "NUEVO_MEDICO_REGISTRADO",
                    Destinatario = "administrador@digesa.gob.pa", // Email del administrador
                    MetodoEnvio = "Email",
                    Estado = "PENDIENTE",
                    FechaEnvio = DateTime.Now,
                    Error = null
                };

                await _context.TbLogNotificaciones.AddAsync(notificacion);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Notificación de nuevo médico registrada: {NombreMedico} ({CodigoMedico})",
                    $"{medico.PrimerNombre} {medico.PrimerApellido}", medico.CodigoMedico);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error enviando notificación de nuevo médico");
            }
        }

        private async Task EnviarNotificacionMedicoVerificado(TbMedico medico, string usuarioVerificador, string observaciones)
        {
            try
            {
                // Notificar al médico por email
                var notificacion = new TbLogNotificaciones
                {
                    SolicitudId = null,
                    TipoNotificacion = "MEDICO_VERIFICADO",
                    Destinatario = medico.Email,
                    MetodoEnvio = "Email",
                    Estado = "PENDIENTE",
                    FechaEnvio = DateTime.Now,
                    Error = null
                };

                await _context.TbLogNotificaciones.AddAsync(notificacion);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Notificación de médico verificado enviada a: {Email}", medico.Email);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error enviando notificación de médico verificado");
            }
        }

        private async Task EnviarNotificacionVerificacionRevocada(TbMedico medico, string usuario, string motivo)
        {
            try
            {
                var notificacion = new TbLogNotificaciones
                {
                    SolicitudId = null,
                    TipoNotificacion = "MEDICO_VERIFICACION_REVOCADA",
                    Destinatario = medico.Email,
                    MetodoEnvio = "Email",
                    Estado = "PENDIENTE",
                    FechaEnvio = DateTime.Now,
                    Error = motivo
                };

                await _context.TbLogNotificaciones.AddAsync(notificacion);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Notificación de verificación revocada enviada a: {Email}", medico.Email);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error enviando notificación de verificación revocada");
            }
        }

        #endregion
    }
}
