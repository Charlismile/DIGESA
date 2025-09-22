using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IAdminService
{
    Task<AdminDashboardStats> ObtenerEstadisticasAsync();
}