using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Services;

namespace DIGESA.Repositorios.Interfaces;

public interface IAdminService
{
    Task<AdminDashboardStats> ObtenerEstadisticasAsync();
    Task<List<TbSolRegCannabis>> ObtenerSolicitudesRecientesAsync(int count = 10);
    Task<List<EstadisticasPorRegion>> ObtenerEstadisticasPorRegionAsync();
}