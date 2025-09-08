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
        var documentoNormalizado = documento.Trim();

        var pacienteEntity = await _context.TbPaciente
            .FirstOrDefaultAsync(p => p.NumDocCedula == documentoNormalizado || 
                                     p.NumDocPasaporte == documentoNormalizado);

        if (pacienteEntity == null)
            return null;

        return new PacienteModel
        {
            Id = pacienteEntity.Id,
            PrimerNombre = pacienteEntity.PrimerNombre,
            SegundoNombre = pacienteEntity.SegundoNombre,
            PrimerApellido = pacienteEntity.PrimerApellido,
            SegundoApellido = pacienteEntity.SegundoApellido,
            NumDocCedula = pacienteEntity.NumDocCedula,
            NumDocPasaporte = pacienteEntity.NumDocPasaporte,
            Nacionalidad = pacienteEntity.Nacionalidad,
            TelefonoResidencialPersonal = pacienteEntity.TelefonoPersonal,
            TelefonoLaboral = pacienteEntity.TelefonoLaboral,
            CorreoElectronico = pacienteEntity.CorreoElectronico,
            TipoDocumentoPacienteEnum = string.IsNullOrEmpty(pacienteEntity.NumDocCedula) ? 
                TipoDocumentoPaciente.Pasaporte : TipoDocumentoPaciente.Cedula
        };
    }

    public async Task<PacienteEstadoModel> GetEstadoPacienteAsync(string documento)
    {
        var documentoNormalizado = documento.Trim();

        // Buscar la última solicitud aprobada para este documento
        var solicitud = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Where(s => (s.Paciente.NumDocCedula == documentoNormalizado || 
                        s.Paciente.NumDocPasaporte == documentoNormalizado) &&
                        s.EstadoSolicitud == "Aprobada")
            .OrderByDescending(s => s.FechaAprobacion)
            .FirstOrDefaultAsync();

        if (solicitud == null || solicitud.Paciente == null)
        {
            return new PacienteEstadoModel
            {
                Activo = false,
                FechaVencimiento = null,
                Nombre = null,
                Apellido = null,
                Documento = documentoNormalizado
            };
        }

        // Calcular fecha de vencimiento (2 años después de la aprobación)
        var fechaVencimiento = solicitud.FechaAprobacion?.AddYears(2);
        var estaActivo = fechaVencimiento >= DateTime.Now;

        return new PacienteEstadoModel
        {
            Activo = estaActivo,
            FechaVencimiento = fechaVencimiento,
            Nombre = solicitud.Paciente.PrimerNombre,
            Apellido = solicitud.Paciente.PrimerApellido,
            Documento = string.IsNullOrEmpty(solicitud.Paciente.NumDocCedula) ? 
                solicitud.Paciente.NumDocPasaporte : solicitud.Paciente.NumDocCedula
        };
    } 
}