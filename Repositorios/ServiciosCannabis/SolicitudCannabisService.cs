using DIGESA.Helpers;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Formularios;
using DIGESA.Models.CannabisModels.Listados;
using DIGESA.Models.CannabisModels.Validadores;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.ServiciosCannabis;

public class SolicitudCannabisService : ISolicitudCannabisService
{
    private readonly DbContextDigesa _context;

    public SolicitudCannabisService(DbContextDigesa context)
    {
        _context = context;
    }
    
    public async Task<int> CrearSolicitudAsync(SolicitudCannabisFormViewModel model)
    {
        var validator = new SolicitudCannabisDomainValidator();
        var errores = validator.Validate(model);

        if (errores.Any())
            throw new InvalidOperationException(string.Join(" | ", errores));
        var paciente = new TbPaciente
        {
            PrimerNombre = model.Paciente.PrimerNombre,
            SegundoNombre = model.Paciente.SegundoNombre,
            PrimerApellido = model.Paciente.PrimerApellido,
            SegundoApellido = model.Paciente.SegundoApellido,

            TipoDocumento = model.Paciente.TipoDocumento.ToString(),
            DocumentoCedula = model.Paciente.NumeroDocumento,
            Nacionalidad = model.Paciente.Nacionalidad,

            FechaNacimiento = model.Paciente.FechaNacimiento.HasValue
                ? DateOnly.FromDateTime(model.Paciente.FechaNacimiento.Value)
                : null,

            ProvinciaId = model.Paciente.ProvinciaId,
            DistritoId = model.Paciente.DistritoId,
            CorregimientoId = model.Paciente.CorregimientoId,
            DireccionExacta = model.Paciente.DireccionExacta,

            RequiereAcompanante =
                model.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si,

            MotivoRequerimientoAcompanante =
                model.Paciente.MotivoRequerimientoAcompanante?.ToString()
        };


        _context.TbPaciente.Add(paciente);
        await _context.SaveChangesAsync();

        var solicitud = new TbSolRegCannabis
        {
            PacienteId = paciente.Id,
            FechaSolicitud = DateTime.Now,
            EsRenovacion = model.EsRenovacion,
            EstadoSolicitudId = 1, // Pendiente
            CreadaPor = "SYSTEM"
        };

        _context.TbSolRegCannabis.Add(solicitud);
        await _context.SaveChangesAsync();

        // Declaración jurada
        _context.TbDeclaracionJurada.Add(new TbDeclaracionJurada
        {
            SolRegCannabisId = solicitud.Id,
            NombreDeclarante = model.Declaracion.NombreDeclarante,
            Detalle = model.Declaracion.Detalle
        });

        await _context.SaveChangesAsync();

        return solicitud.Id;
    }

    public async Task<SolicitudCannabisFormViewModel?> ObtenerPorIdAsync(int id)
    {
        var solicitud = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.TbDeclaracionJurada)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (solicitud == null) return null;

