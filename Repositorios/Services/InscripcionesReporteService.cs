using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class InscripcionesReporteService : IInscripcionesReporteService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<InscripcionesReporteService> _logger;

    public InscripcionesReporteService(
        DbContextDigesa context,
        ILogger<InscripcionesReporteService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResultModel<InscripcionesEstadisticasModel>> ObtenerEstadisticasInscripcionesAsync(
        ReporteFiltrosModel filtros)
    {
        try
        {
            var estadisticas = new InscripcionesEstadisticasModel();

            // Total de solicitudes
            var solicitudesQuery = _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .AsQueryable();

            solicitudesQuery = AplicarFiltrosReporte(solicitudesQuery, filtros);

            var solicitudes = await solicitudesQuery.ToListAsync();

            estadisticas.TotalSolicitudes = solicitudes.Count;
            estadisticas.PrimerasInscripciones = solicitudes.Count(s => !(s.EsRenovacion ?? false));
            estadisticas.Renovaciones = solicitudes.Count(s => s.EsRenovacion ?? false);

            // Por estado
            estadisticas.SolicitudesAprobadas = solicitudes.Count(s => 
                s.EstadoSolicitud?.NombreEstado == "Aprobada");
            estadisticas.SolicitudesPendientes = solicitudes.Count(s => 
                s.EstadoSolicitud?.NombreEstado == "Pendiente");
            estadisticas.SolicitudesRechazadas = solicitudes.Count(s => 
                s.EstadoSolicitud?.NombreEstado == "Rechazada");

            // Carnets activos/inactivos
            estadisticas.CarnetsActivos = solicitudes.Count(s => 
                s.CarnetActivo == true && 
                s.FechaVencimientoCarnet > DateTime.Now);
            estadisticas.CarnetsInactivos = solicitudes.Count(s => 
                s.CarnetActivo == false || 
                (s.FechaVencimientoCarnet.HasValue && s.FechaVencimientoCarnet < DateTime.Now));

            // Carnets por vencer
            var hoy = DateTime.Now;
            estadisticas.CarnetsPorVencer30Dias = solicitudes.Count(s => 
                s.CarnetActivo == true && 
                s.FechaVencimientoCarnet.HasValue &&
                s.FechaVencimientoCarnet.Value > hoy && 
                s.FechaVencimientoCarnet.Value <= hoy.AddDays(30));
            estadisticas.CarnetsPorVencer15Dias = solicitudes.Count(s => 
                s.CarnetActivo == true && 
                s.FechaVencimientoCarnet.HasValue &&
                s.FechaVencimientoCarnet.Value > hoy && 
                s.FechaVencimientoCarnet.Value <= hoy.AddDays(15));
            estadisticas.CarnetsPorVencer7Dias = solicitudes.Count(s => 
                s.CarnetActivo == true && 
                s.FechaVencimientoCarnet.HasValue &&
                s.FechaVencimientoCarnet.Value > hoy && 
                s.FechaVencimientoCarnet.Value <= hoy.AddDays(7));
            estadisticas.CarnetsVencidos = solicitudes.Count(s => 
                s.CarnetActivo == true && 
                s.FechaVencimientoCarnet.HasValue &&
                s.FechaVencimientoCarnet.Value < hoy);

            // Por mes (últimos 6 meses)
            var fechaInicio = hoy.AddMonths(-6);
            estadisticas.SolicitudesPorMes = await solicitudesQuery
                .Where(s => s.FechaSolicitud >= fechaInicio)
                .GroupBy(s => new { s.FechaSolicitud.Value.Year, s.FechaSolicitud.Value.Month })
                .Select(g => new
                {
                    Mes = $"{g.Key.Month:00}/{g.Key.Year}",
                    Total = g.Count(),
                    Aprobadas = g.Count(s => s.EstadoSolicitud.NombreEstado == "Aprobada"),
                    Renovaciones = g.Count(s => s.EsRenovacion == true)
                })
                .ToDictionaryAsync(x => x.Mes, x => new InscripcionesMesModel
                {
                    Mes = x.Mes,
                    Total = x.Total,
                    Aprobadas = x.Aprobadas,
                    Renovaciones = x.Renovaciones
                });

            // Por región
            estadisticas.SolicitudesPorRegion = await solicitudesQuery
                .Include(s => s.Paciente)
                .ThenInclude(p => p.Region)
                .Where(s => s.Paciente.Region != null)
                .GroupBy(s => s.Paciente.Region.Nombre)
                .Select(g => new EstadisticaRegionModel
                {
                    Region = g.Key ?? "Sin región",
                    TotalSolicitudes = g.Count(),
                    SolicitudesAprobadas = g.Count(s => s.EstadoSolicitud.NombreEstado == "Aprobada"),
                    SolicitudesPendientes = g.Count(s => s.EstadoSolicitud.NombreEstado == "Pendiente"),
                    SolicitudesRechazadas = g.Count(s => s.EstadoSolicitud.NombreEstado == "Rechazada"),
                    CarnetsActivos = g.Count(s => s.CarnetActivo == true && 
                                                s.FechaVencimientoCarnet > DateTime.Now)
                })
                .ToListAsync();

            estadisticas.FechaGeneracion = DateTime.Now;
            estadisticas.TerminosBusqueda = ObtenerTerminosBusqueda(filtros);

            return ResultModel<InscripcionesEstadisticasModel>.SuccessResult(
                estadisticas, 
                "Estadísticas de inscripciones obtenidas exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo estadísticas de inscripciones");
            return ResultModel<InscripcionesEstadisticasModel>.ErrorResult(
                "Error al obtener las estadísticas", 
                new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<List<InscripcionDetalleModel>>> ObtenerInscripcionesDetalladasAsync(
        ReporteFiltrosModel filtros)
    {
        try
        {
            var query = _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .Include(s => s.Paciente.Region)
                .Include(s => s.Paciente.Instalacion)
                .AsQueryable();

            query = AplicarFiltrosReporte(query, filtros);

            var inscripciones = await query
                .OrderByDescending(s => s.FechaSolicitud)
                .Select(s => new InscripcionDetalleModel
                {
                    Id = s.Id,
                    NumeroSolicitud = s.NumSolCompleta ?? "N/A",
                    FechaSolicitud = s.FechaSolicitud ?? DateTime.MinValue,
                    Tipo = s.EsRenovacion == true ? "Renovación" : "Primera Inscripción",
                    Estado = s.EstadoSolicitud.NombreEstado,
                    PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                    PacienteDocumento = !string.IsNullOrEmpty(s.Paciente.DocumentoCedula)
                        ? s.Paciente.DocumentoCedula
                        : s.Paciente.DocumentoPasaporte ?? "N/A",
                    PacienteCorreo = s.Paciente.CorreoElectronico ?? string.Empty,
                    PacienteTelefono = s.Paciente.TelefonoPersonal ?? string.Empty,
                    Region = s.Paciente.Region != null ? s.Paciente.Region.Nombre : "N/A",
                    Instalacion = s.Paciente.Instalacion != null ? s.Paciente.Instalacion.Nombre : "N/A",
                    FechaAprobacion = s.FechaAprobacion,
                    NumeroCarnet = s.NumeroCarnet,
                    FechaEmisionCarnet = s.FechaEmisionCarnet,
                    FechaVencimientoCarnet = s.FechaVencimientoCarnet,
                    CarnetActivo = s.CarnetActivo ?? false,
                    DiasParaVencimiento = s.FechaVencimientoCarnet.HasValue
                        ? (int)(s.FechaVencimientoCarnet.Value - DateTime.Now).TotalDays
                        : 0
                })
                .ToListAsync();

            return ResultModel<List<InscripcionDetalleModel>>.SuccessResult(
                inscripciones, 
                $"Se encontraron {inscripciones.Count} inscripciones");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo inscripciones detalladas");
            return ResultModel<List<InscripcionDetalleModel>>.ErrorResult(
                "Error al obtener las inscripciones", 
                new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<List<PacienteActivoInactivoModel>>> ObtenerPacientesActivosInactivosAsync(
        ReporteFiltrosModel filtros)
    {
        try
        {
            var query = _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .Include(s => s.Paciente.Region)
                .Where(s => s.EstadoSolicitud.NombreEstado == "Aprobada")
                .AsQueryable();

            query = AplicarFiltrosReporte(query, filtros);

            var pacientes = await query
                .Select(s => new PacienteActivoInactivoModel
                {
                    Id = s.PacienteId ?? 0,
                    PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                    PacienteDocumento = !string.IsNullOrEmpty(s.Paciente.DocumentoCedula)
                        ? s.Paciente.DocumentoCedula
                        : s.Paciente.DocumentoPasaporte ?? "N/A",
                    PacienteCorreo = s.Paciente.CorreoElectronico ?? string.Empty,
                    PacienteTelefono = s.Paciente.TelefonoPersonal ?? string.Empty,
                    Region = s.Paciente.Region != null ? s.Paciente.Region.Nombre : "N/A",
                    NumeroCarnet = s.NumeroCarnet ?? "N/A",
                    FechaEmisionCarnet = s.FechaEmisionCarnet ?? DateTime.MinValue,
                    FechaVencimientoCarnet = s.FechaVencimientoCarnet ?? DateTime.MinValue,
                    Estado = ObtenerEstadoPaciente(s.CarnetActivo, s.FechaVencimientoCarnet),
                    DiasDesdeVencimiento = s.FechaVencimientoCarnet.HasValue && s.FechaVencimientoCarnet < DateTime.Now
                        ? (int)(DateTime.Now - s.FechaVencimientoCarnet.Value).TotalDays
                        : 0,
                    FechaUltimaRenovacion = s.FechaUltimaRenovacion,
                    SolicitudId = s.Id,
                    NumeroSolicitud = s.NumSolCompleta ?? "N/A"
                })
                .OrderByDescending(p => p.FechaVencimientoCarnet)
                .ToListAsync();

            return ResultModel<List<PacienteActivoInactivoModel>>.SuccessResult(
                pacientes, 
                $"Se encontraron {pacientes.Count} pacientes");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo pacientes activos/inactivos");
            return ResultModel<List<PacienteActivoInactivoModel>>.ErrorResult(
                "Error al obtener los pacientes", 
                new List<string> { ex.Message });
        }
    }

    private IQueryable<TbSolRegCannabis> AplicarFiltrosReporte(
        IQueryable<TbSolRegCannabis> query, ReporteFiltrosModel filtros)
    {
        if (filtros == null) return query;

        if (!string.IsNullOrEmpty(filtros.Estado))
        {
            query = query.Where(s => s.EstadoSolicitud.NombreEstado == filtros.Estado);
        }

        if (filtros.FechaInicio.HasValue)
        {
            query = query.Where(s => s.FechaSolicitud >= filtros.FechaInicio);
        }

        if (filtros.FechaFin.HasValue)
        {
            query = query.Where(s => s.FechaSolicitud <= filtros.FechaFin);
        }

        if (filtros.ProvinciaId.HasValue)
        {
            query = query.Where(s => s.Paciente.ProvinciaId == filtros.ProvinciaId);
        }

        if (filtros.RegionSaludId.HasValue)
        {
            query = query.Where(s => s.Paciente.RegionId == filtros.RegionSaludId);
        }

        if (filtros.TipoInscripcion.HasValue)
        {
            query = query.Where(s => s.EsRenovacion == (filtros.TipoInscripcion == TipoInscripcion.Renovacion));
        }

        if (filtros.CarnetActivo.HasValue)
        {
            query = query.Where(s => s.CarnetActivo == filtros.CarnetActivo);
        }

        if (!string.IsNullOrEmpty(filtros.TerminoBusqueda))
        {
            var termino = filtros.TerminoBusqueda.ToLower();
            query = query.Where(s =>
                s.Paciente.PrimerNombre.ToLower().Contains(termino) ||
                s.Paciente.PrimerApellido.ToLower().Contains(termino) ||
                s.Paciente.DocumentoCedula.ToLower().Contains(termino) ||
                (s.Paciente.DocumentoPasaporte != null && s.Paciente.DocumentoPasaporte.ToLower().Contains(termino)) ||
                (s.NumSolCompleta != null && s.NumSolCompleta.ToLower().Contains(termino)) ||
                (s.NumeroCarnet != null && s.NumeroCarnet.ToLower().Contains(termino)));
        }

        return query;
    }

    private string ObtenerEstadoPaciente(bool? carnetActivo, DateTime? fechaVencimiento)
    {
        if (!carnetActivo.HasValue || carnetActivo.Value == false)
            return "Inactivo";

        if (!fechaVencimiento.HasValue)
            return "Activo (sin fecha vencimiento)";

        if (fechaVencimiento.Value < DateTime.Now)
            return "Vencido";

        return "Activo";
    }

    private string ObtenerTerminosBusqueda(ReporteFiltrosModel filtros)
    {
        if (filtros == null) return "Sin filtros";

        var terminos = new List<string>();

        if (filtros.FechaInicio.HasValue)
            terminos.Add($"Desde: {filtros.FechaInicio.Value:dd/MM/yyyy}");

        if (filtros.FechaFin.HasValue)
            terminos.Add($"Hasta: {filtros.FechaFin.Value:dd/MM/yyyy}");

        if (!string.IsNullOrEmpty(filtros.Estado))
            terminos.Add($"Estado: {filtros.Estado}");

        if (filtros.TipoInscripcion.HasValue)
            terminos.Add($"Tipo: {filtros.TipoInscripcion}");

        if (filtros.CarnetActivo.HasValue)
            terminos.Add($"Carnet: {(filtros.CarnetActivo.Value ? "Activo" : "Inactivo")}");

        if (!string.IsNullOrEmpty(filtros.TerminoBusqueda))
            terminos.Add($"Búsqueda: {filtros.TerminoBusqueda}");

        return string.Join(" | ", terminos);
    }
}