using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class ExportacionService : IExportacionService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<ExportacionService> _logger;

    public ExportacionService(
        DbContextDigesa context,
        ILogger<ExportacionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ResultModel<byte[]>> ExportarUsuariosActivosInactivosAsync(TipoExportacion formato)
    {
        try
        {
            var usuarios = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .Where(s => s.CarnetActivo.HasValue)
                .GroupBy(s => s.CarnetActivo)
                .Select(g => new
                {
                    Estado = g.Key == true ? "Activo" : "Inactivo",
                    Cantidad = g.Count(),
                    Carnets = g.Select(s => new
                    {
                        NumeroCarnet = s.NumeroCarnet,
                        Paciente = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                        Documento = s.Paciente.DocumentoCedula ?? s.Paciente.DocumentoPasaporte,
                        FechaEmision = s.FechaEmisionCarnet,
                        FechaVencimiento = s.FechaVencimientoCarnet,
                        EstadoSolicitud = s.EstadoSolicitud.NombreEstado
                    }).ToList()
                })
                .ToListAsync();

            switch (formato)
            {
                case TipoExportacion.Excel:
                    return await ExportarExcelAsync(usuarios);
                case TipoExportacion.PDF:
                    return await ExportarPDFAsync(usuarios);
                case TipoExportacion.CSV:
                    return await ExportarCSVAsync(usuarios);
                default:
                    return ResultModel<byte[]>.ErrorResult("Formato no soportado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando usuarios activos/inactivos");
            return ResultModel<byte[]>.ErrorResult("Error en exportación", 
                new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<byte[]>> ExportarInscripcionesPorTipoAsync(TipoExportacion formato)
    {
        try
        {
            var inscripciones = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .GroupBy(s => new 
                { 
                    Tipo = s.EsRenovacion == true ? "Renovación" : "Primera Inscripción",
                    Anio = s.FechaSolicitud.Value.Year,
                    Mes = s.FechaSolicitud.Value.Month
                })
                .Select(g => new
                {
                    Tipo = g.Key.Tipo,
                    Periodo = $"{g.Key.Anio}-{g.Key.Mes:00}",
                    Cantidad = g.Count(),
                    Pacientes = g.Select(s => new
                    {
                        Nombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                        Documento = s.Paciente.DocumentoCedula ?? s.Paciente.DocumentoPasaporte,
                        FechaSolicitud = s.FechaSolicitud,
                        Estado = s.EstadoSolicitud.NombreEstado
                    }).ToList()
                })
                .OrderBy(x => x.Periodo)
                .ThenBy(x => x.Tipo)
                .ToListAsync();

            // Lógica de exportación similar...
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando inscripciones por tipo");
            return ResultModel<byte[]>.ErrorResult("Error en exportación", 
                new List<string> { ex.Message });
        }
    }
}