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
            NumeroDocumento = pacienteEntity.DocumentoCedula ?? pacienteEntity.DocumentoPasaporte ?? "",
            Nacionalidad = pacienteEntity.Nacionalidad,
            TelefonoPersonal = pacienteEntity.TelefonoPersonal,
            TelefonoLaboral = pacienteEntity.TelefonoLaboral,
            CorreoElectronico = pacienteEntity.CorreoElectronico,
            DireccionExacta = pacienteEntity.DireccionExacta,
            FechaNacimiento = pacienteEntity.FechaNacimiento?.ToDateTime(TimeOnly.MinValue),
            TipoDiscapacidad = pacienteEntity.TipoDiscapacidad,
            ProvinciaId = pacienteEntity.ProvinciaId,
            DistritoId = pacienteEntity.DistritoId,
            CorregimientoId = pacienteEntity.CorregimientoId,
            RegionSaludId = pacienteEntity.RegionId,
            InstalacionSaludId = pacienteEntity.InstalacionId,
            
            // Mapeo de enums desde strings
            TipoDocumento = string.IsNullOrEmpty(pacienteEntity.DocumentoCedula) ? 
                TipoDocumento.Pasaporte : TipoDocumento.Cedula,
                
            Sexo = pacienteEntity.Sexo?.ToLower() switch
            {
                "masculino" => Sexo.Masculino,
                "femenino" => Sexo.Femenino,
                _ => Sexo.Masculino // Valor por defecto
            },
            
            RequiereAcompanante = pacienteEntity.RequiereAcompanante.HasValue ? 
                (pacienteEntity.RequiereAcompanante.Value ? RequiereAcompanante.Si : RequiereAcompanante.No) 
                : RequiereAcompanante.No,
                
            MotivoRequerimientoAcompanante = pacienteEntity.MotivoRequerimientoAcompanante?.ToLower() switch
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
            .Include(s => s.EstadoSolicitud)
            .Where(s => (s.Paciente.DocumentoCedula == documentoNormalizado || 
                        s.Paciente.DocumentoPasaporte == documentoNormalizado) &&
                        s.EstadoSolicitud != null &&
                        s.EstadoSolicitud.NombreEstado == "Aprobada")
            .OrderByDescending(s => s.FechaAprobacion)
            .FirstOrDefaultAsync();

        if (solicitud == null || solicitud.Paciente == null)
        {
            return new PacienteEstadoModel
            {
                Documento = documentoNormalizado,
                Nombre = "",
                Apellido = "",
                FechaVencimiento = null,
                Activo = false,
                EstadoSolicitud = "No encontrada"
            };
        }

        // Calcular fecha de vencimiento (2 años después de la aprobación)
        var fechaVencimiento = solicitud.FechaAprobacion?.AddYears(2);
        var estaActivo = fechaVencimiento >= DateTime.Now;

        return new PacienteEstadoModel
        {
            Documento = string.IsNullOrEmpty(solicitud.Paciente.DocumentoCedula) ? 
                solicitud.Paciente.DocumentoPasaporte ?? "" : solicitud.Paciente.DocumentoCedula,
            Nombre = solicitud.Paciente.PrimerNombre ?? "",
            Apellido = solicitud.Paciente.PrimerApellido ?? "",
            FechaVencimiento = fechaVencimiento,
            Activo = estaActivo,
            EstadoSolicitud = "Aprobada",
            FechaAprobacion = solicitud.FechaAprobacion
        };
    }
}