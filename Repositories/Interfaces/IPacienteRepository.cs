using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IPacienteRepository : IRepository<Paciente>
    {
        Task<Paciente?> GetByIdWithRelationsAsync(int id);
    }
}