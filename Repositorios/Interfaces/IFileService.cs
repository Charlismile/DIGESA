using DIGESA.Components.Pages.Public;
using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.AspNetCore.Components.Forms;

namespace DIGESA.Repositorios.Interfaces;

public interface IFileService
{
    Task<string> GuardarArchivoAsync(IBrowserFile file, int solicitudId);
    Task<List<TbDocumentoAdjunto>> ProcesarDocumentosAsync(Solicitud.DocumentosModel documentos, int solicitudId, Dictionary<string, int> tipoDocumentoMap);
    Task<List<TbDocumentoAdjunto>> GuardarArchivosAdjuntosAsync(Solicitud.DocumentosModel documentos, int solicitudId, Dictionary<string, int> tipoDocumentoMap);
}