using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class CarnetService : ICarnetService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<CarnetService> _logger;
    private readonly IWebHostEnvironment _environment;

    public CarnetService(
        DbContextDigesa context,
        ILogger<CarnetService> logger,
        IWebHostEnvironment environment)
    {
        _context = context;
        _logger = logger;
        _environment = environment;
    }

    public async Task<ResultModel<string>> GenerarNumeroCarnetAsync(int pacienteId, bool esAcompanante = false)
    {
        try
        {
            var tipo = esAcompanante ? "A" : "U";
            
            // Obtener último número secuencial del tipo correspondiente
            var ultimoCarnet = await _context.TbSolRegCannabis
                .Where(s => s.NumeroCarnet != null && s.NumeroCarnet.StartsWith($"DGSP-{tipo}-"))
                .OrderByDescending(s => s.NumeroCarnet)
                .FirstOrDefaultAsync();

            int siguienteNumero = 1;
            if (ultimoCarnet != null && !string.IsNullOrEmpty(ultimoCarnet.NumeroCarnet))
            {
                var partes = ultimoCarnet.NumeroCarnet.Split('-');
                if (partes.Length >= 3 && int.TryParse(partes[2], out int ultimo))
                {
                    siguienteNumero = ultimo + 1;
                }
            }

            var numeroCarnet = $"DGSP-{tipo}-{siguienteNumero:0000000}";
            
            // Generar también para acompañante si es necesario
            if (esAcompanante)
            {
                var acompanante = await _context.TbAcompanantePaciente
                    .FirstOrDefaultAsync(a => a.PacienteId == pacienteId);
                
                if (acompanante != null)
                {
                    var numeroAcompanante = $"DGSP-A-{siguienteNumero:0000000}A";
                    return ResultModel<string>.SuccessResult(numeroAcompanante, "Número de carnet para acompañante generado");
                }
            }

            return ResultModel<string>.SuccessResult(numeroCarnet, "Número de carnet generado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando número de carnet");
            return ResultModel<string>.ErrorResult("Error al generar número de carnet");
        }
    }

    public async Task<ResultModel<byte[]>> GenerarCarnetPDFAsync(CarnetModel carnet, bool incluirAcompanante = false)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            using (var document = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30))
            {
                var writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Título
                var font = FontFactory.GetFont("Arial", 16, Font.BOLD, BaseColor.BLACK);
                var paragraph = new Paragraph("IDENTIFICACIÓN PARA USO DE CANNABIS MEDICINAL", font);
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.SpacingAfter = 20;
                document.Add(paragraph);

                // Información del usuario
                var table = new PdfPTable(2);
                table.WidthPercentage = 80;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SetWidths(new float[] { 1, 2 });

                // Foto del usuario (placeholder)
                var fotoCell = new PdfPCell();
                fotoCell.FixedHeight = 150;
                fotoCell.Border = 0;
                table.AddCell(fotoCell);

                // Información
                var infoCell = new PdfPCell();
                infoCell.Border = 0;
                infoCell.Padding = 10;

                var infoBuilder = new StringBuilder();
                infoBuilder.AppendLine("USUARIO");
                infoBuilder.AppendLine($"{carnet.NombreCompleto}");
                infoBuilder.AppendLine(" ");
                infoBuilder.AppendLine("CÉDULA / PASAPORTE");
                infoBuilder.AppendLine($"{carnet.Documento}");
                infoBuilder.AppendLine(" ");
                infoBuilder.AppendLine("DIRECCIÓN FÍSICA DEL TITULAR");
                infoBuilder.AppendLine($"{carnet.Direccion}");
                infoBuilder.AppendLine(" ");
                infoBuilder.AppendLine("FECHA DE EMISIÓN");
                infoBuilder.AppendLine($"{carnet.FechaEmision:dd/MM/yyyy}");
                infoBuilder.AppendLine(" ");
                infoBuilder.AppendLine("FECHA DE CADUCIDAD");
                infoBuilder.AppendLine($"{carnet.FechaVencimiento:dd/MM/yyyy}");

                var infoFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                var tituloFont = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);

                var infoParagraph = new Paragraph();
                
                var lines = infoBuilder.ToString().Split('\n');
                foreach (var line in lines)
                {
                    var isTitulo = !line.Contains(":") && line.ToUpper() == line;
                    var fontLine = isTitulo ? tituloFont : infoFont;
                    infoParagraph.Add(new Chunk(line + "\n", fontLine));
                }

                infoCell.AddElement(infoParagraph);
                table.AddCell(infoCell);

                document.Add(table);

                // Número de carnet
                var numeroFont = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLUE);
                var numeroParagraph = new Paragraph($"Número: {carnet.NumeroCarnet}", numeroFont);
                numeroParagraph.Alignment = Element.ALIGN_CENTER;
                numeroParagraph.SpacingBefore = 20;
                document.Add(numeroParagraph);

                // Advertencia legal
                var warning = @"Decreto Ejecutivo 6 de 4 de abril de 2025:

'Está prohibido conducir u operar un vehículo de motor bajo los efectos incapacitantes del cannabis medicinal. 
Toda persona que viole esta prohibición estará sujeta a multas y acciones administrativas y penales'";

                var warningFont = FontFactory.GetFont("Arial", 8, Font.ITALIC, BaseColor.RED);
                var warningParagraph = new Paragraph(warning, warningFont);
                warningParagraph.Alignment = Element.ALIGN_CENTER;
                warningParagraph.SpacingBefore = 30;
                document.Add(warningParagraph);

                // Firma
                var firmaPath = Path.Combine(_environment.WebRootPath, "assets", "firma-digital.png");
                if (File.Exists(firmaPath))
                {
                    var firma = Image.GetInstance(firmaPath);
                    firma.ScaleToFit(150, 50);
                    firma.Alignment = Element.ALIGN_RIGHT;
                    document.Add(firma);
                }

                var firmaText = new Paragraph("FIRMA AUTORIZADA", FontFactory.GetFont("Arial", 10, Font.BOLD));
                firmaText.Alignment = Element.ALIGN_RIGHT;
                document.Add(firmaText);

                document.Close();
            }

            return ResultModel<byte[]>.SuccessResult(memoryStream.ToArray(), "Carnet generado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando carnet PDF");
            return ResultModel<byte[]>.ErrorResult("Error al generar el carnet");
        }
    }

    public async Task<ResultModel<bool>> AsignarCarnetSolicitudAsync(int solicitudId, string numeroCarnet, bool esAcompanante = false)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud == null)
                return ResultModel<bool>.ErrorResult("Solicitud no encontrada");

            // Si es acompañante, generar también carnet para acompañante
            if (esAcompanante && solicitud.PacienteId.HasValue)
            {
                var acompanante = await _context.TbAcompanantePaciente
                    .FirstOrDefaultAsync(a => a.PacienteId == solicitud.PacienteId);
                
                if (acompanante != null)
                {
                    // Generar número de carnet para acompañante (mismo número con sufijo A)
                    var numeroAcompanante = numeroCarnet.EndsWith("A") ? numeroCarnet : numeroCarnet + "A";
                    // Registrar en base de datos para acompañante (necesitarías una tabla para carnets de acompañantes)
                    _logger.LogInformation($"Carnet para acompañante generado: {numeroAcompanante}");
                }
            }

            solicitud.NumeroCarnet = numeroCarnet;
            solicitud.FechaEmisionCarnet = DateTime.Now;
            solicitud.FechaVencimientoCarnet = DateTime.Now.AddYears(2);
            solicitud.CarnetActivo = true;

            _context.TbSolRegCannabis.Update(solicitud);
            await _context.SaveChangesAsync();

            return ResultModel<bool>.SuccessResult(true, "Carnet asignado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error asignando carnet a solicitud {solicitudId}");
            return ResultModel<bool>.ErrorResult("Error al asignar el carnet");
        }
    }

    public async Task<ResultModel<bool>> VerificarCarnetValidoAsync(string numeroCarnet)
    {
        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .FirstOrDefaultAsync(s => s.NumeroCarnet == numeroCarnet);

            if (solicitud == null)
                return ResultModel<bool>.ErrorResult("Carnet no encontrado");

            if (!solicitud.CarnetActivo.HasValue || !solicitud.CarnetActivo.Value)
                return ResultModel<bool>.ErrorResult("Carnet inactivo");

            if (!solicitud.FechaVencimientoCarnet.HasValue || solicitud.FechaVencimientoCarnet.Value < DateTime.Now)
                return ResultModel<bool>.ErrorResult("Carnet vencido");

            return ResultModel<bool>.SuccessResult(true, "Carnet válido");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error verificando carnet {numeroCarnet}");
            return ResultModel<bool>.ErrorResult("Error al verificar el carnet");
        }
    }
}