        return new SolicitudCannabisFormViewModel
        {
            Paciente = new DatosPacienteVM
            {
                PrimerNombre = solicitud.Paciente.PrimerNombre,
                PrimerApellido = solicitud.Paciente.PrimerApellido,
                Nacionalidad = solicitud.Paciente.Nacionalidad,
                FechaNacimiento = solicitud.Paciente.FechaNacimiento.HasValue
                    ? solicitud.Paciente.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue)
                    : null
            }
        };
    }

    public async Task<List<PacienteListadoViewModel>> ObtenerSolicitudesAsync()
    {
        var data = await _context.TbSolRegCannabis
            .AsNoTracking()
            .Include(s => s.Paciente)
            .ThenInclude(p => p.Provincia)
            .Include(s => s.EstadoSolicitud)
            .Select(s => new
            {
                s.Id,
                PrimerNombre = s.Paciente.PrimerNombre,
                PrimerApellido = s.Paciente.PrimerApellido,
                s.Paciente.DocumentoCedula,
                Provincia = s.Paciente.Provincia != null
                    ? s.Paciente.Provincia.NombreProvincia
                    : null,
                Edad = s.Paciente.FechaNacimiento.HasValue
                    ? FechasCarnetHelper.CalcularEdad(
                        s.Paciente.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue))
                    : 0,
                FechaNacimiento = s.Paciente.FechaNacimiento,
                s.FechaSolicitud,
                s.FechaVencimientoCarnet,
                EstadoSolicitud = s.EstadoSolicitud.NombreEstado,
                s.NumeroCarnet,
                CarnetActivo = s.CarnetActivo ?? false
            })
            .ToListAsync();
        
        return data.Select(s => new PacienteListadoViewModel
        {
            Id = s.Id,
            NombreCompleto =
                (s.PrimerNombre ?? "") + " " +
                (s.PrimerApellido ?? ""),
            Documento = s.DocumentoCedula,
            Provincia = s.Provincia ?? "—",
            Edad = s.FechaNacimiento.HasValue
                ? FechasCarnetHelper.CalcularEdad(
                    s.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue))
                : 0,
            FechaNacimiento = s.FechaNacimiento.HasValue
                ? s.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue)
                : DateTime.MinValue,
            FechaSolicitud = s.FechaSolicitud,
            FechaVencimiento = s.FechaVencimientoCarnet,
            EstadoSolicitud = NormalizarEstado(s.EstadoSolicitud),
            NumeroCarnet = s.NumeroCarnet,
            CarnetActivo = s.CarnetActivo,
        }).ToList();
    }


    public async Task<bool> ActualizarSolicitudAsync(
        int solicitudId,
        SolicitudCannabisFormViewModel model,
        string usuario)
    {
        var solicitud = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .FirstOrDefaultAsync(s => s.Id == solicitudId);

        if (solicitud == null)
            return false;

        solicitud.Paciente.PrimerNombre = model.Paciente.PrimerNombre;
        solicitud.Paciente.PrimerApellido = model.Paciente.PrimerApellido;
        solicitud.ModificadaPor = usuario;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RechazarSolicitudAsync(int id, string comentario, string usuario)
    {
        var solicitud = await _context.TbSolRegCannabis.FindAsync(id);
        if (solicitud == null) return false;

        solicitud.EstadoSolicitudId = 3; // Rechazado
        solicitud.ComentarioRevision = comentario;
        solicitud.UsuarioRevisor = usuario;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> InactivarCarnetAsync(int id, string razon, string usuario)
    {
        var solicitud = await _context.TbSolRegCannabis.FindAsync(id);
        if (solicitud == null) return false;

        solicitud.CarnetActivo = false;
        solicitud.RazonInactivacion = razon;
        solicitud.UsuarioInactivador = usuario;
        solicitud.FechaInactivacion = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync()
    {
        return await _context.TbSolRegCannabis
            .AsNoTracking()
            .Include(s => s.EstadoSolicitud)
            .GroupBy(s => s.EstadoSolicitud.NombreEstado)
            .Select(g => new
            {
                Estado = g.Key,
                Cantidad = g.Count()
            })
            .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);
    }
    
    //APROBAR SOLICITUD CON COMENTARIO OPCIONAL
    public async Task<bool> AprobarSolicitudAsync(int id, string usuario, string? comentario = null)
    {
        var solicitud = await _context.TbSolRegCannabis.FindAsync(id);
        if (solicitud == null) return false;

        var ahora = DateTime.Now;
        var hoy = DateTime.Today;

        solicitud.EstadoSolicitudId = 2; 
        solicitud.FechaAprobacion = ahora;
        solicitud.FechaEmisionCarnet = DateTime.Now;
        solicitud.FechaVencimientoCarnet = DateTime.Now.AddYears(2);
        solicitud.CarnetActivo = true;
        solicitud.NumeroCarnet ??=
            $"CAN-{DateTime.Now:yyyy}-{solicitud.Id:D6}";

        solicitud.VersionCarnet++;
        solicitud.UsuarioRevisor = usuario;
        solicitud.ComentarioRevision = comentario;

        await _context.SaveChangesAsync();
        return true;
    }

    
    // obtener listado detalle
    public async Task<PacienteListadoViewModel?> ObtenerDetalleListadoAsync(int id)
    {
        return await _context.TbSolRegCannabis
            .AsNoTracking()
            .Include(s => s.Paciente)
            .ThenInclude(p => p.Provincia)
            .Include(s => s.EstadoSolicitud)
            .Where(s => s.Id == id)
            .Select(s => new PacienteListadoViewModel
            {
                Id = s.Id,
                NombreCompleto = s.Paciente.PrimerNombre + " " + s.Paciente.PrimerApellido,
                Documento = s.Paciente.DocumentoCedula,
                FechaNacimiento = s.Paciente.FechaNacimiento.HasValue
                    ? s.Paciente.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue)
                    : DateTime.MinValue,

                Edad = s.Paciente.FechaNacimiento.HasValue
                    ? FechasCarnetHelper.CalcularEdad(
                        s.Paciente.FechaNacimiento.Value.ToDateTime(TimeOnly.MinValue))
                    : 0,
                Provincia = s.Paciente.Provincia != null
                    ? s.Paciente.Provincia.NombreProvincia
                    : "—",
                Celular = s.Paciente.TelefonoPersonal ?? "No registrado",
                Telefono = s.Paciente.TelefonoLaboral ?? "No registrado",
                FechaSolicitud = s.FechaSolicitud,
                FechaVencimiento = s.FechaVencimientoCarnet,
                EstadoSolicitud = NormalizarEstado(s.EstadoSolicitud.NombreEstado),
                NumeroCarnet = s.NumeroCarnet,
                CarnetActivo = s.CarnetActivo ?? false
            })
            .FirstOrDefaultAsync();
    }

    private static string NormalizarEstado(string estado)
    {
        return estado switch
        {
            "pendiente" => "Pendiente",
            "aprobada" => "Aprobada",
            "rechazada" => "Rechazada",
            _ => estado
        };
    }
}
