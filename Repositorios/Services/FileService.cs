using DIGESA.Components.Pages.Public;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components.Forms;

namespace DIGESA.Repositorios.Services;

public class FileService : IFileService
{
    private readonly DbContextDigesa _context;
    private readonly IWebHostEnvironment _environment;

    public FileService(DbContextDigesa context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public async Task<string> GuardarArchivoAsync(IBrowserFile file, int solicitudId)
    {
        const long maxFileSize = 10 * 1024 * 1024; // 10MB

        if (file.Size > maxFileSize)
            throw new Exception($"El archivo {file.Name} excede el tamaño máximo permitido (10MB)");

        var uploadDirectory = Path.Combine(_environment.WebRootPath, "uploads", solicitudId.ToString());

        if (!Directory.Exists(uploadDirectory))
            Directory.CreateDirectory(uploadDirectory);

        var fileExtension = Path.GetExtension(file.Name);
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadDirectory, uniqueFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.OpenReadStream(maxFileSize).CopyToAsync(stream);

        return $"/uploads/{solicitudId}/{uniqueFileName}";
    }

    public async Task<List<TbDocumentoAdjunto>> ProcesarDocumentosAsync(Solicitud.DocumentosModel documentos, int solicitudId,
        Dictionary<string, int> tipoDocumentoMap)
    {
        var archivosAGuardar = new List<TbDocumentoAdjunto>();

        await ProcesarListaArchivos(documentos.CedulaPaciente, "CedulaPaciente", solicitudId, tipoDocumentoMap,
            archivosAGuardar);
        await ProcesarListaArchivos(documentos.CertificacionMedica, "CertificacionMedica", solicitudId,
            tipoDocumentoMap, archivosAGuardar);
        await ProcesarListaArchivos(documentos.FotoPaciente, "FotoPaciente", solicitudId, tipoDocumentoMap,
            archivosAGuardar);
        await ProcesarListaArchivos(documentos.CedulaAcompanante, "CedulaAcompanante", solicitudId, tipoDocumentoMap,
            archivosAGuardar);
        await ProcesarListaArchivos(documentos.SentenciaTutor, "SentenciaTutor", solicitudId, tipoDocumentoMap,
            archivosAGuardar);
        await ProcesarListaArchivos(documentos.Antecedentes, "Antecedentes", solicitudId, tipoDocumentoMap,
            archivosAGuardar);
        await ProcesarListaArchivos(documentos.IdentidadMenor, "IdentidadMenor", solicitudId, tipoDocumentoMap,
            archivosAGuardar);
        await ProcesarListaArchivos(documentos.ConsentimientoPadres, "ConsentimientoPadres", solicitudId,
            tipoDocumentoMap, archivosAGuardar);
        await ProcesarListaArchivos(documentos.CertificadoNacimientoMenor, "CertificadoNacimientoMenor", solicitudId,
            tipoDocumentoMap, archivosAGuardar);
        await ProcesarListaArchivos(documentos.FotoAcompanante, "FotoAcompanante", solicitudId, tipoDocumentoMap,
            archivosAGuardar);

        return archivosAGuardar;
    }

    public async Task<List<TbDocumentoAdjunto>> GuardarArchivosAdjuntosAsync(Solicitud.DocumentosModel documentos,
        int solicitudId, Dictionary<string, int> tipoDocumentoMap)
    {
        var archivosAGuardar = await ProcesarDocumentosAsync(documentos, solicitudId, tipoDocumentoMap);

        if (archivosAGuardar.Any())
        {
            await _context.TbDocumentoAdjunto.AddRangeAsync(archivosAGuardar);
            await _context.SaveChangesAsync();
        }

        return archivosAGuardar;
    }

    private async Task ProcesarListaArchivos(List<IBrowserFile> files, string tipoDocumentoNombre,
        int solicitudId, Dictionary<string, int> tipoDocumentoMap, List<TbDocumentoAdjunto> archivosAGuardar)
    {
        if (files == null || !files.Any()) return;

        var tipoDocumentoEntry = tipoDocumentoMap.FirstOrDefault(x =>
            x.Key.Equals(tipoDocumentoNombre, StringComparison.OrdinalIgnoreCase));

        if (tipoDocumentoEntry.Value == 0)
        {
            Console.WriteLine($"Tipo de documento no encontrado: {tipoDocumentoNombre}");
            return;
        }

        foreach (var file in files)
        {
            try
            {
                var rutaAlmacenada = await GuardarArchivoAsync(file, solicitudId);
                var nombreGuardado = Path.GetFileName(rutaAlmacenada);

                var documento = new TbDocumentoAdjunto
                {
                    SolRegCannabisId = solicitudId,
                    TipoDocumentoId = tipoDocumentoEntry.Value,
                    NombreOriginal = file.Name,
                    NombreGuardado = nombreGuardado,
                    Url = rutaAlmacenada,
                    FechaSubidaUtc = DateTime.UtcNow,
                    SubidoPor = "Sistema",
                    IsValido = true
                };

                archivosAGuardar.Add(documento);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al procesar archivo {file.Name}: {ex.Message}");
                throw new Exception($"Error al procesar archivo {file.Name}: {ex.Message}", ex);
            }
        }
    }
}