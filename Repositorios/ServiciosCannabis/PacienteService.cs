using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Formularios;
using DIGESA.Models.CannabisModels.Renovaciones;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.ServiciosCannabis;

public class PacienteService : IPaciente
{
    private readonly DbContextDigesa _context;

    public PacienteService(DbContextDigesa context)
    {
        _context = context;
    }

    // ============================================
    // 🔎 ESTADO DEL PACIENTE
    // ============================================
    public async Task<EstadoSolicitudViewModel?> GetEstadoPacienteAsync(string documento)
    {
        var solicitud = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .Where(s =>
                s.Paciente != null &&
                (s.Paciente.DocumentoCedula == documento ||
                 s.Paciente.DocumentoPasaporte == documento))
            .OrderByDescending(s => s.FechaSolicitud)
            .FirstOrDefaultAsync();

        if (solicitud?.Paciente == null)
            return null;

        return new EstadoSolicitudViewModel
        {
            IdEstado = solicitud.EstadoSolicitudId ?? 0,

            Activo = solicitud.CarnetActivo ?? false,

            Nombre = solicitud.Paciente.PrimerNombre,
            Apellido = solicitud.Paciente.PrimerApellido,

            Documento = solicitud.Paciente.DocumentoCedula
                      ?? solicitud.Paciente.DocumentoPasaporte
                      ?? string.Empty,

            FechaVencimiento = solicitud.FechaVencimientoCarnet
        };
    }

    // ============================================
    // 🔎 DETALLE DEL PACIENTE
    // ============================================
    public async Task<PacienteViewModel?> BuscarPorDocumentoAsync(string documento)
    {
        var paciente = await _context.TbPaciente
            .FirstOrDefaultAsync(p =>
                p.DocumentoCedula == documento ||
                p.DocumentoPasaporte == documento);

        if (paciente == null)
            return null;

        return new PacienteViewModel
        {
            Id = paciente.Id,

            PrimerNombre = paciente.PrimerNombre,
            SegundoNombre = paciente.SegundoNombre,
            PrimerApellido = paciente.PrimerApellido,
            SegundoApellido = paciente.SegundoApellido,

            TipoDocumento = paciente.TipoDocumento?.ToUpper() switch
            {
                "CEDULA" => EnumViewModel.TipoDocumento.Cedula,
                "PASAPORTE" => EnumViewModel.TipoDocumento.Pasaporte,
                _ => EnumViewModel.TipoDocumento.Cedula
            },

            Documento = paciente.DocumentoCedula
                      ?? paciente.DocumentoPasaporte
                      ?? string.Empty,

            Nacionalidad = paciente.Nacionalidad,

            // ✅ DateOnly? → DateTime?
            FechaNacimiento = paciente.FechaNacimiento.HasValue
                ? paciente.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue)
                : null,

            Sexo = paciente.Sexo?.ToUpper() switch
            {
                "M" or "MASCULINO" => EnumViewModel.Sexo.Masculino,
                "F" or "FEMENINO" => EnumViewModel.Sexo.Femenino,
                _ => EnumViewModel.Sexo.Masculino
            },

            TelefonoPersonal = paciente.TelefonoPersonal,
            TelefonoLaboral = paciente.TelefonoLaboral,
            CorreoElectronico = paciente.CorreoElectronico,
            DireccionExacta = paciente.DireccionExacta,

            RequiereAcompanante = string.IsNullOrWhiteSpace(paciente.MotivoRequerimientoAcompanante)
                ? EnumViewModel.RequiereAcompanante.No
                : EnumViewModel.RequiereAcompanante.Si,

            MotivoRequerimientoAcompanante = paciente.MotivoRequerimientoAcompanante?.ToUpper() switch
            {
                "MENOR_EDAD" => EnumViewModel.MotivoRequerimientoAcompanante.PacienteMenorEdad,
                "DISCAPACIDAD" => EnumViewModel.MotivoRequerimientoAcompanante.PacienteDiscapacidad,
                _ => null
            },

            TipoDiscapacidad = paciente.TipoDiscapacidad
        };
    }
    public async Task<DatosAcompananteVM?> ObtenerAcompananteAsync(int pacienteId)
    {
        var acompanante = await _context.TbAcompanantePaciente
            .AsNoTracking()
            .Where(a => a.PacienteId == pacienteId)
            .Select(a => new
            {
                a.PrimerNombre,
                a.SegundoNombre,
                a.PrimerApellido,
                a.SegundoApellido,
                a.TipoDocumento,
                a.NumeroDocumento,
                a.Nacionalidad,
                a.Parentesco,
                a.TelefonoMovil
            })
            .FirstOrDefaultAsync();
        if (acompanante == null)
            return null;

        return new DatosAcompananteVM
        {
            PrimerNombre = acompanante.PrimerNombre,
            SegundoNombre = acompanante.SegundoNombre,
            PrimerApellido = acompanante.PrimerApellido,
            SegundoApellido = acompanante.SegundoApellido,

            TipoDocumento = acompanante.TipoDocumento?.ToUpper() switch
            {
                "CEDULA" => EnumViewModel.TipoDocumento.Cedula,
                "PASAPORTE" => EnumViewModel.TipoDocumento.Pasaporte,
                _ => EnumViewModel.TipoDocumento.Cedula
            },

            NumeroDocumento = acompanante.NumeroDocumento,
            Nacionalidad = acompanante.Nacionalidad,
            Parentesco = acompanante.Parentesco,
            TelefonoMovil = acompanante.TelefonoMovil
        };
    }

}
