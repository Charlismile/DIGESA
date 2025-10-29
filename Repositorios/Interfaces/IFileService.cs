namespace DIGESA.Repositorios.Interfaces;

public interface IFileService
{
    Task<string> GuardarArchivoAsync(IBrowserFile file, int solicitudId);
    Task<List<TbDocumentoAdjunto>> ProcesarDocumentosAsync(DocumentosModel documentos, int solicitudId, Dictionary<string, int> tipoDocumentoMap);
    Task<List<TbDocumentoAdjunto>> GuardarArchivosAdjuntosAsync(DocumentosModel documentos, int solicitudId, Dictionary<string, int> tipoDocumentoMap);
}