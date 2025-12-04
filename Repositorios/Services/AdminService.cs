using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class AdminService : IAdminService
{
    private readonly DbContextDigesa _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(DbContextDigesa context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<EstadisticasDashboardModel> ObtenerEstadisticasAsync()
    {
        var estadisticas = new EstadisticasDashboardModel();

        try
        {
            // Total de solicitudes por estado
            var solicitudesPorEstado = await _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .GroupBy(s => s.EstadoSolicitud.NombreEstado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            estadisticas.TotalSolicitudes = solicitudesPorEstado.Sum(x => x.Total);
            estadisticas.SolicitudesAprobadas = solicitudesPorEstado.FirstOrDefault(x => x.Estado == "Aprobada")?.Total ?? 0;
            estadisticas.SolicitudesPendientes = solicitudesPorEstado.FirstOrDefault(x => x.Estado == "Pendiente")?.Total ?? 0;
            estadisticas.SolicitudesRechazadas = solicitudesPorEstado.FirstOrDefault(x => x.Estado == "Rechazada")?.Total ?? 0;

            // Total de usuarios
            estadisticas.TotalUsuarios = await _userManager.Users.CountAsync();
            estadisticas.UsuariosAprobados = await _userManager.Users.CountAsync(u => u.IsAproved);
            estadisticas.UsuariosPendientes = await _userManager.Users.CountAsync(u => !u.IsAproved);

            // Total de pacientes únicos
            estadisticas.TotalPacientes = await _context.TbPaciente.CountAsync();

            // Carnets activos y por vencer
            var hoy = DateTime.Now;
            estadisticas.CarnetsActivos = await _context.TbSolRegCannabis
                .Where(s => s.CarnetActivo == true && s.FechaVencimientoCarnet > hoy)
                .CountAsync();

            estadisticas.CarnetsPorVencer = await _context.TbSolRegCannabis
                .Where(s => s.CarnetActivo == true && 
                           s.FechaVencimientoCarnet > hoy && 
                           s.FechaVencimientoCarnet <= hoy.AddDays(30))
                .CountAsync();

            // Estadísticas por región
            estadisticas.SolicitudesPorRegion = await ObtenerEstadisticasPorRegionAsync();

            // Estadísticas por mes (últimos 6 meses)
            var fechaInicio = hoy.AddMonths(-6);
            estadisticas.SolicitudesPorMes = await _context.TbSolRegCannabis
                .Where(s => s.FechaSolicitud >= fechaInicio)
                .GroupBy(s => new { s.FechaSolicitud.Value.Year, s.FechaSolicitud.Value.Month })
                .Select(g => new
                {
                    Mes = $"{g.Key.Year}-{g.Key.Month:00}",
                    Total = g.Count()
                })
                .ToDictionaryAsync(x => x.Mes, x => x.Total);

            estadisticas.AprobacionesPorMes = await _context.TbSolRegCannabis
                .Where(s => s.FechaAprobacion >= fechaInicio && s.EstadoSolicitud.NombreEstado == "Aprobada")
                .GroupBy(s => new { s.FechaAprobacion.Value.Year, s.FechaAprobacion.Value.Month })
                .Select(g => new
                {
                    Mes = $"{g.Key.Year}-{g.Key.Month:00}",
                    Total = g.Count()
                })
                .ToDictionaryAsync(x => x.Mes, x => x.Total);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en ObtenerEstadisticasAsync: {ex.Message}");
        }

        return estadisticas;
    }

    public async Task<List<SolicitudListModel>> ObtenerSolicitudesRecientesAsync(int count = 10)
    {
        return await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .OrderByDescending(s => s.FechaSolicitud)
            .Take(count)
            .Select(s => new SolicitudListModel
            {
                Id = s.Id,
                NumeroSolicitud = s.NumSolCompleta ?? "N/A",
                FechaSolicitud = s.FechaSolicitud ?? DateTime.MinValue,
                Estado = s.EstadoSolicitud.NombreEstado,
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteDocumento = !string.IsNullOrEmpty(s.Paciente.DocumentoCedula) 
                    ? s.Paciente.DocumentoCedula 
                    : s.Paciente.DocumentoPasaporte ?? "N/A",
                PacienteCorreo = s.Paciente.CorreoElectronico ?? string.Empty,
                EsRenovacion = s.EsRenovacion ?? false,
                NumeroCarnet = s.NumeroCarnet,
                CarnetActivo = s.CarnetActivo ?? false,
                FechaVencimientoCarnet = s.FechaVencimientoCarnet
            })
            .ToListAsync();
    }

    public async Task<List<EstadisticasPorRegion>> ObtenerEstadisticasPorRegionAsync()
    {
        return await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
                .ThenInclude(p => p.Region)
            .Include(s => s.EstadoSolicitud)
            .Where(s => s.Paciente.Region != null)
            .GroupBy(s => s.Paciente.Region.Nombre)
            .Select(g => new EstadisticasPorRegion
            {
                Region = g.Key ?? "Sin región",
                TotalSolicitudes = g.Count(),
                SolicitudesAprobadas = g.Count(s => s.EstadoSolicitud.NombreEstado == "Aprobada"),
                SolicitudesPendientes = g.Count(s => s.EstadoSolicitud.NombreEstado == "Pendiente"),
                SolicitudesRechazadas = g.Count(s => s.EstadoSolicitud.NombreEstado == "Rechazada")
            })
            .OrderByDescending(e => e.TotalSolicitudes)
            .ToListAsync();
    }

    public async Task<DashboardVencimientosModel> ObtenerDashboardVencimientosAsync()
    {
        var hoy = DateTime.Now;
        var modelo = new DashboardVencimientosModel();

        var carnets = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Where(s => s.CarnetActivo == true && s.FechaVencimientoCarnet.HasValue)
            .ToListAsync();

        foreach (var carnet in carnets)
        {
            if (!carnet.FechaVencimientoCarnet.HasValue) continue;

            var diasRestantes = (carnet.FechaVencimientoCarnet.Value - hoy).Days;

            if (diasRestantes <= 0)
            {
                modelo.CarnetsVencidos++;
            }
            else if (diasRestantes <= 7)
            {
                modelo.CarnetsPorVencer7Dias++;
            }
            else if (diasRestantes <= 15)
            {
                modelo.CarnetsPorVencer15Dias++;
            }
            else if (diasRestantes <= 30)
            {
                modelo.CarnetsPorVencer30Dias++;
            }
            else
            {
                modelo.CarnetsVigentes++;
            }
        }

        // Proximos vencimientos (próximos 30 días)
        modelo.ProximosVencimientos = carnets
            .Where(c => c.FechaVencimientoCarnet.HasValue && 
                       c.FechaVencimientoCarnet.Value > hoy && 
                       c.FechaVencimientoCarnet.Value <= hoy.AddDays(30))
            .OrderBy(c => c.FechaVencimientoCarnet)
            .Take(10)
            .Select(c => new NotificacionVencimientoModel
            {
                SolicitudId = c.Id,
                NumeroSolicitud = c.NumSolCompleta ?? "N/A",
                NumeroCarnet = c.NumeroCarnet ?? "N/A",
                PacienteNombre = $"{c.Paciente?.PrimerNombre} {c.Paciente?.PrimerApellido}",
                PacienteCorreo = c.Paciente?.CorreoElectronico ?? string.Empty,
                FechaVencimiento = c.FechaVencimientoCarnet.Value,
                DiasRestantes = (c.FechaVencimientoCarnet.Value - hoy).Days
            })
            .ToList();

        return modelo;
    }
}