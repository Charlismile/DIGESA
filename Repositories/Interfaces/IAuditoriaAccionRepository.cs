using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IAuditoriaAccionRepository : IRepository<AuditoriaAccion>
    {
        Task<List<AuditoriaAccion>> GetByUsuarioIdAsync(int usuarioId);
    }
}