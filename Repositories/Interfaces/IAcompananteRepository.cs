using DIGESA.Models.Entities.DBDIGESA;
using System.Threading.Tasks;

public interface IAcompananteRepository
{
    Task AddAsync(Acompanante acompanante);
    Task UpdateAsync(Acompanante acompanante);
    Task<Acompanante> GetByIdAsync(int id);
}