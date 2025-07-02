using DIGESA.Models.Entities.BDUbicaciones;
using Microsoft.EntityFrameworkCore;

public class DbUbicacionesService : IDbUbicacionesService
{
    private readonly DbUbicacionPanama _context;

    public DbUbicacionesService(DbUbicacionPanama context)
    {
        _context = context;
    }

    public async Task<List<Provincia>> GetProvinciasAsync() =>
        await _context.Provincia.ToListAsync();

    public async Task<List<Distrito>> GetDistritosByProvinciaAsync(int provinciaId) =>
        await _context.Distrito.Where(d => d.ProvinciaId == provinciaId).ToListAsync();

    public async Task<List<Corregimiento>> GetCorregimientosByDistritoAsync(int distritoId) =>
        await _context.Corregimiento.Where(c => c.DistritoId == distritoId).ToListAsync();

    public async Task<List<InstalacionSalud>> GetInstalacionesByCorregimientoAsync(int corregimientoId)
    {
        // Implementa lógica específica si las instalaciones están ligadas a corregimiento
        return await _context.InstalacionSalud.ToListAsync();
    }

    public async Task<List<InstalacionSalud>> GetInstalacionesByProvinciaAsync(int provinciaId)
    {
        // Puedes filtrar según tus reglas de negocio
        return await _context.InstalacionSalud.Take(100).ToListAsync(); // Ejemplo
    }
}