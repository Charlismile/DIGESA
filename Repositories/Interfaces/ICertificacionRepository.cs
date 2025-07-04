using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface ICertificacionRepository : IRepository<Certificacion>
    {
        Task<Certificacion?> GetBySolicitudIdAsync(int solicitudId);
    }
}