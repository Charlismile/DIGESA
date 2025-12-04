using DIGESA.Data;
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

    // Datos geográficos
    public async Task<List<ItemListModel>> GetRegiones()
    {
        return await _context.TbRegionSalud
            .Select(x => new ItemListModel 
            { 
                Id = x.Id, 
                Name = x.Nombre ?? "Sin nombre" 
            })
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetProvincias()
    {
        return await _context.TbProvincia
            .Select(x => new ItemListModel 
            { 
                Id = x.Id, 
                Name = x.NombreProvincia ?? "Sin nombre" 
            })
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetDistritos(int provinciaId)
    {
        return await _context.TbDistrito
            .Where(x => x.ProvinciaId == provinciaId)
            .Select(x => new ItemListModel 
            { 
                Id = x.Id, 
                Name = x.NombreDistrito ?? "Sin nombre" 
            })
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetCorregimientos(int distritoId)
    {
        return await _context.TbCorregimiento
            .Where(x => x.DistritoId == distritoId)
            .Select(x => new ItemListModel 
            { 
                Id = x.Id, 
                Name = x.NombreCorregimiento ?? "Sin nombre" 
            })
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    // Datos de salud
    public async Task<List<ItemListModel>> GetInstalaciones(string? filtro = null)
    {
        var query = _context.TbInstalacionSalud.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filtro))
        {
            query = query.Where(x => x.Nombre != null && x.Nombre.Contains(filtro));
        }

        return await query
            .Select(x => new ItemListModel 
            { 
                Id = x.Id, 
                Name = x.Nombre ?? "Sin nombre" 
            })
            .OrderBy(x => x.Name)
            .Take(20)
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetAllDiagnosticosAsync()
    {
        return await _context.ListaDiagnostico
            .Where(d => d.IsActivo)
            .Select(d => new ItemListModel 
            { 
                Id = d.Id, 
                Name = d.Nombre ?? "Sin nombre" 
            })
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetAllFormasFarmaceuticasAsync()
    {
        return await _context.TbFormaFarmaceutica
            .Where(f => f.IsActivo)
            .Select(f => new ItemListModel 
            { 
                Id = f.Id, 
                Name = f.Nombre 
            })
            .OrderBy(f => f.Name)
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetAllViasAdministracionAsync()
    {
        return await _context.TbViaAdministracion
            .Where(v => v.IsActivo)
            .Select(v => new ItemListModel 
            { 
                Id = v.Id, 
                Name = v.Nombre 
            })
            .OrderBy(v => v.Name)
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetAllUnidadesAsync()
    {
        return await _context.TbUnidades
            .Where(u => u.IsActivo)
            .Select(u => new ItemListModel 
            { 
                Id = u.Id, 
                Name = u.NombreUnidad 
            })
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    // Catálogos misceláneos
    public async Task<List<ItemListModel>> GetTiposDocumentoAdjuntoAsync()
    {
        return await _context.TbTipoDocumentoAdjunto
            .Where(t => t.IsActivo == true)
            .Select(t => new ItemListModel 
            { 
                Id = t.Id, 
                Name = t.Nombre 
            })
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<List<ItemListModel>> GetEstadosSolicitudAsync()
    {
        return await _context.TbEstadoSolicitud
            .Select(e => new ItemListModel 
            { 
                Id = e.IdEstado, 
                Name = e.NombreEstado 
            })
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    // Utilidades
    public async Task<string> GetFakePassword()
    {
        return await Task.FromResult(_configuration["FakePass"] ?? "DIGESA2024!");
    }

    public async Task<int?> CrearInstalacionPersonalizadaAsync(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return null;

        var nombreNormalizado = nombre.Trim();
        var instalacionExistente = await _context.TbInstalacionSalud
            .FirstOrDefaultAsync(i => i.Nombre != null && i.Nombre.ToLower() == nombreNormalizado.ToLower());

        if (instalacionExistente != null)
            return instalacionExistente.Id;

        var nuevaInstalacion = new TbInstalacionSalud
        {
            Nombre = nombreNormalizado
        };

        _context.TbInstalacionSalud.Add(nuevaInstalacion);
        await _context.SaveChangesAsync();

        return nuevaInstalacion.Id;
    }
}