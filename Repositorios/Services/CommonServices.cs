using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class CommonServices : ICommon
{
    private readonly IDbContextFactory<DbContextDigesa> _Context;
    public CommonServices(IDbContextFactory<DbContextDigesa> Context)
    {
        _Context = Context;
       
    }

    public async Task<List<ListModel>> GetInstalaciones(string filtro)
    {
        try
        {
            await using var ctx = await _Context.CreateDbContextAsync();
            var query = ctx.TbInstalacionSalud.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
                query = query.Where(i => i.Nombre!.Contains(filtro));

            return await query
                .OrderBy(i => i.Nombre)
                .Select(x => new ListModel {
                    Id   = x.Id,
                    Name = x.Nombre ?? ""
                })
                .Take(20)        // límite de resultados
                .ToListAsync();
        }
        catch
        {
            return new List<ListModel>();
        }
    }


    public async Task<List<ListModel>> GetRegiones()
    {
        List<ListModel> Lista = new List<ListModel>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                Lista = await localContext.TbRegionSalud
                    .Select(x => new ListModel()
                    {
                        Id = x.Id,
                        Name = x.Nombre ?? "",
                    }).ToListAsync();
            }
        }
        catch (Exception)
        {
        }
        return Lista;
    }
    public async Task<List<ListModel>> GetProvincias()
    {
        List<ListModel> Lista = new List<ListModel>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                Lista = await localContext.TbProvincia
                    .Select(x => new ListModel()
                    {
                        Id = x.Id,
                        Name = x.NombreProvincia ?? "",
                    }).ToListAsync();
            }
        }
        catch (Exception)
        {
        }
        return Lista;
    }
    
    public async Task<List<ListModel>> GetDistritos(int ProvinciaId)
    {
        List<ListModel> Lista = new List<ListModel>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                Lista = await localContext.TbDistrito.Where(x => x.ProvinciaId == ProvinciaId)
                    .Select(x => new ListModel()
                    {
                        Id = x.Id,
                        Name = x.NombreDistrito ?? "",
                    }).ToListAsync();

                Lista = Lista.OrderBy(x => x.Name).ToList();
            }
        }
        catch (Exception)
        {
        }
        return Lista;
    }

    public async Task<List<ListModel>> GetCorregimientos(int DistritoId)
    {
        List<ListModel> Lista = new List<ListModel>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                Lista = await localContext.TbCorregimiento.Where(x => x.DistritoId == DistritoId)
                    .Select(x => new ListModel()
                    {
                        Id = x.Id,
                        Name = x.NombreCorregimiento ?? "",
                    }).ToListAsync();

                Lista = Lista.OrderBy(x => x.Name).ToList();
            }
        }
        catch (Exception)
        {
        }
        return Lista;
    }
    public async Task<List<ListaDiagnostico>> GetAllDiagnosticsAsync()
    {
        await using var context = _Context.CreateDbContext();
        return await context.ListaDiagnostico
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }
    
    public async Task<List<TbFormaFarmaceutica>> GetAllFormasAsync()
    {
        await using var context = _Context.CreateDbContext();
        return await context.TbFormaFarmaceutica
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }

    public async Task<List<TbViaAdministracion>> GetAllViaAdmAsync()
    {
        await using var context = _Context.CreateDbContext();
        return await context.TbViaAdministracion
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }
}