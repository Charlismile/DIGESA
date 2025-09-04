using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class PacienteService : IPaciente
{
    private readonly DbContextDigesa _context;

    public PacienteService(DbContextDigesa context)
    {
        _context = context;
    }
    public async Task<PacienteModel?> BuscarPorDocumentoAsync(string documento)
    {
        var pacienteEntity = await _context.TbPaciente
            .FirstOrDefaultAsync(p => p.NumDocCedula == documento || p.NumDocPasaporte == documento);

        if (pacienteEntity == null)
            return null;

        // Mapear manualmente
        return new PacienteModel
        {
            Id = pacienteEntity.Id,
            PrimerNombre = pacienteEntity.PrimerNombre,
            PrimerApellido = pacienteEntity.PrimerApellido,
            NumDocCedula = pacienteEntity.NumDocCedula,
            NumDocPasaporte = pacienteEntity.NumDocPasaporte?.Trim(),
        };
    }


    // Nuevo método para obtener estado del paciente
    public async Task<PacienteEstadoModel> GetEstadoPacienteAsync(string documento)
    {
        var solicitud = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Where(s => (s.Paciente.NumDocCedula == documento || s.Paciente.NumDocPasaporte == documento)
                        && s.EstadoSolicitud == "Aprobada")
            .OrderByDescending(s => s.FechaAprobacion)
            .FirstOrDefaultAsync();

        if (solicitud == null)
        {
            return new PacienteEstadoModel
            {
                Activo = false,
                FechaVencimiento = null,
                Nombre = null,
                Documento = documento
            };
        }

        var fechaVencimiento = solicitud.FechaAprobacion?.AddYears(2);

        return new PacienteEstadoModel
        {
            Activo = fechaVencimiento >= DateTime.Now,
            FechaVencimiento = fechaVencimiento,
            Nombre = solicitud.Paciente?.PrimerNombre,
            Apellido = solicitud.Paciente?.PrimerApellido,
            Documento = string.IsNullOrEmpty(solicitud.Paciente?.NumDocCedula)
                ? solicitud.Paciente?.NumDocPasaporte
                : solicitud.Paciente.NumDocCedula
        };
    }
}