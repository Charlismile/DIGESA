using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IDiagnosticoRepository : IRepository<Diagnostico>
    {
        Task<List<Diagnostico>> GetDiagnosticosPredefinidosAsync();
    }
}