using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class CommonServices : ICommon
{
    private readonly DbContextDigesa _context;
    private readonly IConfiguration _configuration;

    public CommonServices(DbContextDigesa context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public Task<string> GetFakePassword()
        => Task.FromResult(_configuration["FakePass"] ?? "");

    public async Task<List<ListModel>> GetInstalaciones(string filtro)
    {
        var query = _context.TbInstalacionSalud.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filtro))
            query = query.Where(x => x.Nombre!.Contains(filtro));

        return await query.Select(x => new ListModel { Id = x.Id, Name = x.Nombre ?? "" })
                          .Take(20)
                          .ToListAsync();
    }

    public async Task<List<ListModel>> GetRegiones()
        => await _context.TbRegionSalud.Select(x => new ListModel { Id = x.Id, Name = x.Nombre ?? "" }).ToListAsync();

    public async Task<List<ListModel>> GetProvincias()
        => await _context.TbProvincia.Select(x => new ListModel { Id = x.Id, Name = x.NombreProvincia ?? "" }).ToListAsync();

    public async Task<List<ListModel>> GetDistritos(int ProvinciaId)
        => await _context.TbDistrito.Where(x => x.ProvinciaId == ProvinciaId)
                                    .Select(x => new ListModel { Id = x.Id, Name = x.NombreDistrito ?? "" })
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();

    public async Task<List<ListModel>> GetCorregimientos(int DistritoId)
        => await _context.TbCorregimiento.Where(x => x.DistritoId == DistritoId)
                                         .Select(x => new ListModel { Id = x.Id, Name = x.NombreCorregimiento ?? "" })
                                         .OrderBy(x => x.Name)
                                         .ToListAsync();

    public async Task<List<ListModel>> GetUnidadId()
        => await _context.TbUnidades.Select(x => new ListModel { Id = x.Id, Name = x.NombreUnidad ?? "" }).ToListAsync();

    public async Task<List<ListaDiagnostico>> GetAllDiagnosticsAsync()
        => await _context.ListaDiagnostico.OrderBy(c => c.Nombre).ToListAsync();

    public async Task<List<TbFormaFarmaceutica>> GetAllFormasAsync()
        => await _context.TbFormaFarmaceutica.OrderBy(c => c.Nombre).ToListAsync();

    public async Task<List<TbViaAdministracion>> GetAllViaAdmAsync()
        => await _context.TbViaAdministracion.OrderBy(c => c.Nombre).ToListAsync();
}
