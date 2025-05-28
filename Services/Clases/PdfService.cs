using DIGESA.Models.Entities.DIGESA;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class PdfService
{
    public byte[] GenerateCertification(Certificacion model)
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A6);
                page.Margin(1, Unit.Centimetre);
                page.Header().Text("CERTIFICADO OFICIAL").Bold().FontSize(16);
                page.Content().Column(x =>
                {
                    x.Item().Text($"Nombre: {model.NombreCompleto}");
                    x.Item().Text($"Documento: {model.DocumentoIdentidad}");
                    x.Item().Text($"Fecha de emisión: {model.FechaEmision}");
                    x.Item().Text($"Vigente hasta: {model.VigenciaHasta}");
                    using var ms = new MemoryStream(model.QrCodeImage);
                    x.Item().Image(ms); 
                });
            });
        }).GeneratePdf();
    }
}