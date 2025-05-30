using DIGESA.Models.Entities.DIGESA;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Text;

public class PdfCarnetService
{
    private readonly DbContextDigesa _db;
    private readonly IWebHostEnvironment _env;

    public PdfCarnetService(DbContextDigesa db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    public async Task<byte[]> GenerarCarnetPacienteAsync(int solicitudId)
    {
        var solicitud = await _db.Solicitudes
            .Include(s => s.Paciente)
            .Include(s => s.Acompanante)
            .FirstOrDefaultAsync(s => s.Id == solicitudId);

        if (solicitud == null) throw new Exception("Solicitud no encontrada.");

        var certificados = new List<byte[]>
        {
            CrearCarnetPDF(solicitud.Paciente.NombreCompleto, "Paciente", solicitud.Id)
        };

        if (solicitud.Acompanante != null)
        {
            certificados.Add(CrearCarnetPDF(solicitud.Acompanante.NombreCompleto, "Acompañante", solicitud.Id));
        }

        // Opcional: unificar los PDF en uno solo
        return certificados.First(); // solo uno. Puedes unirlos si lo deseas.
    }

    private byte[] CrearCarnetPDF(string nombre, string tipo, int solicitudId)
    {
        var qrTexto = $"https://midigesa.gob.pa/validar/{solicitudId}/{tipo.ToLower()}";
        var qrImage = GenerarQr(qrTexto);

        var colorFondo = tipo == "Paciente" ? Colors.Green.Lighten3 : Colors.Blue.Lighten3;

        var pdf = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A7.Landscape());
                page.Margin(10);
                page.Background(colorFondo);
                page.Content()
                    .Row(row =>
                    {
                        row.RelativeColumn().Column(col =>
                        {
                            col.Item().Text($"Carnet de {tipo}").Bold().FontSize(14);
                            col.Item().Text(nombre).FontSize(10);
                            col.Item().Text($"Solicitud #{solicitudId}").FontSize(8);
                        });
                        row.ConstantColumn(80).Image(qrImage);
                    });
            });
        }).GeneratePdf();

        return pdf;
    }

    private byte[] GenerarQr(string texto)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrData);
        return qrCode.GetGraphic(20);
    }
}
