using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class SolicitudService : ISolicitudService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<SolicitudService> _logger;

    public SolicitudService(DbContextDigesa context, ILogger<SolicitudService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResultModel<int>> CrearSolicitudAsync(RegistroCannabisModel registro, List<IBrowserFile>? documentos = null)
    {
        try
        {
            // Implementa la lógica de creación de solicitud aquí
            // Esto es un placeholder
            await Task.Delay(100);
            return ResultModel<int>.ErrorResult("Método no implementado completamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando solicitud");
            return ResultModel<int>.ErrorResult("Error al crear la solicitud", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<bool>> ActualizarEstadoSolicitudAsync(EvaluacionSolicitudModel evaluacion, string usuarioRevisor)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .FirstOrDefaultAsync(s => s.Id == evaluacion.SolicitudId);

            if (solicitud == null)
                return ResultModel<bool>.ErrorResult("Solicitud no encontrada");

            var estado = await _context.TbEstadoSolicitud
                .FirstOrDefaultAsync(e => e.NombreEstado.ToLower() == evaluacion.Accion.ToLower());

            if (estado == null)
                return ResultModel<bool>.ErrorResult("Estado no válido");

            solicitud.EstadoSolicitudId = estado.IdEstado;
            solicitud.FechaRevision = DateOnly.FromDateTime(DateTime.Now);
            solicitud.UsuarioRevisor = usuarioRevisor;
            solicitud.ComentarioRevision = evaluacion.Motivo;

            if (evaluacion.Accion.ToLower() == "aprobada")
            {
                solicitud.FechaAprobacion = DateTime.Now;
            }

            _context.TbSolRegCannabis.Update(solicitud);
            await _context.SaveChangesAsync();

            return ResultModel<bool>.SuccessResult(true, "Estado actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error actualizando estado de solicitud");
            return ResultModel<bool>.ErrorResult("Error al actualizar el estado", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<SolicitudDetalleModel>> ObtenerSolicitudDetalleAsync(int solicitudId)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud == null)
                return ResultModel<SolicitudDetalleModel>.ErrorResult("Solicitud no encontrada");

            // Obtener datos relacionados por separado
            var acompanante = await _context.TbAcompanantePaciente
                .FirstOrDefaultAsync(a => a.PacienteId == solicitud.PacienteId);

            var medico = await _context.TbMedicoPaciente
                .FirstOrDefaultAsync(m => m.PacienteId == solicitud.PacienteId);

            var diagnosticos = await _context.TbPacienteDiagnostico
                .Where(d => d.PacienteId == solicitud.PacienteId)
                .ToListAsync();

            var productos = await _context.TbNombreProductoPaciente
                .Where(p => p.PacienteId == solicitud.PacienteId)
                .ToListAsync();

            var documentos = await _context.TbDocumentoAdjunto
                .Where(d => d.SolRegCannabisId == solicitudId)
                .Include(d => d.TipoDocumento)
                .ToListAsync();

            var detalle = new SolicitudDetalleModel
            {
                Id = solicitud.Id,
                NumeroSolicitud = solicitud.NumSolCompleta ?? "N/A",
                FechaSolicitud = solicitud.FechaSolicitud ?? DateTime.MinValue,
                Estado = solicitud.EstadoSolicitud?.NombreEstado ?? "Desconocido",
                EsRenovacion = solicitud.EsRenovacion ?? false,
                ComentarioRevision = solicitud.ComentarioRevision,
                FechaRevision = solicitud.FechaRevision?.ToDateTime(TimeOnly.MinValue),
                UsuarioRevisor = solicitud.UsuarioRevisor,
                FechaAprobacion = solicitud.FechaAprobacion,
                FechaEmisionCarnet = solicitud.FechaEmisionCarnet,
                FechaVencimientoCarnet = solicitud.FechaVencimientoCarnet,
                NumeroCarnet = solicitud.NumeroCarnet,
                CarnetActivo = solicitud.CarnetActivo ?? false
            };

            return ResultModel<SolicitudDetalleModel>.SuccessResult(detalle, "Solicitud obtenida exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo detalle de solicitud");
            return ResultModel<SolicitudDetalleModel>.ErrorResult("Error al obtener la solicitud", new List<string> { ex.Message });
        }
    }

    public async Task<List<SolicitudListModel>> ObtenerSolicitudesAsync(SolicitudesFiltroModel? filtros = null)
    {
        try
        {
            var query = _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .AsQueryable();

            if (filtros != null)
            {
                if (!string.IsNullOrEmpty(filtros.Estado))
                {
                    query = query.Where(s => s.EstadoSolicitud.NombreEstado == filtros.Estado);
                }

                if (!string.IsNullOrEmpty(filtros.TerminoBusqueda))
                {
                    var termino = filtros.TerminoBusqueda.ToLower();
                    query = query.Where(s =>
                        s.Paciente.PrimerNombre.ToLower().Contains(termino) ||
                        s.Paciente.PrimerApellido.ToLower().Contains(termino) ||
                        (s.Paciente.DocumentoCedula != null && s.Paciente.DocumentoCedula.ToLower().Contains(termino)) ||
                        (s.Paciente.DocumentoPasaporte != null && s.Paciente.DocumentoPasaporte.ToLower().Contains(termino)) ||
                        (s.NumSolCompleta != null && s.NumSolCompleta.ToLower().Contains(termino)));
                }
            }

            return await query
                .OrderByDescending(s => s.FechaSolicitud)
                .Select(s => new SolicitudListModel
                {
                    Id = s.Id,
                    NumeroSolicitud = s.NumSolCompleta ?? "N/A",
                    FechaSolicitud = s.FechaSolicitud ?? DateTime.MinValue,
                    Estado = s.EstadoSolicitud.NombreEstado,
                    PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                    PacienteDocumento = !string.IsNullOrEmpty(s.Paciente.DocumentoCedula)
                        ? s.Paciente.DocumentoCedula
                        : s.Paciente.DocumentoPasaporte ?? "N/A",
                    PacienteCorreo = s.Paciente.CorreoElectronico ?? string.Empty,
                    EsRenovacion = s.EsRenovacion ?? false,
                    NumeroCarnet = s.NumeroCarnet,
                    CarnetActivo = s.CarnetActivo ?? false,
                    FechaVencimientoCarnet = s.FechaVencimientoCarnet
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo solicitudes");
            return new List<SolicitudListModel>();
        }
    }

    public async Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync()
    {
        try
        {
            var conteos = await _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .Where(s => s.EstadoSolicitud != null)
                .GroupBy(s => s.EstadoSolicitud.NombreEstado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Cantidad = g.Count()
                })
                .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);

            return conteos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo conteo por estado");
            return new Dictionary<string, int>();
        }
    }

    public async Task<ResultModel<bool>> ValidarSolicitudCompletaAsync(RegistroCannabisModel registro)
    {
        var errores = new List<string>();

        if (registro.Paciente == null)
            errores.Add("El paciente es requerido");

        if (errores.Any())
            return ResultModel<bool>.ErrorResult("Validación fallida", errores);

        return ResultModel<bool>.SuccessResult(true, "Validación exitosa");
    }

    public async Task<ResultModel<bool>> GenerarCarnetAsync(int solicitudId)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud == null)
                return ResultModel<bool>.ErrorResult("Solicitud no encontrada");

            if (solicitud.EstadoSolicitud?.NombreEstado != "Aprobada")
                return ResultModel<bool>.ErrorResult("La solicitud debe estar aprobada para generar el carnet");

            if (!string.IsNullOrEmpty(solicitud.NumeroCarnet))
                return ResultModel<bool>.ErrorResult("La solicitud ya tiene un carnet generado");

            // Generar número de carnet
            var anio = DateTime.Now.Year;
            var mes = DateTime.Now.Month.ToString("00");
            var conteo = await _context.TbSolRegCannabis
                .Where(s => s.FechaEmisionCarnet != null && 
                           s.FechaEmisionCarnet.Value.Year == anio &&
                           s.FechaEmisionCarnet.Value.Month == DateTime.Now.Month)
                .CountAsync();

            var secuencia = (conteo + 1).ToString("0000");
            solicitud.NumeroCarnet = $"CM-{anio}{mes}-{secuencia}";
            solicitud.FechaEmisionCarnet = DateTime.Now;
            solicitud.FechaVencimientoCarnet = DateTime.Now.AddYears(2);
            solicitud.CarnetActivo = true;

            _context.TbSolRegCannabis.Update(solicitud);
            await _context.SaveChangesAsync();

            return ResultModel<bool>.SuccessResult(true, "Carnet generado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error generando carnet para solicitud {solicitudId}");
            return ResultModel<bool>.ErrorResult("Error al generar el carnet", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<bool>> InactivarCarnetAsync(int solicitudId, string motivo)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud == null)
                return ResultModel<bool>.ErrorResult("Solicitud no encontrada");

            if (string.IsNullOrEmpty(solicitud.NumeroCarnet))
                return ResultModel<bool>.ErrorResult("La solicitud no tiene carnet generado");

            solicitud.CarnetActivo = false;
            solicitud.FechaVencimientoCarnet = DateTime.Now;

            _context.TbSolRegCannabis.Update(solicitud);
            await _context.SaveChangesAsync();

            return ResultModel<bool>.SuccessResult(true, "Carnet inactivado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inactivando carnet para solicitud {solicitudId}");
            return ResultModel<bool>.ErrorResult("Error al inactivar el carnet", new List<string> { ex.Message });
        }
    }
}