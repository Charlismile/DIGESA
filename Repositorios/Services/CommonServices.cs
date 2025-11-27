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

    // MÉTODO FALTANTE AGREGADO
    public async Task<List<ItemListModel>> GetAllUnidadesAsync()
    {
        return await _context.TbUnidades
            .Where(u => u.IsActivo)
            .Select(x => new ItemListModel { Id = x.Id, Name = x.NombreUnidad ?? "" })
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    // MÉTODO ACTUALIZADO para usar GetAllUnidadesAsync
    public async Task<List<ItemListModel>> GetUnidadId()
    {
        return await GetAllUnidadesAsync(); // Reutiliza la misma lógica
    }

    // Los demás métodos permanecen igual...
    public Task<string> GetFakePassword()
        => Task.FromResult(_configuration["FakePass"] ?? "");

    public async Task<List<ItemListModel>> GetAllFormasFarmaceuticasAsync()
    {
        return await _context.TbFormaFarmaceutica
            .Where(f => f.IsActivo == true)
            .Select(f => new ItemListModel { Id = f.Id, Name = f.Nombre })
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetAllViasAdministracionAsync()
    {
        return await _context.TbViaAdministracion
            .Where(v => v.IsActivo == true)
            .Select(v => new ItemListModel { Id = v.Id, Name = v.Nombre })
            .ToListAsync();
    }
    
    public async Task<List<ItemListModel>> GetInstalaciones(string filtro)
    {
        var query = _context.TbInstalacionSalud.AsQueryable();
        if (!string.IsNullOrWhiteSpace(filtro))
            query = query.Where(x => x.Nombre!.Contains(filtro));

        return await query.Select(x => new ItemListModel { Id = x.Id, Name = x.Nombre ?? "" })
                          .Take(20)
                          .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetRegiones()
        => await _context.TbRegionSalud.Select(x => new ItemListModel { Id = x.Id, Name = x.Nombre ?? "" }).ToListAsync();

    public async Task<List<ItemListModel>> GetProvincias()
        => await _context.TbProvincia.Select(x => new ItemListModel { Id = x.Id, Name = x.NombreProvincia ?? "" }).ToListAsync();

    public async Task<List<ItemListModel>> GetDistritos(int ProvinciaId)
        => await _context.TbDistrito.Where(x => x.ProvinciaId == ProvinciaId)
                                    .Select(x => new ItemListModel { Id = x.Id, Name = x.NombreDistrito ?? "" })
                                    .OrderBy(x => x.Name)
                                    .ToListAsync();

    public async Task<List<ItemListModel>> GetCorregimientos(int DistritoId)
        => await _context.TbCorregimiento.Where(x => x.DistritoId == DistritoId)
                                         .Select(x => new ItemListModel { Id = x.Id, Name = x.NombreCorregimiento ?? "" })
                                         .OrderBy(x => x.Name)
                                         .ToListAsync();

    public async Task<List<ListaDiagnostico>> GetAllDiagnosticsAsync()
        => await _context.ListaDiagnostico.Where(d => d.IsActivo).OrderBy(c => c.Nombre).ToListAsync();

    public async Task<List<TbFormaFarmaceutica>> GetAllFormasAsync()
        => await _context.TbFormaFarmaceutica.Where(f => f.IsActivo).OrderBy(c => c.Nombre).ToListAsync();

    public async Task<List<TbViaAdministracion>> GetAllViaAdmAsync()
        => await _context.TbViaAdministracion.Where(v => v.IsActivo).OrderBy(c => c.Nombre).ToListAsync();
}