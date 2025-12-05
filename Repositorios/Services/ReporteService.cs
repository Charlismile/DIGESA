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

public class ReporteService : IReporteService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<ReporteService> _logger;
    private readonly IWebHostEnvironment _environment;

    public ReporteService(
        DbContextDigesa context,
        ILogger<ReporteService> logger,
        IWebHostEnvironment environment)
    {
        _context = context;
        _logger = logger;
        _environment = environment;
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<ResultModel<ReporteGeneradoModel>> GenerarReporteSolicitudesAsync(ReporteFiltrosModel filtros,
        TipoExportacion formato)
    {
        try
        {
            var solicitudes = await ObtenerSolicitudesParaReporteAsync(filtros);

            if (!solicitudes.Any())
                return ResultModel<ReporteGeneradoModel>.ErrorResult("No hay datos para generar el reporte");

            var nombreArchivo = $"Reporte_Solicitudes_{DateTime.Now:yyyyMMdd_HHmmss}";
            var rutaArchivo = string.Empty;
            byte[] contenido = Array.Empty<byte>();

            switch (formato)
            {
                case TipoExportacion.Excel:
                    contenido = await ExportarSolicitudesExcelAsync(filtros);
                    nombreArchivo += ".xlsx";
                    break;

                case TipoExportacion.PDF:
                    contenido = await ExportarSolicitudesPDFAsync(solicitudes);
                    nombreArchivo += ".pdf";
                    break;

                case TipoExportacion.CSV:
                    contenido = ExportarSolicitudesCSV(solicitudes);
                    nombreArchivo += ".csv";
                    break;
            }

            if (contenido.Length == 0)
                return ResultModel<ReporteGeneradoModel>.ErrorResult("Error al generar el contenido del reporte");

            // Guardar archivo en disco
            rutaArchivo = await GuardarArchivoReporteAsync(contenido, nombreArchivo);

            // Registrar en base de datos
            var reporte = await RegistrarReporteGeneradoAsync(
                nombreArchivo,
                "Solicitudes",
                formato,
                filtros,
                rutaArchivo,
                contenido.Length
            );

            return ResultModel<ReporteGeneradoModel>.SuccessResult(reporte, "Reporte generado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando reporte de solicitudes");
            return ResultModel<ReporteGeneradoModel>.ErrorResult("Error al generar el reporte",
                new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<ReporteGeneradoModel>> GenerarReportePacientesAsync(ReporteFiltrosModel filtros,
        TipoExportacion formato)
    {
        try
        {
            var pacientes = await ObtenerPacientesParaReporteAsync(filtros);

            if (!pacientes.Any())
                return ResultModel<ReporteGeneradoModel>.ErrorResult("No hay datos para generar el reporte");

            var nombreArchivo = $"Reporte_Pacientes_{DateTime.Now:yyyyMMdd_HHmmss}";
            var rutaArchivo = string.Empty;
            byte[] contenido = Array.Empty<byte>();

            switch (formato)
            {
                case TipoExportacion.Excel:
                    contenido = await ExportarPacientesExcelAsync(pacientes, filtros);
                    nombreArchivo += ".xlsx";
                    break;

                case TipoExportacion.PDF:
                    contenido = await ExportarPacientesPDFAsync(pacientes);
                    nombreArchivo += ".pdf";
                    break;

                case TipoExportacion.CSV:
                    contenido = ExportarPacientesCSV(pacientes);
                    nombreArchivo += ".csv";
                    break;
            }

            if (contenido.Length == 0)
                return ResultModel<ReporteGeneradoModel>.ErrorResult("Error al generar el contenido del reporte");

            // Guardar archivo en disco
            rutaArchivo = await GuardarArchivoReporteAsync(contenido, nombreArchivo);

            // Registrar en base de datos
            var reporte = await RegistrarReporteGeneradoAsync(
                nombreArchivo,
                "Pacientes",
                formato,
                filtros,
                rutaArchivo,
                contenido.Length
            );

            return ResultModel<ReporteGeneradoModel>.SuccessResult(reporte, "Reporte generado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando reporte de pacientes");
            return ResultModel<ReporteGeneradoModel>.ErrorResult("Error al generar el reporte",
                new List<string> { ex.Message });
        }
    }


    public async Task<ResultModel<ReporteGeneradoModel>> GenerarReporteCarnetsAsync(ReporteFiltrosModel filtros,
        TipoExportacion formato)
    {
        try
        {
            var carnets = await ObtenerCarnetsParaReporteAsync(filtros);

            if (!carnets.Any())
                return ResultModel<ReporteGeneradoModel>.ErrorResult("No hay datos para generar el reporte");

            var nombreArchivo = $"Reporte_Carnets_{DateTime.Now:yyyyMMdd_HHmmss}";
            var rutaArchivo = string.Empty;
            byte[] contenido = Array.Empty<byte>();

            switch (formato)
            {
                case TipoExportacion.Excel:
                    contenido = await ExportarCarnetsExcelAsync(carnets);
                    nombreArchivo += ".xlsx";
                    break;

                case TipoExportacion.PDF:
                    contenido = await ExportarCarnetsPDFAsync(carnets);
                    nombreArchivo += ".pdf";
                    break;

                case TipoExportacion.CSV:
                    contenido = ExportarCarnetsCSV(carnets);
                    nombreArchivo += ".csv";
                    break;
            }

            if (contenido.Length == 0)
                return ResultModel<ReporteGeneradoModel>.ErrorResult("Error al generar el contenido del reporte");

            // Guardar archivo en disco
            rutaArchivo = await GuardarArchivoReporteAsync(contenido, nombreArchivo);

            // Registrar en base de datos
            var reporte = await RegistrarReporteGeneradoAsync(
                nombreArchivo,
                "Carnets",
                formato,
                filtros,
                rutaArchivo,
                contenido.Length
            );

            return ResultModel<ReporteGeneradoModel>.SuccessResult(reporte, "Reporte generado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando reporte de carnets");
            return ResultModel<ReporteGeneradoModel>.ErrorResult("Error al generar el reporte",
                new List<string> { ex.Message });
        }
    }

    // Método ExportarCarnetsCSV simplificado
    public async Task<byte[]> ExportarCarnetsCSVAsync(ReporteFiltrosModel filtros)
    {
        var carnets = await ObtenerCarnetsParaReporteAsync(filtros);
        return ExportarCarnetsCSV(carnets);
    }

    private byte[] ExportarCarnetsCSV(List<CarnetReporteModel> carnets)
    {
        var csvBuilder = new StringBuilder();

        // Encabezados
        csvBuilder.AppendLine(
            "Número Carnet,Paciente,Documento,Correo,Teléfono,Fecha Emisión,Fecha Vencimiento,Días Restantes,Estado,Región,Instalación");

        // Datos
        foreach (var carnet in carnets)
        {
            var diasRestantes = carnet.FechaVencimiento.HasValue
                ? (carnet.FechaVencimiento.Value - DateTime.Now).Days
                : 0;

            csvBuilder.AppendLine(
                $"{carnet.NumeroCarnet}," +
                $"{carnet.PacienteNombre}," +
                $"{carnet.PacienteDocumento}," +
                $"{carnet.PacienteCorreo}," +
                $"{carnet.PacienteTelefono}," +
                $"{carnet.FechaEmision:dd/MM/yyyy}," +
                $"{carnet.FechaVencimiento:dd/MM/yyyy}," +
                $"{diasRestantes}," +
                $"{(carnet.Activo ? "Activo" : "Inactivo")}," +
                $"{carnet.Region}," +
                $"{carnet.Instalacion}"
            );
        }

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    public async Task<List<ReporteGeneradoModel>> ObtenerReportesGeneradosAsync(string usuarioId, int dias = 30)
    {
        try
        {
            var fechaLimite = DateTime.Now.AddDays(-dias);

            return await _context.TbReporteGenerado
                .Where(r => r.GeneradoPor == usuarioId && r.FechaGeneracion >= fechaLimite)
                .OrderByDescending(r => r.FechaGeneracion)
                .Select(r => new ReporteGeneradoModel
                {
                    Id = r.Id,
                    NombreArchivo = r.NombreArchivo,
                    TipoReporte = r.TipoReporte,
                    Formato = ObtenerFormatoDesdeTipo(r.TipoReporte),
                    FechaGeneracion = r.FechaGeneracion ?? DateTime.MinValue,
                    GeneradoPor = r.GeneradoPor ?? string.Empty,
                    FiltrosAplicados = r.FiltrosAplicados ?? string.Empty,
                    RutaArchivo = r.RutaArchivo ?? string.Empty,
                    TamanoBytes = r.TamanoBytes ?? 0,
                    Descargado = r.Descargado ?? false
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo reportes para usuario {usuarioId}");
            return new List<ReporteGeneradoModel>();
        }
    }

    private TipoExportacion ObtenerFormatoDesdeTipo(string tipoReporte)
    {
        if (tipoReporte.Contains("Excel", StringComparison.OrdinalIgnoreCase))
            return TipoExportacion.Excel;
        if (tipoReporte.Contains("PDF", StringComparison.OrdinalIgnoreCase))
            return TipoExportacion.PDF;
        if (tipoReporte.Contains("CSV", StringComparison.OrdinalIgnoreCase))
            return TipoExportacion.CSV;
        return TipoExportacion.Excel;
    }

    public async Task<ResultModel<bool>> EliminarReporteAsync(int reporteId)
    {
        try
        {
            var reporte = await _context.TbReporteGenerado
                .FirstOrDefaultAsync(r => r.Id == reporteId);

            if (reporte == null)
                return ResultModel<bool>.ErrorResult("Reporte no encontrado");

            // Eliminar archivo físico si existe
            if (!string.IsNullOrEmpty(reporte.RutaArchivo))
            {
                var filePath = Path.Combine(_environment.WebRootPath, reporte.RutaArchivo.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }

            // Eliminar registro de la base de datos
            _context.TbReporteGenerado.Remove(reporte);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Reporte {reporteId} eliminado");
            return ResultModel<bool>.SuccessResult(true, "Reporte eliminado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error eliminando reporte {reporteId}");
            return ResultModel<bool>.ErrorResult("Error al eliminar el reporte", new List<string> { ex.Message });
        }
    }

    public async Task<byte[]> ExportarSolicitudesExcelAsync(ReporteFiltrosModel filtros)
    {
        var solicitudes = await ObtenerSolicitudesParaReporteAsync(filtros);

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Solicitudes");

        // Encabezados
        var headers = new[]
        {
            "ID", "Número Solicitud", "Fecha Solicitud", "Estado", "Paciente", "Documento",
            "Correo", "Teléfono", "Región", "Instalación", "Es Renovación",
            "Fecha Aprobación", "Carnet", "Estado Carnet", "Vencimiento"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = headers[i];
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        }

        // Datos
        for (int i = 0; i < solicitudes.Count; i++)
        {
            var row = i + 2;
            var solicitud = solicitudes[i];

            worksheet.Cells[row, 1].Value = solicitud.Id;
            worksheet.Cells[row, 2].Value = solicitud.NumeroSolicitud;
            worksheet.Cells[row, 3].Value = solicitud.FechaSolicitud;
            worksheet.Cells[row, 3].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
            worksheet.Cells[row, 4].Value = solicitud.Estado;
            worksheet.Cells[row, 5].Value = solicitud.PacienteNombre;
            worksheet.Cells[row, 6].Value = solicitud.PacienteDocumento;
            worksheet.Cells[row, 7].Value = solicitud.PacienteCorreo;
            worksheet.Cells[row, 8].Value = solicitud.PacienteTelefono;
            worksheet.Cells[row, 9].Value = solicitud.Region;
            worksheet.Cells[row, 10].Value = solicitud.Instalacion;
            worksheet.Cells[row, 11].Value = solicitud.EsRenovacion ? "Sí" : "No";
            worksheet.Cells[row, 12].Value = solicitud.FechaAprobacion;
            if (solicitud.FechaAprobacion.HasValue)
                worksheet.Cells[row, 12].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
            worksheet.Cells[row, 13].Value = solicitud.NumeroCarnet;
            worksheet.Cells[row, 14].Value = solicitud.CarnetActivo ? "Activo" : "Inactivo";
            worksheet.Cells[row, 15].Value = solicitud.FechaVencimiento;
            if (solicitud.FechaVencimiento.HasValue)
                worksheet.Cells[row, 15].Style.Numberformat.Format = "dd/mm/yyyy";
        }

        // Autoajustar columnas
        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        // Agregar filtros
        worksheet.Cells[1, 1, 1, headers.Length].AutoFilter = true;

        return await package.GetAsByteArrayAsync();
    }

    public async Task<byte[]> ExportarPacientesPDFAsync(ReporteFiltrosModel filtros)
    {
        var pacientes = await ObtenerPacientesParaReporteAsync(filtros);

        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        // Título
        var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
        var title = new Paragraph("Reporte de Pacientes - DIGESA Cannabis Medicinal", titleFont);
        title.Alignment = Element.ALIGN_CENTER;
        title.SpacingAfter = 20;
        document.Add(title);

        // Fecha de generación
        var dateFont = FontFactory.GetFont("Arial", 10);
        var dateText = new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}", dateFont);
        dateText.Alignment = Element.ALIGN_RIGHT;
        dateText.SpacingAfter = 10;
        document.Add(dateText);

        // Tabla
        var table = new PdfPTable(10);
        table.WidthPercentage = 100;
        table.SetWidths(new float[] { 0.5f, 1.5f, 2f, 1.5f, 2f, 1.5f, 2f, 1.5f, 1.5f, 1.5f });

        // Encabezados de tabla
        var headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
        string[] headers =
        {
            "ID", "Documento", "Nombre", "Correo", "Teléfono", "Fecha Nac.", "Sexo", "Región", "Instalación", "Estado"
        };

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
        var dataFont = FontFactory.GetFont("Arial", 9);
        foreach (var paciente in pacientes)
        {
            table.AddCell(new PdfPCell(new Phrase(paciente.Id.ToString(), dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.Documento, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.NombreCompleto, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.Correo, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.Telefono, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.FechaNacimiento?.ToString("dd/MM/yyyy") ?? "", dataFont))
                { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.Sexo, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.Region, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.Instalacion, dataFont)) { Padding = 4 });
            table.AddCell(new PdfPCell(new Phrase(paciente.Estado, dataFont)) { Padding = 4 });
        }

        document.Add(table);

        // Pie de página
        var footer = new Paragraph($"Total de registros: {pacientes.Count}", dateFont);
        footer.Alignment = Element.ALIGN_RIGHT;
        footer.SpacingBefore = 20;
        document.Add(footer);

        document.Close();

        return memoryStream.ToArray();
    }

    public async Task<List<ApiSolicitudModel>> ObtenerSolicitudesAPIAsync(ReporteFiltrosModel filtros,
        PaginationModel pagination)
    {
        try
        {
            var query = _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .AsQueryable();

            // Aplicar filtros
            query = AplicarFiltrosSolicitudes(query, filtros);

            // Paginación
            var totalRegistros = await query.CountAsync();
            var solicitudes = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var resultado = new List<ApiSolicitudModel>();

            foreach (var s in solicitudes)
            {
                // Obtener productos asociados
                var productos = await _context.TbNombreProductoPaciente
                    .Where(p => p.PacienteId == s.PacienteId)
                    .ToListAsync();

                resultado.Add(new ApiSolicitudModel
                {
                    NumeroSolicitud = s.NumSolCompleta ?? "N/A",
                    FechaSolicitud = s.FechaSolicitud ?? DateTime.MinValue,
                    Estado = s.EstadoSolicitud?.NombreEstado ?? "Desconocido",
                    EsRenovacion = s.EsRenovacion ?? false,
                    Paciente = new PacienteApiModel
                    {
                        NombreCompleto = $"{s.Paciente?.PrimerNombre} {s.Paciente?.PrimerApellido}",
                        NumeroDocumento = !string.IsNullOrEmpty(s.Paciente?.DocumentoCedula)
                            ? s.Paciente.DocumentoCedula
                            : s.Paciente?.DocumentoPasaporte ?? "N/A",
                        TipoDocumento = s.Paciente?.TipoDocumento ?? "N/A",
                        CorreoElectronico = s.Paciente?.CorreoElectronico ?? string.Empty,
                        TelefonoPersonal = s.Paciente?.TelefonoPersonal ?? string.Empty,
                        RegionSalud = s.Paciente?.Region?.Nombre ?? "N/A",
                        InstalacionSalud = s.Paciente?.Instalacion?.Nombre ?? "N/A"
                    },
                    Productos = productos.Select(p => new ProductoApiModel
                    {
                        NombreComercial = p.NombreComercialProd ?? p.NombreProducto ?? "N/A",
                        FormaFarmaceutica = p.FormaFarmaceutica ?? "N/A",
                        ViaAdministracion = p.ViaConsumoProducto ?? "N/A",
                        Concentracion = $"{p.CantidadConcentracion} {p.NombreConcentracion}",
                        DetalleDosis = p.DetDosisPaciente ?? "N/A"
                    }).ToList(),
                    FechaEmisionCarnet = s.FechaEmisionCarnet,
                    FechaVencimientoCarnet = s.FechaVencimientoCarnet,
                    NumeroCarnet = s.NumeroCarnet,
                    CarnetActivo = s.CarnetActivo ?? false
                });
            }

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo solicitudes para API");
            return new List<ApiSolicitudModel>();
        }
    }

    // ==================== MÉTODOS PRIVADOS AUXILIARES ====================

    private async Task<List<SolicitudReporteModel>> ObtenerSolicitudesParaReporteAsync(ReporteFiltrosModel filtros)
    {
        var query = _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .Include(s => s.Paciente.Region)
            .Include(s => s.Paciente.Instalacion)
            .AsQueryable();

        query = AplicarFiltrosSolicitudes(query, filtros);

        return await query
            .Select(s => new SolicitudReporteModel
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
                PacienteTelefono = s.Paciente.TelefonoPersonal ?? string.Empty,
                Region = s.Paciente.Region != null ? s.Paciente.Region.Nombre : "N/A",
                Instalacion = s.Paciente.Instalacion != null ? s.Paciente.Instalacion.Nombre : "N/A",
                EsRenovacion = s.EsRenovacion ?? false,
                FechaAprobacion = s.FechaAprobacion,
                NumeroCarnet = s.NumeroCarnet,
                CarnetActivo = s.CarnetActivo ?? false,
                FechaVencimiento = s.FechaVencimientoCarnet
            })
            .ToListAsync();
    }

    private async Task<List<PacienteReporteModel>> ObtenerPacientesParaReporteAsync(ReporteFiltrosModel filtros)
    {
        var query = _context.TbPaciente
            .Include(p => p.Region)
            .Include(p => p.Instalacion)
            .AsQueryable();

        // Aplicar filtros
        if (filtros.Sexo.HasValue)
        {
            query = query.Where(p => p.Sexo == filtros.Sexo.Value.ToString());
        }

        if (!string.IsNullOrEmpty(filtros.TipoDocumento))
        {
            query = query.Where(p => p.TipoDocumento == filtros.TipoDocumento);
        }

        if (filtros.EdadMin.HasValue || filtros.EdadMax.HasValue)
        {
            var hoy = DateTime.Today;
            if (filtros.EdadMin.HasValue)
            {
                var fechaMaxNacimiento = hoy.AddYears(-filtros.EdadMin.Value);
                query = query.Where(p => p.FechaNacimiento <= DateOnly.FromDateTime(fechaMaxNacimiento));
            }

            if (filtros.EdadMax.HasValue)
            {
                var fechaMinNacimiento = hoy.AddYears(-filtros.EdadMax.Value - 1);
                query = query.Where(p => p.FechaNacimiento >= DateOnly.FromDateTime(fechaMinNacimiento));
            }
        }

        if (!string.IsNullOrEmpty(filtros.TerminoBusqueda))
        {
            var termino = filtros.TerminoBusqueda.ToLower();
            query = query.Where(p =>
                p.PrimerNombre.ToLower().Contains(termino) ||
                p.PrimerApellido.ToLower().Contains(termino) ||
                p.DocumentoCedula.ToLower().Contains(termino) ||
                (p.DocumentoPasaporte != null && p.DocumentoPasaporte.ToLower().Contains(termino)));
        }

        return await query
            .Select(p => new PacienteReporteModel
            {
                Id = p.Id,
                Documento =
                    !string.IsNullOrEmpty(p.DocumentoCedula) ? p.DocumentoCedula : p.DocumentoPasaporte ?? "N/A",
                NombreCompleto = $"{p.PrimerNombre} {p.PrimerApellido}",
                Correo = p.CorreoElectronico ?? string.Empty,
                Telefono = p.TelefonoPersonal ?? string.Empty,
                FechaNacimiento = p.FechaNacimiento.HasValue
                    ? p.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue)
                    : (DateTime?)null,
                Sexo = p.Sexo ?? "N/A",
                Region = p.Region != null ? p.Region.Nombre : "N/A",
                Instalacion = p.Instalacion != null ? p.Instalacion.Nombre : "N/A",
                Estado = "Activo"
            })
            .ToListAsync();
    }

    private async Task<List<CarnetReporteModel>> ObtenerCarnetsParaReporteAsync(ReporteFiltrosModel filtros)
    {
        var query = _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.Paciente.Region)
            .Include(s => s.Paciente.Instalacion)
            .Where(s => !string.IsNullOrEmpty(s.NumeroCarnet))
            .AsQueryable();

        // Aplicar filtros
        if (filtros.CarnetActivo.HasValue)
        {
            query = query.Where(s => s.CarnetActivo == filtros.CarnetActivo);
        }

        if (filtros.FechaInicio.HasValue)
        {
            query = query.Where(s => s.FechaEmisionCarnet >= filtros.FechaInicio);
        }

        if (filtros.FechaFin.HasValue)
        {
            query = query.Where(s => s.FechaEmisionCarnet <= filtros.FechaFin);
        }

        if (!string.IsNullOrEmpty(filtros.Estado))
        {
            query = query.Where(s => s.CarnetActivo == (filtros.Estado.ToLower() == "activo"));
        }

        return await query
            .Select(s => new CarnetReporteModel
            {
                NumeroCarnet = s.NumeroCarnet ?? "N/A",
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteDocumento = !string.IsNullOrEmpty(s.Paciente.DocumentoCedula)
                    ? s.Paciente.DocumentoCedula
                    : s.Paciente.DocumentoPasaporte ?? "N/A",
                PacienteCorreo = s.Paciente.CorreoElectronico ?? string.Empty,
                PacienteTelefono = s.Paciente.TelefonoPersonal ?? string.Empty,
                FechaEmision = s.FechaEmisionCarnet ?? DateTime.MinValue,
                FechaVencimiento = s.FechaVencimientoCarnet,
                Activo = s.CarnetActivo ?? false,
                Region = s.Paciente.Region != null ? s.Paciente.Region.Nombre : "N/A",
                Instalacion = s.Paciente.Instalacion != null ? s.Paciente.Instalacion.Nombre : "N/A"
            })
            .ToListAsync();
    }

    private IQueryable<TbSolRegCannabis> AplicarFiltrosSolicitudes(IQueryable<TbSolRegCannabis> query,
        ReporteFiltrosModel filtros)
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

    private async Task<string> GuardarArchivoReporteAsync(byte[] contenido, string nombreArchivo)
    {
        var reportsDirectory = Path.Combine(_environment.WebRootPath, "reports");

        if (!Directory.Exists(reportsDirectory))
            Directory.CreateDirectory(reportsDirectory);

        var filePath = Path.Combine(reportsDirectory, nombreArchivo);
        await File.WriteAllBytesAsync(filePath, contenido);

        return $"/reports/{nombreArchivo}";
    }

    private async Task<ReporteGeneradoModel> RegistrarReporteGeneradoAsync(
        string nombreArchivo,
        string tipoReporte,
        TipoExportacion formato,
        ReporteFiltrosModel filtros,
        string rutaArchivo,
        long tamanoBytes)
    {
        var reporte = new TbReporteGenerado
        {
            NombreArchivo = nombreArchivo,
            TipoReporte = $"{tipoReporte} ({formato})",
            FechaGeneracion = DateTime.Now,
            GeneradoPor = "Sistema",
            FiltrosAplicados = SerializarFiltros(filtros),
            RutaArchivo = rutaArchivo,
            TamanoBytes = tamanoBytes,
            Descargado = false
        };

        _context.TbReporteGenerado.Add(reporte);
        await _context.SaveChangesAsync();

        return new ReporteGeneradoModel
        {
            Id = reporte.Id,
            NombreArchivo = reporte.NombreArchivo,
            TipoReporte = reporte.TipoReporte,
            Formato = formato,
            FechaGeneracion = reporte.FechaGeneracion ?? DateTime.MinValue,
            GeneradoPor = reporte.GeneradoPor ?? string.Empty,
            FiltrosAplicados = reporte.FiltrosAplicados ?? string.Empty,
            RutaArchivo = reporte.RutaArchivo ?? string.Empty,
            TamanoBytes = reporte.TamanoBytes ?? 0,
            Descargado = reporte.Descargado ?? false
        };
    }

    private string SerializarFiltros(ReporteFiltrosModel filtros)
    {
        if (filtros == null)
            return "Sin filtros";

        var filtrosList = new List<string>();

        if (filtros.FechaInicio.HasValue)
            filtrosList.Add($"Desde: {filtros.FechaInicio.Value:dd/MM/yyyy}");

        if (filtros.FechaFin.HasValue)
            filtrosList.Add($"Hasta: {filtros.FechaFin.Value:dd/MM/yyyy}");

        if (!string.IsNullOrEmpty(filtros.Estado))
            filtrosList.Add($"Estado: {filtros.Estado}");

        if (filtros.TipoInscripcion.HasValue)
            filtrosList.Add($"Tipo: {filtros.TipoInscripcion}");

        if (filtros.CarnetActivo.HasValue)
            filtrosList.Add($"Carnet Activo: {(filtros.CarnetActivo.Value ? "Sí" : "No")}");

        if (!string.IsNullOrEmpty(filtros.TerminoBusqueda))
            filtrosList.Add($"Búsqueda: {filtros.TerminoBusqueda}");

        return string.Join(" | ", filtrosList);
    }

    // Métodos adicionales simplificados
    private async Task<byte[]> ExportarSolicitudesPDFAsync(List<SolicitudReporteModel> solicitudes)
    {
        using var memoryStream = new MemoryStream();
        var document = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
        var writer = PdfWriter.GetInstance(document, memoryStream);

        document.Open();

        var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
        var title = new Paragraph("Reporte de Solicitudes - DIGESA Cannabis Medicinal", titleFont);
        title.Alignment = Element.ALIGN_CENTER;
        document.Add(title);

        document.Close();

        return memoryStream.ToArray();
    }

    private byte[] ExportarSolicitudesCSV(List<SolicitudReporteModel> solicitudes)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine(
            "ID,Número Solicitud,Fecha Solicitud,Estado,Paciente,Documento,Correo,Teléfono,Región,Instalación,Es Renovación,Fecha Aprobación,Carnet,Estado Carnet,Vencimiento");

        foreach (var s in solicitudes)
        {
            csvBuilder.AppendLine(
                $"{s.Id},{s.NumeroSolicitud},{s.FechaSolicitud:dd/MM/yyyy HH:mm},{s.Estado}," +
                $"{s.PacienteNombre},{s.PacienteDocumento},{s.PacienteCorreo},{s.PacienteTelefono}," +
                $"{s.Region},{s.Instalacion},{(s.EsRenovacion ? "Sí" : "No")}," +
                $"{(s.FechaAprobacion.HasValue ? s.FechaAprobacion.Value.ToString("dd/MM/yyyy HH:mm") : "")}," +
                $"{s.NumeroCarnet},{(s.CarnetActivo ? "Activo" : "Inactivo")}," +
                $"{(s.FechaVencimiento.HasValue ? s.FechaVencimiento.Value.ToString("dd/MM/yyyy") : "")}"
            );
        }

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    private async Task<byte[]> ExportarPacientesExcelAsync(List<PacienteReporteModel> pacientes)
    {
        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Pacientes");

        var headers = new[]
        {
            "ID", "Documento", "Nombre", "Correo", "Teléfono", "Fecha Nac.", "Sexo", "Región", "Instalación", "Estado"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cells[1, i + 1].Value = headers[i];
            worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
        }

        for (int i = 0; i < pacientes.Count; i++)
        {
            var row = i + 2;
            var p = pacientes[i];

            worksheet.Cells[row, 1].Value = p.Id;
            worksheet.Cells[row, 2].Value = p.Documento;
            worksheet.Cells[row, 3].Value = p.NombreCompleto;
            worksheet.Cells[row, 4].Value = p.Correo;
            worksheet.Cells[row, 5].Value = p.Telefono;
            worksheet.Cells[row, 6].Value = p.FechaNacimiento;
            if (p.FechaNacimiento.HasValue)
                worksheet.Cells[row, 6].Style.Numberformat.Format = "dd/mm/yyyy";
            worksheet.Cells[row, 7].Value = p.Sexo;
            worksheet.Cells[row, 8].Value = p.Region;
            worksheet.Cells[row, 9].Value = p.Instalacion;
            worksheet.Cells[row, 10].Value = p.Estado;
        }

        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
        return await package.GetAsByteArrayAsync();
    }

    private byte[] ExportarPacientesCSV(List<PacienteReporteModel> pacientes)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("ID,Documento,Nombre,Correo,Teléfono,Fecha Nacimiento,Sexo,Región,Instalación,Estado");

        foreach (var p in pacientes)
        {
            csvBuilder.AppendLine(
                $"{p.Id},{p.Documento},{p.NombreCompleto},{p.Correo},{p.Telefono}," +
                $"{(p.FechaNacimiento.HasValue ? p.FechaNacimiento.Value.ToString("dd/MM/yyyy") : "")}," +
                $"{p.Sexo},{p.Region},{p.Instalacion},{p.Estado}"
            );
        }

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    private async Task<byte[]> ExportarPacientesExcelAsync(List<PacienteReporteModel> pacientes, ReporteFiltrosModel filtros)
{
    using var package = new ExcelPackage();
    var worksheet = package.Workbook.Worksheets.Add("Pacientes");

    var headers = new[] { "ID", "Documento", "Nombre", "Correo", "Teléfono", "Fecha Nac.", "Sexo", "Región", "Instalación", "Estado" };

    for (int i = 0; i < headers.Length; i++)
    {
        worksheet.Cells[1, i + 1].Value = headers[i];
        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
        worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
    }

    for (int i = 0; i < pacientes.Count; i++)
    {
        var row = i + 2;
        var p = pacientes[i];

        worksheet.Cells[row, 1].Value = p.Id;
        worksheet.Cells[row, 2].Value = p.Documento;
        worksheet.Cells[row, 3].Value = p.NombreCompleto;
        worksheet.Cells[row, 4].Value = p.Correo;
        worksheet.Cells[row, 5].Value = p.Telefono;
        worksheet.Cells[row, 6].Value = p.FechaNacimiento;
        if (p.FechaNacimiento.HasValue)
            worksheet.Cells[row, 6].Style.Numberformat.Format = "dd/mm/yyyy";
        worksheet.Cells[row, 7].Value = p.Sexo;
        worksheet.Cells[row, 8].Value = p.Region;
        worksheet.Cells[row, 9].Value = p.Instalacion;
        worksheet.Cells[row, 10].Value = p.Estado;
    }

    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
    return await package.GetAsByteArrayAsync();
}

// También corregir estos métodos para que coincidan con la interfaz:
public async Task<byte[]> ExportarPacientesPDFAsync(ReporteFiltrosModel filtros)
{
    var pacientes = await ObtenerPacientesParaReporteAsync(filtros);
    return await ExportarPacientesPDFAsync(pacientes);
}

private async Task<byte[]> ExportarPacientesPDFAsync(List<PacienteReporteModel> pacientes)
{
    using var memoryStream = new MemoryStream();
    var document = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
    var writer = PdfWriter.GetInstance(document, memoryStream);

    document.Open();

    // Título
    var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
    var title = new Paragraph("Reporte de Pacientes - DIGESA Cannabis Medicinal", titleFont);
    title.Alignment = Element.ALIGN_CENTER;
    title.SpacingAfter = 20;
    document.Add(title);

    // Fecha de generación
    var dateFont = FontFactory.GetFont("Arial", 10);
    var dateText = new Paragraph($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}", dateFont);
    dateText.Alignment = Element.ALIGN_RIGHT;
    dateText.SpacingAfter = 10;
    document.Add(dateText);

    // Tabla
    var table = new PdfPTable(10);
    table.WidthPercentage = 100;
    table.SetWidths(new float[] { 0.5f, 1.5f, 2f, 1.5f, 2f, 1.5f, 2f, 1.5f, 1.5f, 1.5f });

    // Encabezados de tabla
    var headerFont = FontFactory.GetFont("Arial", 10, Font.BOLD);
    string[] headers = { "ID", "Documento", "Nombre", "Correo", "Teléfono", "Fecha Nac.", "Sexo", "Región", "Instalación", "Estado" };

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
    var dataFont = FontFactory.GetFont("Arial", 9);
    foreach (var paciente in pacientes)
    {
        table.AddCell(new PdfPCell(new Phrase(paciente.Id.ToString(), dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.Documento, dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.NombreCompleto, dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.Correo, dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.Telefono, dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.FechaNacimiento?.ToString("dd/MM/yyyy") ?? "", dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.Sexo, dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.Region, dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.Instalacion, dataFont)) { Padding = 4 });
        table.AddCell(new PdfPCell(new Phrase(paciente.Estado, dataFont)) { Padding = 4 });
    }

    document.Add(table);

    // Pie de página
    var footer = new Paragraph($"Total de registros: {pacientes.Count}", dateFont);
    footer.Alignment = Element.ALIGN_RIGHT;
    footer.SpacingBefore = 20;
    document.Add(footer);

    document.Close();

    return memoryStream.ToArray();
}

    // Modelos internos
    private class SolicitudReporteModel
    {
        public int Id { get; set; }
        public string NumeroSolicitud { get; set; } = string.Empty;
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string PacienteNombre { get; set; } = string.Empty;
        public string PacienteDocumento { get; set; } = string.Empty;
        public string PacienteCorreo { get; set; } = string.Empty;
        public string PacienteTelefono { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Instalacion { get; set; } = string.Empty;
        public bool EsRenovacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string? NumeroCarnet { get; set; }
        public bool CarnetActivo { get; set; }
        public DateTime? FechaVencimiento { get; set; }
    }

    private class PacienteReporteModel
    {
        public int Id { get; set; }
        public string Documento { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Instalacion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }

    private class CarnetReporteModel
    {
        public string NumeroCarnet { get; set; } = string.Empty;
        public string PacienteNombre { get; set; } = string.Empty;
        public string PacienteDocumento { get; set; } = string.Empty;
        public string PacienteCorreo { get; set; } = string.Empty;
        public string PacienteTelefono { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool Activo { get; set; }
        public string Region { get; set; } = string.Empty;
        public string Instalacion { get; set; } = string.Empty;
    }
}