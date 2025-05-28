using DIGESA.Models.Entities.DIGESA;

namespace DIGESA.Services;

public class SolicitudService
{
    private readonly DbContextDigesa _context;

    public SolicitudService(DbContextDigesa context)
    {
        _context = context;
    }

    public async Task<int> CrearSolicitudAsync(int pacienteId, int medicoId, string motivo, List<int> diagnosticoIds, Tratamiento tratamiento)
    {
        var solicitud = new Solicitud
        {
            PacienteId = pacienteId,
            MedicoId = medicoId,
            MotivoSolicitud = motivo,
            Estado = "Pendiente",
            FechaSolicitud = DateTime.UtcNow
        };

        _context.Solicitudes.Add(solicitud);
        await _context.SaveChangesAsync();

        // Asociar diagnósticos
        foreach (var diagId in diagnosticoIds)
        {
            _context.SolicitudDiagnosticos.Add(new SolicitudDiagnostico
            {
                SolicitudId = solicitud.Id,
                DiagnosticoId = diagId
            });
        }

        // Agregar tratamiento
        if (tratamiento != null)
        {
            tratamiento.SolicitudId = solicitud.Id;
            _context.Tratamientos.Add(tratamiento);
        }

        await _context.SaveChangesAsync();
        return solicitud.Id;
    }

    public async Task<Solicitud> ObtenerSolicitudPorIdAsync(int id)
    {
        return await _context.Solicitudes
            .Include(s => s.Paciente)
            .Include(s => s.Medico)
            .Include(s => s.Tratamiento)
            .Include(s => s.SolicitudDiagnosticos)
                .ThenInclude(sd => sd.Diagnostico)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Solicitud>> ObtenerSolicitudesPorPacienteAsync(int pacienteId)
    {
        return await _context.Solicitudes
            .Where(s => s.PacienteId == pacienteId)
            .OrderByDescending(s => s.FechaSolicitud)
            .ToListAsync();
    }

    public async Task CambiarEstadoSolicitudAsync(int solicitudId, string nuevoEstado, string observaciones, int revisorId)
    {
        var solicitud = await _context.Solicitudes.FindAsync(solicitudId);
        if (solicitud == null) throw new Exception("Solicitud no encontrada");

        solicitud.Estado = nuevoEstado;

        _context.RevisionMedicas.Add(new RevisionMedica
        {
            SolicitudId = solicitudId,
            RevisorId = revisorId,
            Observaciones = observaciones,
            Aprobado = nuevoEstado == "Aprobada",
            FechaRevision = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }
}
