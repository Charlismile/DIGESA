using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositories.Interfaces
{
    public interface IDecisionRevisionRepository : IRepository<DecisionRevision>
    {
        Task<DecisionRevision?> GetByNameAsync(string nombre);
    }
}