using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class SolicitudService : ISolicitudService
{
    private readonly DbContextDigesa _context;
    public SolicitudService(DbContextDigesa context) => _context = context;

    // 🔹 1. Obtener todas las solicitudes
    public async Task<List<SolicitudModel>> ObtenerSolicitudesAsync()
    {
        return await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .Select(s => new SolicitudModel
            {
                Id = s.Id,
                NumSolCompleta = s.NumSolCompleta,
                FechaSolicitud = s.FechaSolicitud,
                EstadoSolicitud = s.EstadoSolicitud.NombreEstado,
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteCedula = s.Paciente.DocumentoCedula
            })
            .ToListAsync();
    }

    // 🔹 2. Conteo por estado
    public async Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync()
    {
        return await _context.TbSolRegCannabis
            .Include(s => s.EstadoSolicitud)
            .GroupBy(s => s.EstadoSolicitud.NombreEstado)
            .Select(g => new { Estado = g.Key ?? "Desconocido", Cantidad = g.Count() })
            .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);
    }

    // 🔹 3. Solicitudes pendientes o en revisión
    public async Task<List<SolicitudModel>> ObtenerSolicitudesPendientesORevisionAsync()
    {
        return await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .Where(s => s.EstadoSolicitud.NombreEstado == "Pendiente" ||
                        s.EstadoSolicitud.NombreEstado == "En Revisión")
            .Select(s => new SolicitudModel
            {
                Id = s.Id,
                NumSolCompleta = s.NumSolCompleta,
                FechaSolicitud = s.FechaSolicitud,
                EstadoSolicitud = s.EstadoSolicitud.NombreEstado,
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteCedula = s.Paciente.DocumentoCedula
            })
            .ToListAsync();
    }

    // 🔹 4. Obtener detalle completo de una solicitud
    public async Task<SolicitudDetalleModel?> ObtenerDetalleSolicitudAsync(int id)
    {
        var solicitud = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .Include(s => s.TbDeclaracionJurada)
            .Include(s => s.TbDocumentoAdjunto)
            .ThenInclude(d => d.TipoDocumento)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (solicitud == null) return null;

        return new SolicitudDetalleModel
        {
            Id = solicitud.Id,
            NumSolCompleta = solicitud.NumSolCompleta,
            FechaSolicitud = solicitud.FechaSolicitud,
            EstadoSolicitud = solicitud.EstadoSolicitud?.NombreEstado,
            PacienteNombre = $"{solicitud.Paciente?.PrimerNombre} {solicitud.Paciente?.PrimerApellido}",
            PacienteCedula = solicitud.Paciente?.DocumentoCedula,
            ComentarioRevision = solicitud.ComentarioRevision,
            DocumentosAdjuntos = solicitud.TbDocumentoAdjunto?
                .Select(d => d.NombreOriginal ?? d.NombreGuardado ?? "Sin nombre")
                .ToList() ?? new List<string>(),
            Declaraciones = solicitud.TbDeclaracionJurada?
                .Select(d => d.Detalle ?? "Sin detalle")
                .ToList() ?? new List<string>()
        };
    }

    // 🔹 5. Actualizar estado y registrar datos del revisor
    public async Task<bool> ActualizarEstadoSolicitudAsync(int id, string nuevoEstado, string usuarioRevisor,
        string? comentario = null)
    {
        var solicitud = await _context.TbSolRegCannabis
            .FirstOrDefaultAsync(s => s.Id == id);

        if (solicitud == null) return false;

        // Buscar el ID del estado por nombre
        var estado = await _context.TbEstadoSolicitud
            .FirstOrDefaultAsync(e => e.NombreEstado == nuevoEstado);

        if (estado == null) return false;

        solicitud.EstadoSolicitudId = estado.IdEstado;
        solicitud.UsuarioRevisor = usuarioRevisor;
        solicitud.ComentarioRevision = comentario;
        solicitud.ModificadaPor = usuarioRevisor;
        solicitud.ModificadaEn = DateOnly.FromDateTime(DateTime.Today);

        if (nuevoEstado == "Aprobada")
            solicitud.FechaAprobacion = DateTime.Now;

        _context.TbSolRegCannabis.Update(solicitud);

        // CORRECCIÓN: Pasar el nombre del estado (string) en lugar del ID (int)
        await RegistrarHistorialCambioAsync(solicitud.Id, nuevoEstado, usuarioRevisor, comentario);

        await _context.SaveChangesAsync();
        return true;
    }

// 🔹 6. Registrar cambios en el historial (VERSIÓN SIMPLIFICADA)
    public async Task<bool> RegistrarHistorialCambioAsync(int solicitudId, string nombreEstado, string usuarioRevisor,
        string? comentario)
    {
        // Buscar el ID del estado por su nombre
        var estado = await _context.TbEstadoSolicitud
            .FirstOrDefaultAsync(e => e.NombreEstado == nombreEstado);

        if (estado == null) return false;

        var historial = new TbSolRegCannabisHistorial
        {
            SolRegCannabisId = solicitudId,
            EstadoSolicitudIdHistorial = estado.IdEstado, 
            UsuarioRevisor = usuarioRevisor,
            FechaCambio = DateOnly.FromDateTime(DateTime.Today), 
            Comentario = comentario
        };

        await _context.TbSolRegCannabisHistorial.AddAsync(historial);
        return true;
    }

    // 🔹 7. Método adicional: Obtener historial de una solicitud
    public async Task<List<TbSolRegCannabisHistorial>> ObtenerHistorialSolicitudAsync(int solicitudId)
    {
        return await _context.TbSolRegCannabisHistorial
            .Include(h => h.EstadoSolicitudIdHistorialNavigation)
            .Where(h => h.SolRegCannabisId == solicitudId)
            .OrderByDescending(h => h.FechaCambio)
            .ToListAsync();
    }

    // 🔹 8. Método adicional: Buscar solicitudes por cédula del paciente
    public async Task<List<SolicitudModel>> BuscarSolicitudesPorCedulaAsync(string cedula)
    {
        return await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .Where(s => s.Paciente.DocumentoCedula.Contains(cedula))
            .Select(s => new SolicitudModel
            {
                Id = s.Id,
                NumSolCompleta = s.NumSolCompleta,
                FechaSolicitud = s.FechaSolicitud,
                EstadoSolicitud = s.EstadoSolicitud.NombreEstado,
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteCedula = s.Paciente.DocumentoCedula
            })
            .ToListAsync();
    }
}