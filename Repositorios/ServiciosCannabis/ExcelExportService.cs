using ClosedXML.Excel;
using DIGESA.Data;
using DIGESA.Models.CannabisModels.Listados;
using DIGESA.Models.CannabisModels.Reportes;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.ServiciosCannabis;

public class ExcelExportService : IExcelExportService
{
    private readonly DbContextDigesa _context; // Agregar campo privado

    // Agregar constructor con inyección de dependencia
    public ExcelExportService(DbContextDigesa context)
    {
        _context = context;
    }

    public async Task<byte[]> ExportSolicitudesAsync(
        List<PacienteListadoViewModel> solicitudes,
        string? estadoFiltro,
        string? documentoBusqueda)
    {
        try
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Solicitudes");

            // Título del reporte
            worksheet.Cell(1, 1).Value = "REPORTE DE SOLICITUDES - SISTEMA CANNABIS MEDICINAL";
            worksheet.Cell(1, 1).Style.Font.Bold = true;
            worksheet.Cell(1, 1).Style.Font.FontSize = 14;
            worksheet.Range(1, 1, 1, 8).Merge();

            // Información del filtro
            int row = 3;
            if (!string.IsNullOrEmpty(estadoFiltro) || !string.IsNullOrEmpty(documentoBusqueda))
            {
                worksheet.Cell(row, 1).Value = "Filtros aplicados:";
                worksheet.Cell(row, 1).Style.Font.Bold = true;
                row++;

                if (!string.IsNullOrEmpty(estadoFiltro))
                {
                    worksheet.Cell(row, 1).Value = $"Estado: {estadoFiltro}";
                    row++;
                }

                if (!string.IsNullOrEmpty(documentoBusqueda))
                {
                    worksheet.Cell(row, 1).Value = $"Documento buscado: {documentoBusqueda}";
                    row++;
                }

                worksheet.Cell(row, 1).Value = $"Total de registros: {solicitudes.Count}";
                worksheet.Cell(row, 1).Style.Font.Bold = true;
                row++;
                row++; // Espacio en blanco
            }

            // Encabezados de la tabla
            int headerRow = row;
            worksheet.Cell(headerRow, 1).Value = "N° Solicitud";
            worksheet.Cell(headerRow, 2).Value = "Paciente";
            worksheet.Cell(headerRow, 3).Value = "Documento";
            worksheet.Cell(headerRow, 4).Value = "Edad";
            worksheet.Cell(headerRow, 5).Value = "Provincia";
            worksheet.Cell(headerRow, 6).Value = "Fecha Solicitud";
            worksheet.Cell(headerRow, 7).Value = "Fecha Vencimiento";
            worksheet.Cell(headerRow, 8).Value = "Estado";

            // Estilo de encabezados
            var headerRange = worksheet.Range(headerRow, 1, headerRow, 8);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Datos
            int dataRow = headerRow + 1;
            foreach (var item in solicitudes)
            {
                worksheet.Cell(dataRow, 1).Value = item.Id;
                worksheet.Cell(dataRow, 2).Value = item.NombreCompleto?.Trim();
                worksheet.Cell(dataRow, 3).Value = item.DocumentoFormateado;
                worksheet.Cell(dataRow, 4).Value = item.Edad;
                worksheet.Cell(dataRow, 5).Value = item.Provincia;

                if (item.FechaSolicitud.HasValue)
                {
                    worksheet.Cell(dataRow, 6).Value = item.FechaSolicitud.Value;
                    worksheet.Cell(dataRow, 6).Style.DateFormat.Format = "dd/MM/yyyy";
                }

                if (item.FechaVencimiento.HasValue)
                {
                    worksheet.Cell(dataRow, 7).Value = item.FechaVencimiento.Value;
                    worksheet.Cell(dataRow, 7).Style.DateFormat.Format = "dd/MM/yyyy";

                    // Resaltar fechas vencidas o próximas a vencer
                    if (item.EstaVencido)
                    {
                        worksheet.Cell(dataRow, 7).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 199, 206);
                        worksheet.Cell(dataRow, 7).Style.Font.FontColor = XLColor.FromArgb(156, 0, 6);
                    }
                    else if (item.EstaProximoAVencer)
                    {
                        worksheet.Cell(dataRow, 7).Style.Fill.BackgroundColor = XLColor.FromArgb(255, 235, 156);
                        worksheet.Cell(dataRow, 7).Style.Font.FontColor = XLColor.FromArgb(156, 101, 0);
                    }
                }

                worksheet.Cell(dataRow, 8).Value = item.EstadoSolicitud;

                // Color según estado
                var estadoCell = worksheet.Cell(dataRow, 8);
                switch (item.EstadoSolicitud?.ToLower())
                {
                    case "aprobada":
                        estadoCell.Style.Fill.BackgroundColor = XLColor.FromArgb(198, 239, 206);
                        estadoCell.Style.Font.FontColor = XLColor.FromArgb(0, 97, 0);
                        break;
                    case "rechazada":
                        estadoCell.Style.Fill.BackgroundColor = XLColor.FromArgb(255, 199, 206);
                        estadoCell.Style.Font.FontColor = XLColor.FromArgb(156, 0, 6);
                        break;
                    case "pendiente":
                        estadoCell.Style.Fill.BackgroundColor = XLColor.FromArgb(255, 235, 156);
                        estadoCell.Style.Font.FontColor = XLColor.FromArgb(156, 101, 0);
                        break;
                }

                dataRow++;
            }

