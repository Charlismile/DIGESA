using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IRolRepository : IRepository<Rol>
    {
        Task<Rol?> GetByNameAsync(string nombre);
    }
}