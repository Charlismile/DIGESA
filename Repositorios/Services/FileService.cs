using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class FileService : IFileService
{
    private readonly DbContextDigesa _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<FileService> _logger;

    public FileService(DbContextDigesa context, IWebHostEnvironment environment, ILogger<FileService> logger)
    {
        _context = context;
        _environment = environment;
        _logger = logger;
    }

    public async Task<string> GuardarArchivoAsync(IBrowserFile file, int solicitudId)
    {
        const long maxFileSize = 10 * 1024 * 1024; // 10MB

        if (file.Size > maxFileSize)
            throw new InvalidOperationException($"El archivo {file.Name} excede el tamaño máximo permitido (10MB)");

        // Validar extensiones permitidas
        var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };
        var fileExtension = Path.GetExtension(file.Name).ToLower();
        
        if (!allowedExtensions.Contains(fileExtension))
            throw new InvalidOperationException($"Tipo de archivo no permitido: {fileExtension}");

        var uploadDirectory = Path.Combine(_environment.WebRootPath, "uploads", solicitudId.ToString());

        if (!Directory.Exists(uploadDirectory))
            Directory.CreateDirectory(uploadDirectory);

        var uniqueFileName = $"{Guid.NewGuid():N}{fileExtension}";
        var filePath = Path.Combine(uploadDirectory, uniqueFileName);

        try
        {
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(stream);

            _logger.LogInformation($"Archivo guardado: {uniqueFileName} para solicitud {solicitudId}");
            return $"/uploads/{solicitudId}/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error guardando archivo: {file.Name}");
            throw;
        }
    }

    public async Task<List<TbDocumentoAdjunto>> GuardarArchivosAdjuntosAsync(
        List<IBrowserFile> archivos, int solicitudId, int tipoDocumentoId)
    {
        var documentos = new List<TbDocumentoAdjunto>();

        foreach (var archivo in archivos)
        {
            try
            {
                var rutaAlmacenada = await GuardarArchivoAsync(archivo, solicitudId);
                var nombreGuardado = Path.GetFileName(rutaAlmacenada);

                var documento = new TbDocumentoAdjunto
                {
                    SolRegCannabisId = solicitudId,
                    TipoDocumentoId = tipoDocumentoId,
                    NombreOriginal = archivo.Name,
                    NombreGuardado = nombreGuardado,
                    Url = rutaAlmacenada,
                    FechaSubidaUtc = DateTime.UtcNow,
                    SubidoPor = "Sistema",
                    IsValido = true,
                    EsDocumentoMedico = tipoDocumentoId == 2, // Suponiendo que 2 es Certificación Médica
                    Version = 1
                };

                documentos.Add(documento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error procesando archivo: {archivo.Name}");
                throw;
            }
        }

        if (documentos.Any())
        {
            await _context.TbDocumentoAdjunto.AddRangeAsync(documentos);
            await _context.SaveChangesAsync();
        }

        return documentos;
    }

    public async Task<bool> EliminarArchivoAsync(int documentoId)
    {
        try
        {
            var documento = await _context.TbDocumentoAdjunto
                .FirstOrDefaultAsync(d => d.Id == documentoId);

            if (documento == null)
                return false;

            // Eliminar archivo físico
            var filePath = Path.Combine(_environment.WebRootPath, documento.Url.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Eliminar registro de la base de datos
            _context.TbDocumentoAdjunto.Remove(documento);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Archivo eliminado: {documentoId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error eliminando archivo: {documentoId}");
            return false;
        }
    }

    public async Task<string> ObtenerRutaArchivoAsync(int documentoId)
    {
        var documento = await _context.TbDocumentoAdjunto
            .FirstOrDefaultAsync(d => d.Id == documentoId);

        return documento?.Url ?? string.Empty;
    }

    // Método auxiliar para obtener todos los archivos de una solicitud
    public async Task<List<TbDocumentoAdjunto>> ObtenerArchivosPorSolicitudAsync(int solicitudId)
    {
        return await _context.TbDocumentoAdjunto
            .Include(d => d.TipoDocumento)
            .Where(d => d.SolRegCannabisId == solicitudId)
            .OrderByDescending(d => d.FechaSubidaUtc)
            .ToListAsync();
    }
}