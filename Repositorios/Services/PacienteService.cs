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
            .FirstOrDefaultAsync(p => p.DocumentoCedula == documentoNormalizado || 
                                     p.DocumentoPasaporte == documentoNormalizado);

        if (pacienteEntity == null)
            return null;

        return new PacienteModel
        {
            Id = pacienteEntity.Id,
            PrimerNombre = pacienteEntity.PrimerNombre,
            SegundoNombre = pacienteEntity.SegundoNombre,
            PrimerApellido = pacienteEntity.PrimerApellido,
            SegundoApellido = pacienteEntity.SegundoApellido,
            NumDocCedula = pacienteEntity.DocumentoCedula,
            NumDocPasaporte = pacienteEntity.DocumentoPasaporte,
            Nacionalidad = pacienteEntity.Nacionalidad,
            TelefonoResidencialPersonal = pacienteEntity.TelefonoPersonal,
            TelefonoLaboral = pacienteEntity.TelefonoLaboral,
            CorreoElectronico = pacienteEntity.CorreoElectronico,
            DireccionExacta = pacienteEntity.DireccionExacta,
            FechaNacimiento = pacienteEntity.FechaNacimiento?.ToDateTime(TimeOnly.MinValue),
            TipoDiscapacidad = pacienteEntity.TipoDiscapacidad,
            pacienteInstalacionId = pacienteEntity.InstalacionId,
            pacienteRegionId = pacienteEntity.RegionId,
            pacienteProvinciaId = pacienteEntity.ProvinciaId,
            pacienteDistritoId = pacienteEntity.DistritoId,
            pacienteCorregimientoId = pacienteEntity.CorregimientoId,
            pacienteInstalacion = pacienteEntity.NombreInstalacion,
            
            // Mapeo de enums desde strings
            TipoDocumentoPacienteEnum = string.IsNullOrEmpty(pacienteEntity.DocumentoCedula) ? 
                TipoDocumentoPaciente.Pasaporte : TipoDocumentoPaciente.Cedula,
                
            SexoEnum = pacienteEntity.Sexo?.ToLower() switch
            {
                "masculino" => Sexo.Masculino,
                "femenino" => Sexo.Femenino,
                _ => null
            },
            
            RequiereAcompananteEnum = pacienteEntity.RequiereAcompanante.HasValue ? 
                (pacienteEntity.RequiereAcompanante.Value ? RequiereAcompanante.Si : RequiereAcompanante.No) 
                : null,
                
            MotivoRequerimientoAcompananteEnum = pacienteEntity.MotivoRequerimientoAcompanante?.ToLower() switch
            {
                "pacientemenoredad" => MotivoRequerimientoAcompanante.PacienteMenorEdad,
                "pacientediscapacidad" => MotivoRequerimientoAcompanante.PacienteDiscapacidad,
                _ => null
            }
        };
    }

    public async Task<PacienteEstadoModel> GetEstadoPacienteAsync(string documento)
    {
        var documentoNormalizado = documento.Trim();

        // Buscar la última solicitud aprobada para este documento
        var solicitud = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud) // Incluir la navegación al estado
            .Where(s => (s.Paciente.DocumentoCedula == documentoNormalizado || 
                        s.Paciente.DocumentoPasaporte == documentoNormalizado) &&
                        s.EstadoSolicitud.NombreEstado == "Aprobada")
            .OrderByDescending(s => s.FechaAprobacion)
            .FirstOrDefaultAsync();

        if (solicitud == null || solicitud.Paciente == null)
        {
            return new PacienteEstadoModel
            {
                Documento = documentoNormalizado,
                Nombre = null,
                Apellido = null,
                FechaVencimiento = null,
                Activo = false
            };
        }

        // Calcular fecha de vencimiento (2 años después de la aprobación)
        var fechaVencimiento = solicitud.FechaAprobacion?.AddYears(2);
        var estaActivo = fechaVencimiento >= DateTime.Now;

        return new PacienteEstadoModel
        {
            Documento = string.IsNullOrEmpty(solicitud.Paciente.DocumentoCedula) ? 
                solicitud.Paciente.DocumentoPasaporte : solicitud.Paciente.DocumentoCedula,
            Nombre = solicitud.Paciente.PrimerNombre,
            Apellido = solicitud.Paciente.PrimerApellido,
            FechaVencimiento = fechaVencimiento,
            Activo = estaActivo
        };
    }
}