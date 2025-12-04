using DIGESA.Data;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class PacienteService : IPaciente
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<PacienteService> _logger;

    public PacienteService(DbContextDigesa context, ILogger<PacienteService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PacienteModel?> BuscarPorDocumentoAsync(string documento)
    {
        var documentoNormalizado = documento.Trim();

        var pacienteEntity = await _context.TbPaciente
            .Include(p => p.Provincia)
            .Include(p => p.Distrito)
            .Include(p => p.Corregimiento)
            .Include(p => p.Region)
            .Include(p => p.Instalacion)
            .FirstOrDefaultAsync(p => p.DocumentoCedula == documentoNormalizado || 
                                     p.DocumentoPasaporte == documentoNormalizado);

        if (pacienteEntity == null)
            return null;

        return MapearPacienteEntityAModel(pacienteEntity);
    }

    public async Task<PacienteEstadoModel> GetEstadoPacienteAsync(string documento)
    {
        var documentoNormalizado = documento.Trim();

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

        var fechaVencimiento = solicitud.FechaAprobacion?.AddYears(2);
        var estaActivo = fechaVencimiento >= DateTime.Now;

        return new PacienteEstadoModel
        {
            Documento = !string.IsNullOrEmpty(solicitud.Paciente.DocumentoCedula) 
                ? solicitud.Paciente.DocumentoCedula 
                : solicitud.Paciente.DocumentoPasaporte ?? "",
            Nombre = solicitud.Paciente.PrimerNombre ?? "",
            Apellido = solicitud.Paciente.PrimerApellido ?? "",
            FechaVencimiento = fechaVencimiento,
            Activo = estaActivo,
            EstadoSolicitud = "Aprobada",
            FechaAprobacion = solicitud.FechaAprobacion
        };
    }

    public async Task<ResultModel<PacienteModel>> CrearPacienteAsync(PacienteModel paciente)
    {
        try
        {
            var existe = await _context.TbPaciente
                .AnyAsync(p => (p.DocumentoCedula == paciente.NumeroDocumento && 
                               paciente.TipoDocumento == TipoDocumento.Cedula) ||
                              (p.DocumentoPasaporte == paciente.NumeroDocumento && 
                               paciente.TipoDocumento == TipoDocumento.Pasaporte));

            if (existe)
            {
                return ResultModel<PacienteModel>.ErrorResult(
                    "Ya existe un paciente con este número de documento", 
                    new List<string> { "Documento duplicado" });
            }

            var pacienteEntity = new TbPaciente
            {
                PrimerNombre = paciente.PrimerNombre ?? string.Empty,
                SegundoNombre = paciente.SegundoNombre,
                PrimerApellido = paciente.PrimerApellido ?? string.Empty,
                SegundoApellido = paciente.SegundoApellido,
                TipoDocumento = paciente.TipoDocumento.ToString(),
                DocumentoCedula = paciente.TipoDocumento == TipoDocumento.Cedula 
                    ? paciente.NumeroDocumento 
                    : null,
                DocumentoPasaporte = paciente.TipoDocumento == TipoDocumento.Pasaporte 
                    ? paciente.NumeroDocumento 
                    : null,
                Nacionalidad = paciente.Nacionalidad ?? string.Empty,
                FechaNacimiento = paciente.FechaNacimiento.HasValue 
                    ? DateOnly.FromDateTime(paciente.FechaNacimiento.Value) 
                    : null,
                Sexo = paciente.Sexo.ToString(),
                RequiereAcompanante = paciente.RequiereAcompanante == RequiereAcompanante.Si,
                MotivoRequerimientoAcompanante = paciente.MotivoRequerimientoAcompanante?.ToString(),
                TipoDiscapacidad = paciente.TipoDiscapacidad,
                TelefonoPersonal = paciente.TelefonoPersonal,
                TelefonoLaboral = paciente.TelefonoLaboral,
                CorreoElectronico = paciente.CorreoElectronico ?? string.Empty,
                ProvinciaId = paciente.ProvinciaId,
                DistritoId = paciente.DistritoId,
                CorregimientoId = paciente.CorregimientoId,
                DireccionExacta = paciente.DireccionExacta ?? string.Empty,
                RegionId = paciente.RegionSaludId,
                InstalacionId = paciente.InstalacionSaludId,
                InstalacionPersonalizada = paciente.InstalacionSaludPersonalizada
            };

            _context.TbPaciente.Add(pacienteEntity);
            await _context.SaveChangesAsync();

            paciente.Id = pacienteEntity.Id;
            return ResultModel<PacienteModel>.SuccessResult(paciente, "Paciente creado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando paciente");
            return ResultModel<PacienteModel>.ErrorResult(
                "Error al crear el paciente", 
                new List<string> { ex.Message });
        }
    }

    public async Task<ResultModel<PacienteModel>> ActualizarPacienteAsync(PacienteModel paciente)
    {
        try
        {
            var pacienteEntity = await _context.TbPaciente
                .FirstOrDefaultAsync(p => p.Id == paciente.Id);

            if (pacienteEntity == null)
            {
                return ResultModel<PacienteModel>.ErrorResult("Paciente no encontrado");
            }

            // Actualizar propiedades
            pacienteEntity.PrimerNombre = paciente.PrimerNombre ?? pacienteEntity.PrimerNombre;
            pacienteEntity.SegundoNombre = paciente.SegundoNombre ?? pacienteEntity.SegundoNombre;
            pacienteEntity.PrimerApellido = paciente.PrimerApellido ?? pacienteEntity.PrimerApellido;
            pacienteEntity.SegundoApellido = paciente.SegundoApellido ?? pacienteEntity.SegundoApellido;
            pacienteEntity.Nacionalidad = paciente.Nacionalidad ?? pacienteEntity.Nacionalidad;
            pacienteEntity.FechaNacimiento = paciente.FechaNacimiento.HasValue 
                ? DateOnly.FromDateTime(paciente.FechaNacimiento.Value) 
                : pacienteEntity.FechaNacimiento;
            pacienteEntity.Sexo = paciente.Sexo.ToString();
            pacienteEntity.TelefonoPersonal = paciente.TelefonoPersonal ?? pacienteEntity.TelefonoPersonal;
            pacienteEntity.TelefonoLaboral = paciente.TelefonoLaboral ?? pacienteEntity.TelefonoLaboral;
            pacienteEntity.CorreoElectronico = paciente.CorreoElectronico ?? pacienteEntity.CorreoElectronico;
            pacienteEntity.ProvinciaId = paciente.ProvinciaId ?? pacienteEntity.ProvinciaId;
            pacienteEntity.DistritoId = paciente.DistritoId ?? pacienteEntity.DistritoId;
            pacienteEntity.CorregimientoId = paciente.CorregimientoId ?? pacienteEntity.CorregimientoId;
            pacienteEntity.DireccionExacta = paciente.DireccionExacta ?? pacienteEntity.DireccionExacta;
            pacienteEntity.RegionId = paciente.RegionSaludId ?? pacienteEntity.RegionId;
            pacienteEntity.InstalacionId = paciente.InstalacionSaludId ?? pacienteEntity.InstalacionId;
            pacienteEntity.InstalacionPersonalizada = paciente.InstalacionSaludPersonalizada ?? pacienteEntity.InstalacionPersonalizada;
            pacienteEntity.RequiereAcompanante = paciente.RequiereAcompanante == RequiereAcompanante.Si;
            pacienteEntity.MotivoRequerimientoAcompanante = paciente.MotivoRequerimientoAcompanante?.ToString();
            pacienteEntity.TipoDiscapacidad = paciente.TipoDiscapacidad;

            _context.TbPaciente.Update(pacienteEntity);
            await _context.SaveChangesAsync();

            return ResultModel<PacienteModel>.SuccessResult(paciente, "Paciente actualizado exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error actualizando paciente ID: {paciente.Id}");
            return ResultModel<PacienteModel>.ErrorResult(
                "Error al actualizar el paciente", 
                new List<string> { ex.Message });
        }
    }

    private PacienteModel MapearPacienteEntityAModel(TbPaciente entity)
    {
        var model = new PacienteModel
        {
            Id = entity.Id,
            PrimerNombre = entity.PrimerNombre,
            SegundoNombre = entity.SegundoNombre,
            PrimerApellido = entity.PrimerApellido,
            SegundoApellido = entity.SegundoApellido,
            NumeroDocumento = !string.IsNullOrEmpty(entity.DocumentoCedula) 
                ? entity.DocumentoCedula 
                : entity.DocumentoPasaporte,
            Nacionalidad = entity.Nacionalidad,
            TelefonoPersonal = entity.TelefonoPersonal,
            TelefonoLaboral = entity.TelefonoLaboral,
            CorreoElectronico = entity.CorreoElectronico,
            DireccionExacta = entity.DireccionExacta,
            FechaNacimiento = entity.FechaNacimiento?.ToDateTime(TimeOnly.MinValue),
            TipoDiscapacidad = entity.TipoDiscapacidad,
            ProvinciaId = entity.ProvinciaId,
            DistritoId = entity.DistritoId,
            CorregimientoId = entity.CorregimientoId,
            RegionSaludId = entity.RegionId,
            InstalacionSaludId = entity.InstalacionId,
            InstalacionSaludPersonalizada = entity.InstalacionPersonalizada
        };

        model.TipoDocumento = string.IsNullOrEmpty(entity.DocumentoCedula) 
            ? TipoDocumento.Pasaporte 
            : TipoDocumento.Cedula;
        
        model.Sexo = entity.Sexo?.ToLower() switch
        {
            "masculino" => Sexo.Masculino,
            "femenino" => Sexo.Femenino,
            _ => Sexo.Masculino
        };
        
        model.RequiereAcompanante = entity.RequiereAcompanante.HasValue 
            ? (entity.RequiereAcompanante.Value ? RequiereAcompanante.Si : RequiereAcompanante.No) 
            : RequiereAcompanante.No;
        
        if (entity.MotivoRequerimientoAcompanante != null)
        {
            model.MotivoRequerimientoAcompanante = entity.MotivoRequerimientoAcompanante.ToLower() switch
            {
                "pacientemenoredad" => MotivoRequerimientoAcompananteE.PacienteMenorEdad,
                "pacientediscapacidad" => MotivoRequerimientoAcompananteE.PacienteDiscapacidad,
                _ => null
            };
        }

        return model;
    }

    // Implementa los demás métodos según tu interfaz IPacienteService
    public async Task<List<InscripcionPacienteModel>> ObtenerInscripcionesAsync(FiltroInscripcionesModel? filtros = null)
    {
        // Implementación aquí
        return new List<InscripcionPacienteModel>();
    }

    public async Task<EstadisticasInscripcionesModel> ObtenerEstadisticasInscripcionesAsync()
    {
        // Implementación aquí
        return new EstadisticasInscripcionesModel();
    }
}