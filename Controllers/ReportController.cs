using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Consultor")]
public class ReportesController : ControllerBase
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<ReportesController> _logger;
    private readonly IReporteService _reporteService;

    public ReportesController(
        DbContextDigesa context,
        ILogger<ReportesController> logger,
        IReporteService reporteService)
    {
        _context = context;
        _logger = logger;
        _reporteService = reporteService;
    }

    [HttpGet("pacientes-activos-inactivos")]
    public async Task<IActionResult> ObtenerPacientesActivosInactivos([FromQuery] ReporteFiltrosModel filtros)
    {
        try
        {
            var query = _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .Where(s => s.EstadoSolicitud.NombreEstado == "Aprobada")
                .AsQueryable();

            if (filtros.CarnetActivo.HasValue)
            {
                query = query.Where(s => s.CarnetActivo == filtros.CarnetActivo.Value);
            }

            if (filtros.FechaInicio.HasValue)
            {
                query = query.Where(s => s.FechaAprobacion >= filtros.FechaInicio.Value);
            }

            if (filtros.FechaFin.HasValue)
            {
                query = query.Where(s => s.FechaAprobacion <= filtros.FechaFin.Value);
            }

            var resultados = await query
                .OrderBy(s => s.Paciente.PrimerNombre)
                .Select(s => new
                {
                    s.Id,
                    s.NumeroCarnet,
                    Paciente = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                    Documento = s.Paciente.DocumentoCedula ?? s.Paciente.DocumentoPasaporte ?? "N/A",
                    s.Paciente.CorreoElectronico,
                    s.Paciente.TelefonoPersonal,
                    s.Paciente.DireccionExacta,
                    s.FechaAprobacion,
                    s.FechaVencimientoCarnet,
                    EstadoCarnet = s.CarnetActivo == true ? "Activo" : "Inactivo",
                    DiasRestantes = s.FechaVencimientoCarnet.HasValue ?
                        (s.FechaVencimientoCarnet.Value - DateTime.Now).Days : 0,
                    s.EsRenovacion
                })
                .ToListAsync();

            return Ok(new { success = true, data = resultados });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo pacientes activos/inactivos");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("inscripciones-primera-vez-renovaciones")]
    public async Task<IActionResult> ObtenerInscripcionesPorTipo([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
    {
        try
        {
            var query = _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .Where(s => s.EstadoSolicitud.NombreEstado == "Aprobada")
                .AsQueryable();

            if (fechaInicio.HasValue)
            {
                query = query.Where(s => s.FechaAprobacion >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(s => s.FechaAprobacion <= fechaFin.Value);
            }

            var resultados = await query
                .GroupBy(s => s.EsRenovacion)
                .Select(g => new
                {
                    Tipo = g.Key == true ? "Renovación" : "Primera Vez",
                    Cantidad = g.Count(),
                    CarnetsActivos = g.Count(s => s.CarnetActivo == true),
                    CarnetsInactivos = g.Count(s => s.CarnetActivo == false),
                    PorVencer = g.Count(s => s.CarnetActivo == true &&
                                           s.FechaVencimientoCarnet.HasValue &&
                                           s.FechaVencimientoCarnet.Value <= DateTime.Now.AddDays(30) &&
                                           s.FechaVencimientoCarnet.Value > DateTime.Now)
                })
                .ToListAsync();

            return Ok(new { success = true, data = resultados });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo inscripciones por tipo");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("exportar-excel")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ExportarExcel([FromQuery] ReporteFiltrosModel filtros, [FromQuery] string tipoReporte = "solicitudes")
    {
        try
        {
            byte[] contenido;
            string nombreArchivo;

            switch (tipoReporte.ToLower())
            {
                case "solicitudes":
                    var resultadoSolicitudes = await _reporteService.GenerarReporteSolicitudesAsync(filtros, TipoExportacion.Excel);
                    if (!resultadoSolicitudes.Success)
                        return BadRequest(new { success = false, message = resultadoSolicitudes.Message });
                    
                    contenido = await _reporteService.ExportarSolicitudesExcelAsync(filtros);
                    nombreArchivo = $"Reporte_Solicitudes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    break;

                case "pacientes":
                    var resultadoPacientes = await _reporteService.GenerarReportePacientesAsync(filtros, TipoExportacion.Excel);
                    if (!resultadoPacientes.Success)
                        return BadRequest(new { success = false, message = resultadoPacientes.Message });
                    
                    contenido = await _reporteService.ExportarPacientesExcelAsync(filtros);
                    nombreArchivo = $"Reporte_Pacientes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    break;

                case "carnets":
                    var resultadoCarnets = await _reporteService.GenerarReporteCarnetsAsync(filtros, TipoExportacion.Excel);
                    if (!resultadoCarnets.Success)
                        return BadRequest(new { success = false, message = resultadoCarnets.Message });
                    
                    contenido = await _reporteService.ExportarCarnetsExcelAsync(filtros);
                    nombreArchivo = $"Reporte_Carnets_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    break;

                default:
                    return BadRequest(new { success = false, message = "Tipo de reporte no válido" });
            }

            return File(contenido, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando a Excel");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("exportar-pdf")]
    [Authorize(Roles = "Administrador,Consultor")]
    public async Task<IActionResult> ExportarPDF([FromQuery] ReporteFiltrosModel filtros)
    {
        try
        {
            var contenido = await _reporteService.ExportarPacientesPDFAsync(filtros);
            var nombreArchivo = $"Reporte_Pacientes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return File(contenido, "application/pdf", nombreArchivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando a PDF");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("dashboard-estadisticas")]
    public async Task<IActionResult> ObtenerEstadisticasDashboard()
    {
        try
        {
            var totalPacientes = await _context.TbSolRegCannabis
                .Where(s => s.EstadoSolicitud.NombreEstado == "Aprobada")
                .Select(s => s.PacienteId)
                .Distinct()
                .CountAsync();

            var pacientesActivos = await _context.TbSolRegCannabis
                .Where(s => s.EstadoSolicitud.NombreEstado == "Aprobada" &&
                           s.CarnetActivo == true &&
                           s.FechaVencimientoCarnet > DateTime.Now)
                .Select(s => s.PacienteId)
                .Distinct()
                .CountAsync();

            var nuevasInscripciones = await _context.TbSolRegCannabis
                .Where(s => s.EstadoSolicitud.NombreEstado == "Aprobada" &&
                           s.EsRenovacion == false &&
                           s.FechaAprobacion.Value.Month == DateTime.Now.Month &&
                           s.FechaAprobacion.Value.Year == DateTime.Now.Year)
                .CountAsync();

            var renovaciones = await _context.TbSolRegCannabis
                .Where(s => s.EstadoSolicitud.NombreEstado == "Aprobada" &&
                           s.EsRenovacion == true &&
                           s.FechaAprobacion.Value.Month == DateTime.Now.Month &&
                           s.FechaAprobacion.Value.Year == DateTime.Now.Year)
                .CountAsync();

            var porVencer = await _context.TbSolRegCannabis
                .Where(s => s.EstadoSolicitud.NombreEstado == "Aprobada" &&
                           s.CarnetActivo == true &&
                           s.FechaVencimientoCarnet.HasValue &&
                           s.FechaVencimientoCarnet.Value <= DateTime.Now.AddDays(30) &&
                           s.FechaVencimientoCarnet.Value > DateTime.Now)
                .CountAsync();

            var estadisticas = new
            {
                TotalPacientes,
                PacientesActivos,
                PacientesInactivos = totalPacientes - pacientesActivos,
                NuevasInscripciones,
                Renovaciones,
                PorVencer,
                PorcentajeActivos = totalPacientes > 0 ? Math.Round((pacientesActivos * 100.0) / totalPacientes, 2) : 0
            };

            return Ok(new { success = true, data = estadisticas });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo estadísticas del dashboard");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpGet("api-exportacion")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> ObtenerDatosAPI([FromQuery] ReporteFiltrosModel filtros, [FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 50)
    {
        try
        {
            var pagination = new PaginationModel { PageNumber = pagina, PageSize = tamanoPagina };
            var datos = await _reporteService.ObtenerSolicitudesAPIAsync(filtros, pagination);

            return Ok(new
            {
                success = true,
                data = datos,
                paginacion = new
                {
                    pagina,
                    tamanoPagina,
                    totalRegistros = datos.Count
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo datos para API");
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}