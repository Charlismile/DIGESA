using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IMedicoRepository : IRepository<Medico>
    {
        Task<Medico?> GetByUserIdAsync(int userId);
    }
}