using ClosedXML.Excel;
using DIGESA.Data;
using DIGESA.Models.CannabisModels.Listados;
using DIGESA.Repositorios.InterfacesCannabis;

namespace DIGESA.Repositorios.ServiciosCannabis;

public class ExcelExportService : IExcelExportService
{
    public Task<byte[]> ExportSolicitudesAsync(
        List<PacienteListadoViewModel> data,
        string? estado,
        string? documento)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Solicitudes");

        // Encabezados
        ws.Cell(1, 1).Value = "N° Solicitud";
        ws.Cell(1, 2).Value = "Paciente";
        ws.Cell(1, 3).Value = "Documento";
        ws.Cell(1, 4).Value = "Fecha Solicitud";
        ws.Cell(1, 5).Value = "Fecha Vencimiento";
        ws.Cell(1, 6).Value = "Estado";

        ws.Range(1, 1, 1, 6).Style.Font.Bold = true;

        int row = 2;
        foreach (var s in data)
        {
            ws.Cell(row, 1).Value = s.Id;
            ws.Cell(row, 2).Value = s.NombreCompleto;
            ws.Cell(row, 3).Value = s.Documento;
            ws.Cell(row, 4).Value = s.FechaSolicitud?.ToString("dd/MM/yyyy");
            ws.Cell(row, 5).Value = s.FechaVencimiento?.ToString("dd/MM/yyyy");
            ws.Cell(row, 6).Value = s.EstadoSolicitud;
            row++;
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return Task.FromResult(ms.ToArray());
    }

    public Task<byte[]> ExportUsuariosAsync(List<ApplicationUser> usuarios)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Usuarios");

        ws.Cell(1, 1).Value = "Nombre";
        ws.Cell(1, 2).Value = "Correo";
        ws.Cell(1, 3).Value = "Activo";

        ws.Range(1, 1, 1, 3).Style.Font.Bold = true;

        int row = 2;
        foreach (var u in usuarios)
        {
            ws.Cell(row, 1).Value = $"{u.FirstName} {u.LastName}";
            ws.Cell(row, 2).Value = u.Email;
            ws.Cell(row, 3).Value = u.IsAproved ? "SI" : "NO";
            row++;
        }

        ws.Columns().AdjustToContents();

        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return Task.FromResult(ms.ToArray());
    }
}
