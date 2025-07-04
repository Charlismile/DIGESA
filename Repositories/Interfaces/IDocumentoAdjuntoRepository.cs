using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IDocumentoAdjuntoRepository : IRepository<DocumentoAdjunto>
    {
        Task<List<DocumentoAdjunto>> GetBySolicitudAsync(int solicitudId);
    }
}