
using System.ComponentModel.DataAnnotations;
using DIGESA.Models.Entities.DBDIGESA;
using Microsoft.EntityFrameworkCore;

public class PacienteService : IPacienteService
{
    private readonly DbContextDigesa _context;
    private readonly ILogger<PacienteService> _logger;

    public PacienteService(DbContextDigesa context, ILogger<PacienteService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> CreateAsync(PacienteRegistroDTO model)
    {
        try
        {
            var errores = model.Validate();
            if (errores.Count > 0)
                throw new ValidationException(string.Join(", ", errores));

            // Mapeo de Paciente
            var paciente = new Paciente
            {
                NombreCompleto = model.NombreCompleto,
                TipoDocumento = model.TipoDocumento,
                NumeroDocumento = model.NumeroDocumento,
                Nacionalidad = model.Nacionalidad,
                FechaNacimiento = model.FechaNacimiento.Value,
                Sexo = model.Sexo,
                DireccionResidencia = model.DireccionResidencia,
                TelefonoResidencial = model.TelefonoResidencial,
                TelefonoPersonal = model.TelefonoPersonal,
                TelefonoLaboral = model.TelefonoLaboral,
                CorreoElectronico = model.CorreoElectronico,
                InstalacionSalud = model.InstalacionSalud,
                RegionSalud = model.RegionSalud,
                RequiereAcompanante = model.RequiereAcompanante
            };

            await _context.Paciente.AddAsync(paciente);
            await _context.SaveChangesAsync();

            // Si requiere acompañante
            if (model.RequiereAcompanante && model.Acompanante != null)
            {
                var acompanante = new Acompanante
                {
                    PacienteId = paciente.Id,
                    NombreCompleto = model.Acompanante.NombreCompleto,
                    TipoDocumento = model.Acompanante.TipoDocumento,
                    NumeroDocumento = model.Acompanante.NumeroDocumento,
                    Nacionalidad = model.Acompanante.Nacionalidad,
                    Parentesco = model.Acompanante.Parentesco
                };
                await _context.Acompanante.AddAsync(acompanante);
            }

            // Médico
            var medico = new Medico
            {
                NombreCompleto = model.Medico.NombreCompleto,
                Especialidad = model.Medico.Especialidad,
                NumeroRegistroIdoneidad = model.Medico.RegistroIdoneidad,
                NumeroTelefono = model.Medico.NumeroTelefono,
                InstalacionSalud = model.Medico.InstalacionSalud
            };
            await _context.Medico.AddAsync(medico);
            await _context.SaveChangesAsync(); // Necesario para obtener Id del médico

            // Solicitud
            var solicitud = new Solicitud
            {
                PacienteId = paciente.Id,
                MedicoId = medico.Id,
                // EstadoSolicitud = "Pendiente",
                FechaSolicitud = DateTime.Now
            };
            await _context.Solicitud.AddAsync(solicitud);
            await _context.SaveChangesAsync();

            // Diagnósticos
            var diagnosticoIds = new List<int>();
            if (model.Diagnostico.Alzheimer) diagnosticoIds.Add(await GetOrCreateDiagnostico("Alzheimer"));
            if (model.Diagnostico.Epilepsia) diagnosticoIds.Add(await GetOrCreateDiagnostico("Epilepsia"));
            if (model.Diagnostico.SIDA) diagnosticoIds.Add(await GetOrCreateDiagnostico("VIH/SIDA"));
            if (model.Diagnostico.Anorexia) diagnosticoIds.Add(await GetOrCreateDiagnostico("Anorexia"));
            if (model.Diagnostico.Fibromialgia) diagnosticoIds.Add(await GetOrCreateDiagnostico("Fibromialgia"));
            if (model.Diagnostico.Artritis) diagnosticoIds.Add(await GetOrCreateDiagnostico("Artritis"));
            if (model.Diagnostico.Glaucoma) diagnosticoIds.Add(await GetOrCreateDiagnostico("Glaucoma"));
            if (model.Diagnostico.EstresPostraumatico) diagnosticoIds.Add(await GetOrCreateDiagnostico("Síndrome de Estrés Postraumático"));
            if (model.Diagnostico.Autismo) diagnosticoIds.Add(await GetOrCreateDiagnostico("Autismo"));
            if (model.Diagnostico.HepatitisC) diagnosticoIds.Add(await GetOrCreateDiagnostico("Hepatitis C"));

            foreach (var id in diagnosticoIds)
            {
                await _context.SolicitudDiagnostico.AddAsync(new SolicitudDiagnostico
                {
                    SolicitudId = solicitud.Id,
                    DiagnosticoId = id,
                    EsPrimario = diagnosticoIds.First() == id
                });
            }

            // Tratamiento
            var tratamiento = new Tratamiento
            {
                SolicitudId = solicitud.Id,
                ConcentracionCbd = model.Terapia.ConcentracionCBD,
                ConcentracionThc = model.Terapia.ConcentracionTHC,
                OtrosCannabinoides = model.Terapia.OtrosCannabinoides,
                Dosis = model.Terapia.Dosis,
                FrecuenciaAdministracion = model.Terapia.FrecuenciaAdministracion,
                DuracionTratamientoDias = model.Terapia.DuracionTratamientoDias,
                CantidadPrescrita = model.Terapia.CantidadPrescrita,
                InstruccionesAdicionales = model.Terapia.InstruccionesAdicionales
            };
            await _context.Tratamiento.AddAsync(tratamiento);

            await _context.SaveChangesAsync();

            return paciente.Id;
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validación fallida en registro de paciente");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al registrar paciente");
            throw;
        }
    }

    private async Task<int> GetOrCreateDiagnostico(string nombre)
    {
        var diag = await _context.Diagnostico.FirstOrDefaultAsync(d => d.Nombre == nombre);
        if (diag == null)
        {
            diag = new Diagnostico { Nombre = nombre, Descripcion = $"Diagnóstico creado automáticamente - {nombre}" };
            await _context.Diagnostico.AddAsync(diag);
            await _context.SaveChangesAsync();
        }
        return diag.Id;
    }
    public async Task<List<Paciente>> GetAllAsync()
    {
        return await _context.Paciente.ToListAsync();
    }
}