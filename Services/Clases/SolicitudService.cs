using DIGESA.Data;
using DIGESA.Models.Entities.DIGESA;
using DIGESA.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class SolicitudService : ISolicitudService
{
    private readonly DbContextDigesa _context;

    public SolicitudService(DbContextDigesa context)
    {
        _context = context;
    }

    public async Task GuardarSolicitud(Solicitud solicitud)
    {
        _context.Solicitudes.Add(solicitud);
        await _context.SaveChangesAsync();
    }

    public async Task<Solicitud?> ObtenerSolicitudPorId(int id)
    {
        return await _context.Solicitudes
            .Include(s => s.Paciente)
            .Include(s => s.Medico)
            .Include(s => s.Acompanante)
            .Include(s => s.Certificacion)
            .Include(s => s.DocumentoAdjuntos)
            .Include(s => s.Revisiones)
            .Include(s => s.SolicitudDiagnosticos)
            .Include(s => s.Tratamientos)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    public async Task<List<Solicitud>> ObtenerSolicitudesPendientes()
    {
        return await _context.Solicitudes.Where(s => s.Estado == "Pendiente").ToListAsync();
    }

    public async Task ActualizarSolicitud(Solicitud solicitud)
    {
        _context.Solicitudes.Update(solicitud);
        await _context.SaveChangesAsync();
    }

}