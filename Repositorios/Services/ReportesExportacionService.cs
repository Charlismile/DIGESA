using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Drawing;

namespace DIGESA.Repositorios.Services;

public class ReportesExportacionService : IReportesExportacionService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<ReportesExportacionService> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly IInscripcionesReporteService _inscripcionesReporteService;

    public ReportesExportacionService(
        DbContextDigesa context,
        ILogger<ReportesExportacionService> logger,
        IWebHostEnvironment environment,
        IInscripcionesReporteService inscripcionesReporteService)
    {
        _context = context;
        _logger = logger;
        _environment = environment;
        _inscripcionesReporteService = inscripcionesReporteService;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<ResultModel<byte[]>> ExportarPacientesActivosInactivosAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato)
    {
        try
        {
            var resultado = await _inscripcionesReporteService
                .ObtenerPacientesActivosInactivosAsync(filtros);

            if (!resultado.Exito || resultado.Datos == null || !resultado.Datos.Any())
                return ResultModel<byte[]>.ErrorResult("No hay datos para exportar");

            switch (formato)
            {
                case TipoExportacion.Excel:
                    return await ExportarPacientesActivosInactivosExcelAsync(resultado.Datos);
                case TipoExportacion.PDF:
                    return await ExportarPacientesActivosInactivosPDFAsync(resultado.Datos);
                case TipoExportacion.CSV:
                    return ExportarPacientesActivosInactivosCSV(resultado.Datos);
                default:
                    return ResultModel<byte[]>.ErrorResult("Formato no soportado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando pacientes activos/inactivos");
            return ResultModel<byte[]>.ErrorResult("Error en exportación", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<byte[]>> ExportarInscripcionesRenovacionesAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato)
    {
        try
        {
            var resultado = await _inscripcionesReporteService
                .ObtenerInscripcionesDetalladasAsync(filtros);

            if (!resultado.Exito || resultado.Datos == null || !resultado.Datos.Any())
                return ResultModel<byte[]>.ErrorResult("No hay datos para exportar");

            switch (formato)
            {
                case TipoExportacion.Excel:
                    return await ExportarInscripcionesRenovacionesExcelAsync(resultado.Datos);
                case TipoExportacion.PDF:
                    return await ExportarInscripcionesRenovacionesPDFAsync(resultado.Datos);
                case TipoExportacion.CSV:
                    return ExportarInscripcionesRenovacionesCSV(resultado.Datos);
                default:
                    return ResultModel<byte[]>.ErrorResult("Formato no soportado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando inscripciones/renovaciones");
            return ResultModel<byte[]>.ErrorResult("Error en exportación", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<byte[]>> ExportarDashboardEstadisticasAsync(
        DashboardEstadisticasModel dashboard, TipoExportacion formato)
    {
        try
        {
            switch (formato)
            {
                case TipoExportacion.Excel:
                    return await ExportarDashboardExcelAsync(dashboard);
                case TipoExportacion.PDF:
                    return await ExportarDashboardPDFAsync(dashboard);
                case TipoExportacion.CSV:
                    return ExportarDashboardCSV(dashboard);
                default:
                    return ResultModel<byte[]>.ErrorResult("Formato no soportado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando dashboard");
            return ResultModel<byte[]>.ErrorResult("Error en exportación", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<byte[]>> ExportarCarnetsProximosVencerAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato)
    {
        try
        {
            var carnets = await ObtenerCarnetsProximosVencerAsync(filtros);

            if (!carnets.Any())
                return ResultModel<byte[]>.ErrorResult("No hay datos para exportar");

            switch (formato)
            {
                case TipoExportacion.Excel:
                    return await ExportarCarnetsProximosVencerExcelAsync(carnets);
                case TipoExportacion.PDF:
                    return await ExportarCarnetsProximosVencerPDFAsync(carnets);
                case TipoExportacion.CSV:
                    return ExportarCarnetsProximosVencerCSV(carnets);
                default:
                    return ResultModel<byte[]>.ErrorResult("Formato no soportado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando carnets próximos a vencer");
            return ResultModel<byte[]>.ErrorResult("Error en exportación", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<byte[]>> ExportarRenovacionesPendientesAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato)
    {
        try
        {
            var renovaciones = await ObtenerRenovacionesPendientesAsync(filtros);

            if (!renovaciones.Any())
                return ResultModel<byte[]>.ErrorResult("No hay datos para exportar");

            switch (formato)
            {
                case TipoExportacion.Excel:
                    return await ExportarRenovacionesPendientesExcelAsync(renovaciones);
                case TipoExportacion.PDF:
                    return await ExportarRenovacionesPendientesPDFAsync(renovaciones);
                case TipoExportacion.CSV:
                    return ExportarRenovacionesPendientesCSV(renovaciones);
                default:
                    return ResultModel<byte[]>.ErrorResult("Formato no soportado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando renovaciones pendientes");
            return ResultModel<byte[]>.ErrorResult("Error en exportación", new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<byte[]>> ExportarDeclaracionesJuradasAsync(
        ReporteFiltrosModel filtros, TipoExportacion formato)
    {
        try
        {
            var declaraciones = await ObtenerDeclaracionesJuradasAsync(filtros);

            if (!declaraciones.Any())
                return ResultModel<byte[]>.ErrorResult("No hay datos para exportar");

            switch (formato)
            {
                case TipoExportacion.Excel:
                    return await ExportarDeclaracionesJuradasExcelAsync(declaraciones);
                case TipoExportacion.PDF:
                    return await ExportarDeclaracionesJuradasPDFAsync(declaraciones);
                case TipoExportacion.CSV:
                    return ExportarDeclaracionesJuradasCSV(declaraciones);
                default:
                    return ResultModel<byte[]>.ErrorResult("Formato no soportado");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando declaraciones juradas");
            return ResultModel<byte[]>.ErrorResult("Error en exportación", new List<string> { ex.Message });
        }
    }

    // ========== MÉTODOS PRIVADOS DE OBTENCIÓN DE DATOS ==========

    private async Task<List<CarnetProximoVencerModel>> ObtenerCarnetsProximosVencerAsync(ReporteFiltrosModel filtros)
    {
        var hoy = DateTime.Now;
        var fechaLimite = hoy.AddDays(30);

        var query = _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Where(s => s.CarnetActivo == true &&
                       s.FechaVencimientoCarnet.HasValue &&
                       s.FechaVencimientoCarnet.Value > hoy &&
                       s.FechaVencimientoCarnet.Value <= fechaLimite);

        // Aplicar filtros
        if (filtros != null)
        {
            query = AplicarFiltrosGenericos(query, filtros);
        }

        return await query
            .OrderBy(s => s.FechaVencimientoCarnet)
            .Select(s => new CarnetProximoVencerModel
            {
                NumeroCarnet = s.NumeroCarnet ?? "N/A",
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteDocumento = !string.IsNullOrEmpty(s.Paciente.DocumentoCedula)
                    ? s.Paciente.DocumentoCedula
                    : s.Paciente.DocumentoPasaporte ?? "N/A",
                PacienteCorreo = s.Paciente.CorreoElectronico ?? string.Empty,
                FechaVencimiento = s.FechaVencimientoCarnet.Value,
                DiasRestantes = (s.FechaVencimientoCarnet.Value - hoy).Days,
                Notificado30Dias = false, // Implementar lógica de notificaciones
                Notificado15Dias = false,
                Notificado7Dias = false,
                UltimaNotificacion = null
            })
            .ToListAsync();
    }

    private async Task<List<RenovacionPendienteReporteModel>> ObtenerRenovacionesPendientesAsync(ReporteFiltrosModel filtros)
    {
        var query = _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .Where(s => s.EsRenovacion == true &&
                       s.EstadoSolicitud.NombreEstado == "Pendiente");

        // Aplicar filtros
        if (filtros != null)
        {
            query = AplicarFiltrosGenericos(query, filtros);
        }

        return await query
            .OrderBy(s => s.FechaSolicitud)
            .Select(s => new RenovacionPendienteReporteModel
            {
                SolicitudId = s.Id,
                NumeroSolicitud = s.NumSolCompleta ?? "N/A",
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteDocumento = !string.IsNullOrEmpty(s.Paciente.DocumentoCedula)
                    ? s.Paciente.DocumentoCedula
                    : s.Paciente.DocumentoPasaporte ?? "N/A",
                FechaSolicitudRenovacion = s.FechaSolicitud ?? DateTime.MinValue,
                DiasDesdeVencimiento = 0, // Calcular según fecha vencimiento original
                Estado = s.EstadoSolicitud.NombreEstado,
                Comentario = s.ComentarioRevision ?? string.Empty
            })
            .ToListAsync();
    }

    private async Task<List<DeclaracionJuradaExportModel>> ObtenerDeclaracionesJuradasAsync(ReporteFiltrosModel filtros)
    {
        var query = _context.TbDeclaracionJurada
            .Include(d => d.SolRegCannabis)
                .ThenInclude(s => s.Paciente)
            .AsQueryable();

        // Aplicar filtros
        if (filtros != null)
        {
            if (filtros.FechaInicio.HasValue)
            {
                query = query.Where(d => d.Fecha >= DateOnly.FromDateTime(filtros.FechaInicio.Value));
            }
            if (filtros.FechaFin.HasValue)
            {
                query = query.Where(d => d.Fecha <= DateOnly.FromDateTime(filtros.FechaFin.Value));
            }
        }

        return await query
            .OrderByDescending(d => d.Fecha)
            .Select(d => new DeclaracionJuradaExportModel
            {
                Id = d.Id,
                SolicitudId = d.SolRegCannabisId ?? 0,
                NumeroSolicitud = d.SolRegCannabis.NumSolCompleta ?? "N/A",
                PacienteNombre = $"{d.SolRegCannabis.Paciente.PrimerNombre} {d.SolRegCannabis.Paciente.PrimerApellido}",
                PacienteDocumento = !string.IsNullOrEmpty(d.SolRegCannabis.Paciente.DocumentoCedula)
                    ? d.SolRegCannabis.Paciente.DocumentoCedula
                    : d.SolRegCannabis.Paciente.DocumentoPasaporte ?? "N/A",
                FechaDeclaracion = d.Fecha?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue,
                Aceptada = d.Aceptada ?? false,
                NombreDeclarante = d.NombreDeclarante ?? string.Empty
            })
            .ToListAsync();
    }

    private IQueryable<TbSolRegCannabis> AplicarFiltrosGenericos(IQueryable<TbSolRegCannabis> query, ReporteFiltrosModel filtros)
    {
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

        if (filtros.RegionSaludId.HasValue)
        {
            query = query.Where(s => s.Paciente.RegionId == filtros.RegionSaludId);
        }

        if (!string.IsNullOrEmpty(filtros.TerminoBusqueda))
        {
            var termino = filtros.TerminoBusqueda.ToLower();
            query = query.Where(s =>
                s.Paciente.PrimerNombre.ToLower().Contains(termino) ||
                s.Paciente.PrimerApellido.ToLower().Contains(termino) ||
                s.Paciente.DocumentoCedula.ToLower().Contains(termino) ||
                (s.Paciente.DocumentoPasaporte != null && s.Paciente.DocumentoPasaporte.ToLower().Contains(termino)));
        }

        return query;
    }

    // ========== MÉTODOS PRIVADOS DE EXPORTACIÓN ==========

    private async Task<byte[]> ExportarPacientesActivosInactivosExcelAsync(List<PacienteActivoInactivoModel> pacientes)
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Pacientes Activos-Inactivos");

        // Título
        worksheet.Cells[1, 1].Value = "Reporte de Pacientes Activos e Inactivos";
        worksheet.Cells[1, 1, 1, 9].Merge = true;
        worksheet.Cells[1, 1].Style.Font.Bold = true;
        worksheet.Cells[1, 1].Style.Font.Size = 14;
        worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        // Fecha generación
        worksheet.Cells[2, 1].Value = $"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}";
        worksheet.Cells[2, 1, 2, 9].Merge = true;
        worksheet.Cells[2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

        // Encabezados
        var headers = new[]
        {
            "Paciente", "Documento", "Correo", "Teléfono", "Región", "N° Carnet",
            "Fecha Emisión", "Fecha Vencimiento", "Estado", "Días desde Vencimiento"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[4, i + 1].Value = headers[i];
            worksheet.Cells[4, i + 1].Style.Font.Bold = true;
            worksheet.Cells[4, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[4, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            worksheet.Cells[4, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
        }

        // Datos
        for (int i = 0; i < pacientes.Count; i++)
        {
            var row = i + 5;
            var p = pacientes[i];

            worksheet.Cells[row, 1].Value = p.PacienteNombre;
            worksheet.Cells[row, 2].Value = p.PacienteDocumento;
            worksheet.Cells[row, 3].Value = p.PacienteCorreo;
            worksheet.Cells[row, 4].Value = p.PacienteTelefono;
            worksheet.Cells[row, 5].Value = p.Region;
            worksheet.Cells[row, 6].Value = p.NumeroCarnet;
            worksheet.Cells[row, 7].Value = p.FechaEmisionCarnet;
            worksheet.Cells[row, 7].Style.Numberformat.Format = "dd/mm/yyyy";
            worksheet.Cells[row, 8].Value = p.FechaVencimientoCarnet;
            worksheet.Cells[row, 8].Style.Numberformat.Format = "dd/mm/yyyy";
            worksheet.Cells[row, 9].Value = p.Estado;
            worksheet.Cells[row, 10].Value = p.DiasDesdeVencimiento;

            // Colorear según estado
            var estadoCell = worksheet.Cells[row, 9];
            if (p.Estado == "Activo")
                estadoCell.Style.Font.Color.SetColor(Color.Green);
            else if (p.Estado == "Vencido")
                estadoCell.Style.Font.Color.SetColor(Color.Orange);
            else if (p.Estado == "Inactivo")
                estadoCell.Style.Font.Color.SetColor(Color.Red);
        }

        // Autoajustar columnas
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        // Agregar totales
        var totalRow = pacientes.Count + 6;
        worksheet.Cells[totalRow, 1].Value = "TOTALES:";
        worksheet.Cells[totalRow, 1].Style.Font.Bold = true;
        worksheet.Cells[totalRow, 9].Value = pacientes.Count;
        worksheet.Cells[totalRow, 9].Style.Font.Bold = true;

        return await package.GetAsByteArrayAsync();
    }

    private async Task<byte[]> ExportarPacientesActivosInactivosPDFAsync(List<PacienteActivoInactivoModel> pacientes)
    {
        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        // Título
        var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
        var title = new Paragraph("Reporte de Pacientes Activos e Inactivos", titleFont);
        title.Alignment = Element.ALIGN_CENTER;
        title.SpacingAfter = 10;
        document.Add(title);

        // Subtítulo
        var subtitleFont = FontFactory.GetFont("Arial", 10);
        var subtitle = new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}", subtitleFont);
        subtitle.Alignment = Element.ALIGN_RIGHT;
        subtitle.SpacingAfter = 20;
        document.Add(subtitle);

        // Tabla
        var table = new PdfPTable(10);
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 2f, 1.5f, 2f, 1.5f, 1.5f, 1.5f, 1.2f, 1.2f, 1f, 1f });

        // Encabezados
        var headerFont = FontFactory.GetFont("Arial", 9, Font.BOLD);
        string[] headers = { "Paciente", "Documento", "Correo", "Teléfono", "Región", "N° Carnet", "Emisión", "Vencimiento", "Estado", "Días" };

        foreach (var header in headers)
        {
            table.AddCell(new PdfPCell(new Phrase(header, headerFont))
            {
                BackgroundColor = BaseColor.LIGHT_GRAY,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5
            });
        }

        // Datos
        var dataFont = FontFactory.GetFont("Arial", 8);
        foreach (var p in pacientes)
        {
            table.AddCell(new PdfPCell(new Phrase(p.PacienteNombre, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(p.PacienteDocumento, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(p.PacienteCorreo, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(p.PacienteTelefono, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(p.Region, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(p.NumeroCarnet, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(p.FechaEmisionCarnet.ToString("dd/MM/yyyy"), dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(p.FechaVencimientoCarnet.ToString("dd/MM/yyyy"), dataFont)) { Padding = 4 });
            
            var estadoCell = new PdfPCell(new Phrase(p.Estado, dataFont)) { Padding = 4 };
            if (p.Estado == "Activo")
                estadoCell.BackgroundColor = new BaseColor(220, 255, 220);
            else if (p.Estado == "Vencido")
                estadoCell.BackgroundColor = new BaseColor(255, 255, 200);
            else
                estadoCell.BackgroundColor = new BaseColor(255, 220, 220);
            table.AddCell(estadoCell);
            
            table.AddCell(new PdfPCell(new Phrase(p.DiasDesdeVencimiento.ToString(), dataFont)) { Padding = 4 });
        }

        document.Add(table);

        // Totales
        var totalFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
        var totales = new Paragraph($"\n\nTotal de pacientes: {pacientes.Count}", totalFont);
        totales.Alignment = Element.ALIGN_RIGHT;
        document.Add(totales);

        document.Close();
        return memoryStream.ToArray();
    }

    private byte[] ExportarPacientesActivosInactivosCSV(List<PacienteActivoInactivoModel> pacientes)
    {
        var csvBuilder = new StringBuilder();

        // Encabezados
        csvBuilder.AppendLine("Paciente,Documento,Correo,Teléfono,Región,Número Carnet,Fecha Emisión,Fecha Vencimiento,Estado,Días desde Vencimiento");

        // Datos
        foreach (var p in pacientes)
        {
            csvBuilder.AppendLine(
                $"{EscapeCsv(p.PacienteNombre)}," +
                $"{EscapeCsv(p.PacienteDocumento)}," +
                $"{EscapeCsv(p.PacienteCorreo)}," +
                $"{EscapeCsv(p.PacienteTelefono)}," +
                $"{EscapeCsv(p.Region)}," +
                $"{EscapeCsv(p.NumeroCarnet)}," +
                $"{p.FechaEmisionCarnet:dd/MM/yyyy}," +
                $"{p.FechaVencimientoCarnet:dd/MM/yyyy}," +
                $"{EscapeCsv(p.Estado)}," +
                $"{p.DiasDesdeVencimiento}"
            );
        }

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    // Métodos similares para los otros tipos de exportación...

    private string EscapeCsv(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        return value;
    }

    // Modelos internos para exportación
    private class DeclaracionJuradaExportModel
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public string NumeroSolicitud { get; set; } = string.Empty;
        public string PacienteNombre { get; set; } = string.Empty;
        public string PacienteDocumento { get; set; } = string.Empty;
        public DateTime FechaDeclaracion { get; set; }
        public bool Aceptada { get; set; }
        public string NombreDeclarante { get; set; } = string.Empty;
    }
}