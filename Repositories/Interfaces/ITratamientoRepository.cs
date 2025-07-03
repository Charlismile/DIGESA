using DIGESA.Models.Entities.DBDIGESA;
using System.Threading.Tasks;

public interface ITratamientoRepository
{
    Task AddAsync(Tratamiento tratamiento);
}