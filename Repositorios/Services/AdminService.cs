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
            // Option 1: If you want to count by EstadoSolicitud navigation property
            var totalSolicitudes = await _context.TbSolRegCannabis.CountAsync();

            // Count by joining with TbEstadoSolicitud
            var estados = await _context.TbEstadoSolicitud.ToListAsync();
            
            var aprobadas = await _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .CountAsync(s => s.EstadoSolicitud.NombreEstado == "Aprobada");
                
            var pendientes = await _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .CountAsync(s => s.EstadoSolicitud.NombreEstado == "Pendiente");
                
            var rechazadas = await _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .CountAsync(s => s.EstadoSolicitud.NombreEstado == "Denegada");

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

        // Alternative method using EstadoSolicitudId (more efficient)
        public async Task<AdminDashboardStats> ObtenerEstadisticasPorIdAsync()
        {
            // You'll need to know the ID values for each estado
            var totalSolicitudes = await _context.TbSolRegCannabis.CountAsync();
            
            var aprobadas = await _context.TbSolRegCannabis
                .CountAsync(s => s.EstadoSolicitudId == 1); // Assuming 1 = Aprobada
                
            var pendientes = await _context.TbSolRegCannabis
                .CountAsync(s => s.EstadoSolicitudId == 2); // Assuming 2 = Pendiente
                
            var rechazadas = await _context.TbSolRegCannabis
                .CountAsync(s => s.EstadoSolicitudId == 3); // Assuming 3 = Denegada

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

        // Additional useful methods for admin dashboard
        public async Task<List<TbSolRegCannabis>> ObtenerSolicitudesRecientesAsync(int count = 10)
        {
            return await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .Include(s => s.EstadoSolicitud)
                .OrderByDescending(s => s.FechaSolicitud)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<EstadisticasPorRegion>> ObtenerEstadisticasPorRegionAsync()
        {
            return await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .ThenInclude(p => p.Region)
                .GroupBy(s => s.Paciente.Region.Nombre)
                .Select(g => new EstadisticasPorRegion
                {
                    Region = g.Key,
                    TotalSolicitudes = g.Count(),
                    Aprobadas = g.Count(s => s.EstadoSolicitud.NombreEstado == "Aprobada"),
                    Pendientes = g.Count(s => s.EstadoSolicitud.NombreEstado == "Pendiente")
                })
                .ToListAsync();
        }
    }

    // Additional model for region statistics
    public class EstadisticasPorRegion
    {
        public string Region { get; set; }
        public int TotalSolicitudes { get; set; }
        public int Aprobadas { get; set; }
        public int Pendientes { get; set; }
    }
}