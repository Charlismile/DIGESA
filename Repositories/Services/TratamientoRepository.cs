using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class TratamientoRepository : ITratamientoRepository
{
    private readonly DbContextDigesa _context;

    public TratamientoRepository(DbContextDigesa context)
    {
        _context = context;
    }

    public async Task AddAsync(Tratamiento tratamiento)
    {
        await _context.Tratamiento.AddAsync(tratamiento);
        await _context.SaveChangesAsync();
    }
}