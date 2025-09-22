using DIGESA.Data;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services
{
    public class AdminService : IAdminService
    {
        private readonly DbContextDigesa _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminService(DbContextDigesa context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<AdminDashboardStats> ObtenerEstadisticasAsync()
        {
            var totalSolicitudes = await _context.TbSolRegCannabis.CountAsync();

            var aprobadas = await _context.TbSolRegCannabis.CountAsync(s => s.EstadoSolicitud == "Aprobada");
            var pendientes = await _context.TbSolRegCannabis.CountAsync(s => s.EstadoSolicitud == "Pendiente");
            var rechazadas = await _context.TbSolRegCannabis.CountAsync(s => s.EstadoSolicitud == "Denegada");

            var totalUsuarios = await _userManager.Users.CountAsync();

            return new AdminDashboardStats
            {
                TotalSolicitudes = totalSolicitudes,
                Aprobadas = aprobadas,
                Pendientes = pendientes,
                Rechazadas = rechazadas,
                TotalUsuarios = totalUsuarios
            };
        }
    }
}