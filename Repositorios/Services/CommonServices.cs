using DIGESA.DTOs;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class CommonServices : ICommon
{
    private readonly IDbContextFactory<DbContextDigesa> _Context;
    private readonly IConfiguration _Configuration;

    public CommonServices(IDbContextFactory<DbContextDigesa> Context, IConfiguration Configuration)
    {
        _Context = Context;
        _Configuration = Configuration;
    }

    public async Task<string> GetFakePassword()
    {
        string Password = _Configuration.GetSection("FakePass").Value ?? "";
        return await Task.FromResult(Password);
    }

    public async Task<List<UbicacionDto>> ObtenerRegionesAsync()
    {
        var lista = new List<UbicacionDto>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                lista = await localContext.TbRegionSalud
                    .Select(x => new UbicacionDto()
                    {
                        RId = x.RegionSaludId,
                        NombreRegion = x.Nombre
                    }).ToListAsync();
            }
        }
        catch (Exception)
        {
            // manejar errores o log
        }

        return lista;
    }

    public async Task<List<ListDto>> GetProvincias()
    {
        List<ListDto> Lista = new List<ListDto>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                Lista = await localContext.Provincia
                    .Select(x => new ListDto()
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

    public async Task<List<ListDto>> GetDistritosPorProvincia(int provinciaId)

    {
        List<ListDto> Lista = new List<ListDto>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                Lista = await localContext.Distrito.Where(x => x.ProvinciaId == provinciaId)
                    .Select(x => new ListDto()
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

    public async Task<List<ListDto>> GetCorregimientos(int distritoId)
    {
        List<ListDto> Lista = new List<ListDto>();
        try
        {
            using (var localContext = await _Context.CreateDbContextAsync())
            {
                Lista = await localContext.Corregimiento.Where(x => x.DistritoId == distritoId)
                    .Select(x => new ListDto()
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