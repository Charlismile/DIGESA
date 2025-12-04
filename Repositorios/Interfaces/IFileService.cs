using DIGESA.Components.Pages.Public;
using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.AspNetCore.Components.Forms;

namespace DIGESA.Repositorios.Interfaces;

public interface IFileService
{
    Task<string> GuardarArchivoAsync(IBrowserFile file, int solicitudId);
    Task<List<TbDocumentoAdjunto>> GuardarArchivosAdjuntosAsync(List<IBrowserFile> archivos, int solicitudId, int tipoDocumentoId);
    Task<bool> EliminarArchivoAsync(int documentoId);
    Task<string> ObtenerRutaArchivoAsync(int documentoId);
}