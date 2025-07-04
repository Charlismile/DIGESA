using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IAcompananteRepository : IRepository<Acompanante>
    {
        Task<Acompanante?> GetByPacienteIdAsync(int pacienteId);
    }
}