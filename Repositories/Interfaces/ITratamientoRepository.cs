using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface ITratamientoRepository : IRepository<Tratamiento>
    {
        Task<Tratamiento?> GetBySolicitudIdAsync(int solicitudId);
    }
}