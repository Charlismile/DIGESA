using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositories.Services;

public class RepositoryBase<T> : IRepository<T> where T : class
{
    protected readonly DbContextDigesa Context;

    public RepositoryBase(DbContextDigesa context)
    {
        Context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            Context.Set<T>().Remove(entity);
    }
}