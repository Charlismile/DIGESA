using DIGESA.Data;
using DIGESA.Models.Entities.DIGESA;
using DIGESA.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly DbContextDigesa _db;

        public SolicitudService(DbContextDigesa db)
        {
            _db = db;
        }

        public async Task<List<Solicitud>> ObtenerSolicitudesPendientes()
        {
            return await _db.Solicitudes
                .Include(s => s.Paciente)
                .Where(s => s.Estado == "Pendiente")
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<Solicitud?> ObtenerSolicitudPorId(int id)
        {
            return await _db.Solicitudes
                .Include(s => s.Paciente)
                .Include(s => s.Medico)
                .Include(s => s.Acompanante)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task ActualizarSolicitud(Solicitud solicitud)
        {
            _db.Solicitudes.Update(solicitud);
            await _db.SaveChangesAsync();
        }

        public async Task<Usuario?> ObtenerUsuarioPorEmail(string email)
        {
            return await _db.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}