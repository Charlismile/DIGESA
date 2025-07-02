using System.Collections.Generic;
using System.Threading.Tasks;
using DIGESA.Models.Entities.BDUbicaciones;

public interface IDbUbicacionesService
{
    Task<List<Provincia>> GetProvinciasAsync();
    Task<List<Distrito>> GetDistritosByProvinciaAsync(int provinciaId);
    Task<List<Corregimiento>> GetCorregimientosByDistritoAsync(int distritoId);
    Task<List<InstalacionSalud>> GetInstalacionesByCorregimientoAsync(int corregimientoId);
    Task<List<InstalacionSalud>> GetInstalacionesByProvinciaAsync(int provinciaId);
}