            // Ajustar ancho de columnas
            worksheet.Columns().AdjustToContents();

            // Agregar bordes a la tabla
            var dataRange = worksheet.Range(headerRow, 1, dataRow - 1, 8);
            dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            // Pie de página
            worksheet.Cell(dataRow + 1, 1).Value = $"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}";
            worksheet.Cell(dataRow + 1, 1).Style.Font.Italic = true;

            // Convertir a byte array
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return await Task.FromResult(stream.ToArray());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error al generar el reporte Excel: {ex.Message}", ex);
        }
    }

    public Task<byte[]> ExportUsuariosAsync(List<ApplicationUser> usuarios)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Usuarios");

        // Encabezados
        worksheet.Cell(1, 1).Value = "Nombre";
        worksheet.Cell(1, 2).Value = "Correo Electrónico";
        worksheet.Cell(1, 3).Value = "Activo";
        worksheet.Cell(1, 4).Value = "Fecha Registro";
        worksheet.Cell(1, 5).Value = "Último Login";
        worksheet.Cell(1, 6).Value = "Requiere Cambiar Contraseña";

        var headerRange = worksheet.Range(1, 1, 1, 6);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
        headerRange.Style.Font.FontColor = XLColor.White;

        // Datos
        int row = 2;
        foreach (var usuario in usuarios)
        {
            worksheet.Cell(row, 1).Value = $"{usuario.FirstName} {usuario.LastName}";
            worksheet.Cell(row, 2).Value = usuario.Email;
            worksheet.Cell(row, 3).Value = usuario.IsAproved ? "SI" : "NO";

            // Usar CreatedOn en lugar de RegisteredDate
            if (usuario.CreatedOn.HasValue)
            {
                worksheet.Cell(row, 4).Value = usuario.CreatedOn.Value;
                worksheet.Cell(row, 4).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
            }

            // Último login
            if (usuario.LastLoginDate.HasValue)
            {
                worksheet.Cell(row, 5).Value = usuario.LastLoginDate.Value;
                worksheet.Cell(row, 5).Style.DateFormat.Format = "dd/MM/yyyy HH:mm";
            }

            // Requiere cambiar contraseña
            worksheet.Cell(row, 6).Value = usuario.MustChangePassword ? "SI" : "NO";

            // Color según estado activo
            var estadoCell = worksheet.Cell(row, 3);
            if (usuario.IsAproved)
            {
                estadoCell.Style.Fill.BackgroundColor = XLColor.FromArgb(198, 239, 206);
                estadoCell.Style.Font.FontColor = XLColor.FromArgb(0, 97, 0);
            }
            else
            {
                estadoCell.Style.Fill.BackgroundColor = XLColor.FromArgb(255, 199, 206);
                estadoCell.Style.Font.FontColor = XLColor.FromArgb(156, 0, 6);
            }

            // Color para requerir cambio de contraseña
            var cambioPassCell = worksheet.Cell(row, 6);
            if (usuario.MustChangePassword)
            {
                cambioPassCell.Style.Fill.BackgroundColor = XLColor.FromArgb(255, 235, 156);
                cambioPassCell.Style.Font.FontColor = XLColor.FromArgb(156, 101, 0);
            }
            else
            {
                cambioPassCell.Style.Fill.BackgroundColor = XLColor.FromArgb(198, 239, 206);
                cambioPassCell.Style.Font.FontColor = XLColor.FromArgb(0, 97, 0);
            }

            row++;
        }

        worksheet.Columns().AdjustToContents();

        // Bordes
        var dataRange = worksheet.Range(1, 1, row - 1, 6);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return Task.FromResult(stream.ToArray());
    }

    // Método adicional para exportar reportes estadísticos
    public Task<byte[]> ExportReporteEstadisticoAsync(Dictionary<string, int> estadisticas)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Estadísticas");

        // Título
        worksheet.Cell(1, 1).Value = "REPORTE ESTADÍSTICO - SISTEMA CANNABIS MEDICINAL";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
        worksheet.Range(1, 1, 1, 3).Merge();

        // Encabezados
        worksheet.Cell(3, 1).Value = "Categoría";
        worksheet.Cell(3, 2).Value = "Cantidad";
        worksheet.Cell(3, 3).Value = "Porcentaje";

        var headerRange = worksheet.Range(3, 1, 3, 3);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
        headerRange.Style.Font.FontColor = XLColor.White;

        // Datos
        int row = 4;
        int total = estadisticas.Sum(x => x.Value);

        foreach (var item in estadisticas)
        {
            worksheet.Cell(row, 1).Value = item.Key;
            worksheet.Cell(row, 2).Value = item.Value;

            if (total > 0)
            {
                double porcentaje = (item.Value * 100.0) / total;
                worksheet.Cell(row, 3).Value = porcentaje;
                worksheet.Cell(row, 3).Style.NumberFormat.Format = "0.00%";
            }

            row++;
        }

        // Total
        worksheet.Cell(row, 1).Value = "TOTAL";
        worksheet.Cell(row, 1).Style.Font.Bold = true;
        worksheet.Cell(row, 2).Value = total;
        worksheet.Cell(row, 2).Style.Font.Bold = true;

        worksheet.Columns().AdjustToContents();

        // Bordes
        var dataRange = worksheet.Range(3, 1, row, 3);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return Task.FromResult(stream.ToArray());
    }

    public Task<byte[]> ExportEstadisticasPacientesProductosAsync(
        List<EstadisticaPacienteProducto> estadisticas)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Estadísticas Pacientes-Productos");

        // Título principal
        worksheet.Cell(1, 1).Value = "REPORTE ESTADÍSTICO - PACIENTES Y PRODUCTOS DE CANNABIS MEDICINAL";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
        worksheet.Range(1, 1, 1, 6).Merge();

        // Sub título
        worksheet.Cell(2, 1).Value = $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}";
        worksheet.Cell(2, 1).Style.Font.Italic = true;
        worksheet.Range(2, 1, 2, 6).Merge();

        // Sección 1: Totales generales
        worksheet.Cell(4, 1).Value = "RESUMEN GENERAL";
        worksheet.Cell(4, 1).Style.Font.Bold = true;
        worksheet.Cell(4, 1).Style.Font.FontSize = 12;

        var totalPacientes = estadisticas.Sum(e => e.CantidadPacientes);
        var totalProductosUnicos = estadisticas.Select(e => e.NombreProducto).Distinct().Count();

        worksheet.Cell(5, 1).Value = "Total de Pacientes Registrados:";
        worksheet.Cell(5, 2).Value = totalPacientes;
        worksheet.Cell(5, 2).Style.Font.Bold = true;

        worksheet.Cell(6, 1).Value = "Total de Productos Diferentes:";
        worksheet.Cell(6, 2).Value = totalProductosUnicos;
        worksheet.Cell(6, 2).Style.Font.Bold = true;

        // Espacio
        int row = 8;

        // Sección 2: Distribución por productos
        worksheet.Cell(row, 1).Value = "DISTRIBUCIÓN POR PRODUCTOS";
        worksheet.Cell(row, 1).Style.Font.Bold = true;
        worksheet.Cell(row, 1).Style.Font.FontSize = 12;
        row++;

        // Encabezados productos
        worksheet.Cell(row, 1).Value = "Producto";
        worksheet.Cell(row, 2).Value = "Cantidad de Pacientes";
        worksheet.Cell(row, 3).Value = "Porcentaje";
        worksheet.Cell(row, 4).Value = "Forma Farmacéutica";
        worksheet.Cell(row, 5).Value = "Vía de Administración";
        worksheet.Cell(row, 6).Value = "Concentración Principal";

        var headerRangeProductos = worksheet.Range(row, 1, row, 6);
        headerRangeProductos.Style.Font.Bold = true;
        headerRangeProductos.Style.Fill.BackgroundColor = XLColor.FromArgb(79, 129, 189);
        headerRangeProductos.Style.Font.FontColor = XLColor.White;
        headerRangeProductos.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        row++;

        // Datos de productos
        foreach (var item in estadisticas.OrderByDescending(e => e.CantidadPacientes))
        {
            worksheet.Cell(row, 1).Value = item.NombreProducto?.ToUpper() ?? "NO ESPECIFICADO";

            // Aplicar formato de mayúsculas automático
            if (!string.IsNullOrEmpty(item.NombreProducto))
            {
                worksheet.Cell(row, 1).Value = item.NombreProducto.ToUpper();
            }

            worksheet.Cell(row, 2).Value = item.CantidadPacientes;

            // Calcular porcentaje
            if (totalPacientes > 0)
            {
                double porcentaje = (item.CantidadPacientes * 100.0) / totalPacientes;
                worksheet.Cell(row, 3).Value = porcentaje / 100.0; // Convertir a decimal para formato porcentaje
                worksheet.Cell(row, 3).Style.NumberFormat.Format = "0.00%";
            }

            // Forma farmacéutica en mayúsculas
            worksheet.Cell(row, 4).Value = !string.IsNullOrEmpty(item.FormaFarmaceutica)
                ? item.FormaFarmaceutica.ToUpper()
                : "NO ESPECIFICADA";

            // Vía de administración en mayúsculas
            worksheet.Cell(row, 5).Value = !string.IsNullOrEmpty(item.ViaAdministracion)
                ? item.ViaAdministracion.ToUpper()
                : "NO ESPECIFICADA";

            // Concentración
            worksheet.Cell(row, 6).Value = item.ConcentracionPrincipal;

            row++;
        }

        // Sección 3: Gráfico (opcional - se puede agregar con ClosedXML)
        row += 2;
        worksheet.Cell(row, 1).Value = "TENDENCIAS Y ANÁLISIS";
        worksheet.Cell(row, 1).Style.Font.Bold = true;
        worksheet.Cell(row, 1).Style.Font.FontSize = 12;
        row++;

        // Análisis cualitativo
        var productoMasUsado = estadisticas.OrderByDescending(e => e.CantidadPacientes).FirstOrDefault();
        if (productoMasUsado != null)
        {
            worksheet.Cell(row, 1).Value = "Producto más utilizado:";
            worksheet.Cell(row, 2).Value = productoMasUsado.NombreProducto?.ToUpper() ?? "NO ESPECIFICADO";
            worksheet.Cell(row, 2).Style.Font.Bold = true;
            row++;

            worksheet.Cell(row, 1).Value = "Pacientes que lo usan:";
            worksheet.Cell(row, 2).Value = productoMasUsado.CantidadPacientes;
            worksheet.Cell(row, 3).Value = $"{((productoMasUsado.CantidadPacientes * 100.0) / totalPacientes):F1}%";
            row++;
        }

        // Ajustar ancho de columnas
        worksheet.Columns().AdjustToContents();

        // Agregar bordes a las secciones principales
        var dataRange = worksheet.Range(9, 1, row - 1, 6);
        dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return Task.FromResult(stream.ToArray());
    }

    public async Task<List<EstadisticaPacienteProducto>> GetEstadisticasPacientesProductosAsync()
    {
        try
        {
            // Primero obtener todos los productos aprobados
            var productosQuery = _context.TbNombreProductoPaciente
                .Include(p => p.Solicitud)
                .ThenInclude(s => s.Paciente)
                .Include(p => p.FormaFarmaceuticaNavigation)
                .Include(p => p.ViaAdministracion)
                .Where(p => p.Solicitud != null &&
                            p.Solicitud.EstadoSolicitudId == 2 &&
                            p.PacienteId != null)
                .Select(p => new
                {
                    p.NombreProducto,
                    p.FormaFarmaceutica,
                    p.ViaConsumoProducto,
                    p.NombreConcentracion,
                    p.CantidadConcentracion,
                    PacienteId = p.PacienteId.Value,
                    p.Solicitud!.FechaSolicitud,
                    FormaNombre = p.FormaFarmaceuticaNavigation != null
                        ? p.FormaFarmaceuticaNavigation.Nombre
                        : p.FormaFarmaceutica,
                    ViaNombre = p.ViaAdministracion != null ? p.ViaAdministracion.Nombre : p.ViaConsumoProducto
                });

            var productos = await productosQuery.ToListAsync();

            // Procesar en memoria para evitar conflictos con EF
            var estadisticas = productos
                .GroupBy(p => new
                {
                    NombreProducto = p.NombreProducto ?? "NO ESPECIFICADO",
                    FormaFarmaceutica = p.FormaNombre ?? "NO ESPECIFICADA",
                    ViaConsumoProducto = p.ViaNombre ?? "NO ESPECIFICADA"
                })
                .Select(g => new EstadisticaPacienteProducto
                {
                    NombreProducto = g.Key.NombreProducto,
                    CantidadPacientes = g.Select(p => p.PacienteId).Distinct().Count(),
                    FormaFarmaceutica = g.Key.FormaFarmaceutica,
                    ViaAdministracion = g.Key.ViaConsumoProducto,
                    ConcentracionPrincipal = ObtenerConcentracionPrincipal(
                        g.Where(p => !string.IsNullOrEmpty(p.NombreConcentracion))
                            .Select(p => p.NombreConcentracion!)
                            .FirstOrDefault() ?? "NO ESPECIFICADA"
                    ),
                    CantidadConcentracionPromedio = (decimal?)g.Average(p => p.CantidadConcentracion) ?? 0,
                    FechaUltimoUso = g.Max(p => p.FechaSolicitud) ?? DateTime.MinValue,
                    CantidadTotalProductos = g.Count()
                })
                .OrderByDescending(e => e.CantidadPacientes)
                .ThenBy(e => e.NombreProducto)
                .ToList();

            return estadisticas;
        }
        catch (Exception ex)
        {
            // Log del error
            Console.WriteLine($"Error en GetEstadisticasPacientesProductosAsync: {ex.Message}");
            throw;
        }
    }

// Método auxiliar para limpiar/analizar la concentración
    private string ObtenerConcentracionPrincipal(string concentracion)
    {
        if (string.IsNullOrEmpty(concentracion) || concentracion == "NO ESPECIFICADA")
            return "NO ESPECIFICADA";

        concentracion = concentracion.Trim().ToUpper();

        // Identificar si es CBD, THC, o mixto
        if (concentracion.Contains("CBD") && concentracion.Contains("THC"))
            return "CBD/THC MIXTO";
        else if (concentracion.Contains("CBD"))
            return "CBD";
        else if (concentracion.Contains("THC"))
            return "THC";
        else
            return concentracion; // Otra concentración especificada
    }
}