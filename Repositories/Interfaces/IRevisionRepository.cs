using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IRevisionRepository : IRepository<Revision>
    {
        Task<List<Revision>> GetBySolicitudAsync(int solicitudId);
    }
}