using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class SolicitudService : ISolicitudService
{
    private readonly DbContextDigesa _context;
    public SolicitudService(DbContextDigesa context) => _context = context;

    public async Task<List<SolicitudModel>> ObtenerSolicitudesAsync()
    {
        return await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Select(s => new SolicitudModel
            {
                Id = s.Id,
                NumSolCompleta = s.NumSolCompleta,
                FechaSolicitud = s.FechaSolicitud,
                EstadoSolicitud = s.EstadoSolicitud,
                PacienteNombre = s.Paciente.PrimerNombre + " " + s.Paciente.PrimerApellido,
                PacienteCedula = s.Paciente.NumDocCedula
            })
            .ToListAsync();
    }

    public async Task<Dictionary<string,int>> ObtenerConteoPorEstadoAsync()
    {
        return await _context.TbSolRegCannabis
            .GroupBy(s => s.EstadoSolicitud)
            .Select(g => new { Estado = g.Key ?? "Desconocido", Cantidad = g.Count() })
            .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);
    }
}