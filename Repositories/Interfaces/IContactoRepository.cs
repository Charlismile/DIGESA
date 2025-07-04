using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IContactoRepository : IRepository<Contacto>
    {
        Task<List<Contacto>> GetByPropietarioAsync(string propietarioTipo, int propietarioId);
    }
}