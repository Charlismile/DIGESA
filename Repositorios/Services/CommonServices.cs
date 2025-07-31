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

    public async Task<List<ListModel>> GetInstalaciones()
    {
        List<ListModel> Lista = new List<ListModel>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                Lista = await localContext.TbInstalacionSalud
                    .Select(x => new ListModel()
                    {
                        Id = x.Id,
                        Name = x.NombreInstalacion ?? "",
                    }).ToListAsync();
            }
        }
        catch (Exception)
        {
        }
        return Lista;
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
                        Name = x.NombreRegion ?? "",
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
}