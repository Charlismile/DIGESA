using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IEstadoSolicitudRepository : IRepository<EstadoSolicitud>
    {
        Task<EstadoSolicitud?> GetByNameAsync(string name);
    }
}