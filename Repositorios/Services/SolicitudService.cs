using DIGESA.Components.Pages.Public;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class SolicitudService : ISolicitudService
{
    private readonly DbContextDigesa _context;
    private readonly IFileService _fileService;
    private readonly ICommon _commonService;
    private readonly IEmailService _emailService;
    private readonly ILogger<SolicitudService> _logger;

    public SolicitudService(DbContextDigesa context, IFileService fileService,
        ICommon commonService, IEmailService emailService, ILogger<SolicitudService> logger)
    {
        _context = context;
        _fileService = fileService;
        _commonService = commonService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<int> CrearSolicitudCompletaAsync(RegistroCannabisUnionModel registro,
        Solicitud.DocumentosModel documentos)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Guardar paciente
            var pacienteId = await GuardarPacienteAsync(registro.Paciente);

            // 2. Guardar acompañante si aplica
            if (registro.Paciente.RequiereAcompanante == RequiereAcompanante.Si && registro.Acompanante != null)
                await GuardarAcompananteAsync(registro.Acompanante, pacienteId);

            // 3. Guardar médico
            var medicoId = await GuardarMedicoAsync(registro.Medico, pacienteId);

            // 4. Guardar diagnósticos (usando el nuevo DiagnosticoModel)
            await GuardarDiagnosticosAsync(registro.Diagnostico, pacienteId);

            // 5. Guardar producto del paciente (usando el nuevo ProductoModel)
            await GuardarProductoPacienteAsync(registro.Producto, pacienteId);

            // 6. Guardar comorbilidades si existen
            if (registro.Comorbilidad != null && registro.Comorbilidad.TieneComorbilidadEnum == TieneComorbilidad.Si)
                await GuardarComorbilidadesAsync(registro.Comorbilidad, pacienteId);

            // 7. Crear solicitud principal
            var solicitudId = await CrearSolicitudPrincipalAsync(pacienteId);

            // 8. Obtener tipos de documento
            var tipoDocumentoMap = await ObtenerTiposDocumentoAsync();

            // 9. Guardar archivos
            await _fileService.GuardarArchivosAdjuntosAsync(documentos, solicitudId, tipoDocumentoMap);

            // 10. Crear historial
            await CrearHistorialSolicitudAsync(solicitudId);

            await transaction.CommitAsync();
            return solicitudId;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ValidarSolicitudCompletaAsync(RegistroCannabisUnionModel registro)
    {
        // Validaciones básicas
        if (registro.Paciente == null)
            return false;

        if (string.IsNullOrWhiteSpace(registro.Paciente.PrimerNombre) ||
            string.IsNullOrWhiteSpace(registro.Paciente.PrimerApellido))
            return false;

        if (registro.Paciente.FechaNacimiento == null)
            return false;

        // Validar que tenga al menos un diagnóstico (usando el nuevo modelo)
        if (registro.Diagnostico?.SelectedDiagnosticosIds?.Any() != true &&
            string.IsNullOrWhiteSpace(registro.Diagnostico?.NombreOtroDiagnostico))
            return false;

        return await Task.FromResult(true);
    }

    public async Task<int?> CrearOGuardarInstalacionPersonalizadaAsync(string nombreInstalacion)
    {
        if (string.IsNullOrWhiteSpace(nombreInstalacion))
            return null;

        var instalacionExistente = await _context.TbInstalacionSalud
            .FirstOrDefaultAsync(i => i.Nombre.ToLower() == nombreInstalacion.Trim().ToLower());

        if (instalacionExistente != null)
            return instalacionExistente.Id;

        var nuevaInstalacion = new TbInstalacionSalud
        {
            Nombre = nombreInstalacion.Trim()
        };

        _context.TbInstalacionSalud.Add(nuevaInstalacion);
        await _context.SaveChangesAsync();

        return nuevaInstalacion.Id;
    }

    public async Task<Dictionary<string, int>> ObtenerTiposDocumentoAsync()
    {
        var tipos = await _context.TbTipoDocumentoAdjunto
            .Where(t => t.IsActivo == true)
            .ToListAsync();

        return tipos.ToDictionary(t => t.Nombre.Trim(), t => t.Id);
    }

    public async Task<List<SolicitudModel>> ObtenerSolicitudesAsync()
    {
        var solicitudes = await _context.TbSolRegCannabis
            .Include(s => s.Paciente)
            .Include(s => s.EstadoSolicitud)
            .OrderByDescending(s => s.FechaSolicitud)
            .Select(s => new SolicitudModel
            {
                Id = s.Id,
                NumeroSolicitud = s.NumSolCompleta ?? "N/A",
                FechaSolicitud = s.FechaSolicitud ?? DateTime.MinValue,
                Estado = s.EstadoSolicitud != null ? s.EstadoSolicitud.NombreEstado : "Desconocido",
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteDocumento = s.Paciente.DocumentoCedula ?? s.Paciente.DocumentoPasaporte ?? "N/A",
                PacienteCorreo = s.Paciente.CorreoElectronico
            })
            .ToListAsync();

        return solicitudes;
    }

    public async Task<bool> ActualizarEstadoSolicitudAsync(int solicitudId, string nuevoEstado, string usuarioRevisor,
        string comentario)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var solicitud = await _context.TbSolRegCannabis
                .Include(s => s.Paciente)
                .FirstOrDefaultAsync(s => s.Id == solicitudId);

            if (solicitud == null) return false;

            var estadoNuevo = await _context.TbEstadoSolicitud
                .FirstOrDefaultAsync(e => e.NombreEstado == nuevoEstado);

            if (estadoNuevo == null) return false;

            // Actualizar solicitud
            solicitud.EstadoSolicitudId = estadoNuevo.IdEstado;
            solicitud.FechaRevision = DateOnly.FromDateTime(DateTime.Now);
            solicitud.UsuarioRevisor = usuarioRevisor;
            solicitud.ComentarioRevision = comentario;

            if (nuevoEstado == "Aprobada")
                solicitud.FechaAprobacion = DateTime.Now;

            _context.TbSolRegCannabis.Update(solicitud);

            var historial = new TbSolRegCannabisHistorial
            {
                SolRegCannabisId = solicitudId,
                EstadoSolicitudIdHistorial = estadoNuevo.IdEstado,
                Comentario = comentario,
                UsuarioRevisor = usuarioRevisor,
                FechaCambio = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.TbSolRegCannabisHistorial.Add(historial);

            await _context.SaveChangesAsync();

            // ENVIAR CORREO CON MANEJO DE ERRORES
            if (!string.IsNullOrEmpty(solicitud.Paciente.CorreoElectronico))
            {
                try
                {
                    string asunto = $"Resultado de su solicitud de Cannabis Medicinal: {nuevoEstado}";
                    string cuerpo = $@"
            <h2>Estimado/a {solicitud.Paciente.PrimerNombre} {solicitud.Paciente.PrimerApellido}</h2>
            <p>Su solicitud <strong>{solicitud.NumSolCompleta}</strong> fue <strong>{nuevoEstado}</strong>.</p>
            <p><b>Motivo:</b> {comentario}</p>
            <br>
            <p>Atentamente,<br>DIGESA - Plataforma de Cannabis Medicinal</p>";

                    bool correoEnviado = await _emailService.EnviarCorreoAsync(
                        solicitud.Paciente.CorreoElectronico, asunto, cuerpo);

                    if (!correoEnviado)
                    {
                        _logger.LogWarning($"No se pudo enviar el correo a: {solicitud.Paciente.CorreoElectronico}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error enviando correo a: {solicitud.Paciente.CorreoElectronico}");
                }
            }

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<Dictionary<string, int>> ObtenerConteoPorEstadoAsync()
    {
        try
        {
            var conteos = await _context.TbSolRegCannabis
                .Include(s => s.EstadoSolicitud)
                .Where(s => s.EstadoSolicitud != null)
                .GroupBy(s => s.EstadoSolicitud.NombreEstado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Cantidad = g.Count()
                })
                .ToDictionaryAsync(x => x.Estado, x => x.Cantidad);

            // Asegurarse de que todos los estados tengan al menos 0
            var todosEstados = await _context.TbEstadoSolicitud
                .Select(e => e.NombreEstado)
                .ToListAsync();

            foreach (var estado in todosEstados)
            {
                if (!conteos.ContainsKey(estado))
                {
                    conteos[estado] = 0;
                }
            }

            return conteos;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener conteos por estado: {ex.Message}");
            return new Dictionary<string, int>();
        }
    }

    // MÉTODOS PRIVADOS AUXILIARES

    private async Task<int> GuardarPacienteAsync(PacienteModel pacienteModel)
    {
        var paciente = new TbPaciente
        {
            PrimerNombre = pacienteModel.PrimerNombre ?? "",
            SegundoNombre = pacienteModel.SegundoNombre,
            PrimerApellido = pacienteModel.PrimerApellido ?? "",
            SegundoApellido = pacienteModel.SegundoApellido,
            TipoDocumento = pacienteModel.TipoDocumento.ToString(),
            DocumentoCedula = pacienteModel.TipoDocumento == TipoDocumento.Cedula
                ? pacienteModel.NumeroDocumento
                : null,
            DocumentoPasaporte = pacienteModel.TipoDocumento == TipoDocumento.Pasaporte
                ? pacienteModel.NumeroDocumento
                : null,
            Nacionalidad = pacienteModel.Nacionalidad ?? "",
            FechaNacimiento = pacienteModel.FechaNacimiento.HasValue
                ? DateOnly.FromDateTime(pacienteModel.FechaNacimiento.Value)
                : null,
            Sexo = pacienteModel.Sexo.ToString(),
            TelefonoPersonal = pacienteModel.TelefonoPersonal,
            TelefonoLaboral = pacienteModel.TelefonoLaboral,
            CorreoElectronico = pacienteModel.CorreoElectronico ?? "",
            ProvinciaId = pacienteModel.ProvinciaId,
            DistritoId = pacienteModel.DistritoId,
            CorregimientoId = pacienteModel.CorregimientoId,
            RegionId = pacienteModel.RegionSaludId,
            InstalacionId = pacienteModel.InstalacionSaludId,
            // InstalacionPersonalizada = pacienteModel.InstalacionSaludPersonalizada, 
            DireccionExacta = pacienteModel.DireccionExacta ?? "",
            RequiereAcompanante = pacienteModel.RequiereAcompanante == RequiereAcompanante.Si,
            MotivoRequerimientoAcompanante = pacienteModel.MotivoRequerimientoAcompanante?.ToString(),
            TipoDiscapacidad = pacienteModel.TipoDiscapacidad
        };

        _context.TbPaciente.Add(paciente);
        await _context.SaveChangesAsync();
        return paciente.Id;
    }


    private async Task GuardarAcompananteAsync(AcompananteModel acompananteModel, int pacienteId)
    {
        var acompanante = new TbAcompanantePaciente
        {
            PacienteId = pacienteId,
            PrimerNombre = acompananteModel.PrimerNombre ?? "",
            SegundoNombre = acompananteModel.SegundoNombre,
            PrimerApellido = acompananteModel.PrimerApellido ?? "",
            SegundoApellido = acompananteModel.SegundoApellido,
            TipoDocumento = acompananteModel.TipoDocumento.ToString(),
            NumeroDocumento = acompananteModel.NumeroDocumento ?? "",
            Nacionalidad = acompananteModel.Nacionalidad ?? "",
            Parentesco = acompananteModel.Parentesco.ToString(),
            TelefonoMovil = acompananteModel.TelefonoMovil
        };

        _context.TbAcompanantePaciente.Add(acompanante);
        await _context.SaveChangesAsync();
    }

    private async Task<int> GuardarMedicoAsync(MedicoModel medicoModel, int pacienteId)
    {
        var medico = new TbMedicoPaciente
        {
            PrimerNombre = medicoModel.PrimerNombre ?? "",
            PrimerApellido = medicoModel.PrimerApellido ?? "",
            MedicoDisciplina = medicoModel.MedicoDisciplina.ToString(),
            MedicoIdoneidad = medicoModel.MedicoIdoneidad ?? "",
            MedicoTelefono = medicoModel.TelefonoMovil ?? "",
            InstalacionId = medicoModel.InstalacionSaludId,
            // InstalacionPersonalizada = medicoModel.InstalacionSaludPersonalizada, 
            RegionId = medicoModel.RegionSaludId,
            DetalleMedico = medicoModel.DetalleEspecialidad ?? "Sin detalle",
            PacienteId = pacienteId
        };

        _context.TbMedicoPaciente.Add(medico);
        await _context.SaveChangesAsync();
        return medico.Id;
    }
    private async Task GuardarDiagnosticosAsync(DiagnosticoModel diagnosticoModel, int pacienteId)
    {
        var diagnosticosList = await _commonService.GetAllDiagnosticsAsync();

        // Guardar diagnósticos seleccionados
        foreach (var diagnosticoId in diagnosticoModel.SelectedDiagnosticosIds)
        {
            var diagnosticoNombre = diagnosticosList.FirstOrDefault(d => d.Id == diagnosticoId)?.Nombre;
            if (!string.IsNullOrEmpty(diagnosticoNombre))
            {
                var diagnostico = new TbPacienteDiagnostico
                {
                    PacienteId = pacienteId,
                    NombreDiagnostico = diagnosticoNombre,
                    Id = diagnosticoId 
                };
                _context.TbPacienteDiagnostico.Add(diagnostico);
            }
        }

        // Guardar diagnóstico "Otro" si está seleccionado
        if (diagnosticoModel.IsOtroDiagSelected && !string.IsNullOrWhiteSpace(diagnosticoModel.NombreOtroDiagnostico))
        {
            var otroDiagnostico = new TbPacienteDiagnostico
            {
                PacienteId = pacienteId,
                NombreDiagnostico = diagnosticoModel.NombreOtroDiagnostico,
                // DiagnosticoId = null // Indicar que es personalizado
            };
            _context.TbPacienteDiagnostico.Add(otroDiagnostico);
        }

        await _context.SaveChangesAsync();
    }

    private async Task GuardarProductoPacienteAsync(ProductoModel productoModel, int pacienteId)
    {
        // Para las formas farmacéuticas múltiples
        var formasFarmaceuticas = new List<string>();
        var formasList = await _commonService.GetAllFormasFarmaceuticasAsync(); // Necesitarás crear este método

        foreach (var formaId in productoModel.SelectedFormaIds)
        {
            var formaNombre = formasList.FirstOrDefault(f => f.Id == formaId)?.Name;
            if (!string.IsNullOrEmpty(formaNombre))
                formasFarmaceuticas.Add(formaNombre);
        }

        // Agregar forma personalizada si existe
        if (productoModel.IsOtraFormaSelected && !string.IsNullOrWhiteSpace(productoModel.NombreOtraForma))
            formasFarmaceuticas.Add(productoModel.NombreOtraForma);

        // Para las vías de administración múltiples
        var viasAdministracion = new List<string>();
        var viasList = await _commonService.GetAllViasAdministracionAsync(); // Necesitarás crear este método

        foreach (var viaId in productoModel.SelectedViaAdmIds)
        {
            var viaNombre = viasList.FirstOrDefault(v => v.Id == viaId)?.Name;
            if (!string.IsNullOrEmpty(viaNombre))
                viasAdministracion.Add(viaNombre);
        }

        // Agregar vía personalizada si existe
        if (productoModel.IsOtraViaAdmSelected && !string.IsNullOrWhiteSpace(productoModel.NombreOtraViaAdm))
            viasAdministracion.Add(productoModel.NombreOtraViaAdm);

        var productoPaciente = new TbNombreProductoPaciente
        {
            PacienteId = pacienteId,
            NombreProducto = productoModel.NombreProducto ?? productoModel.NombreProductoEnum.ToString(),
            NombreComercialProd = productoModel.NombreComercialProd,
            FormaFarmaceutica = string.Join(", ", formasFarmaceuticas), // Concatenar todas las formas
            CantidadConcentracion = productoModel.CantidadConcentracion,
            NombreConcentracion = productoModel.ConcentracionEnum == ConcentracionE.OTRO
                ? productoModel.NombreConcentracion
                : productoModel.ConcentracionEnum.ToString(),
            ViaConsumoProducto = string.Join(", ", viasAdministracion), // Concatenar todas las vías
            ProductoUnidad = productoModel.ProductoUnidad,
            ProductoUnidadId = productoModel.ProductoUnidadId,
            DetDosisPaciente = productoModel.DetDosisPaciente,
            DosisFrecuencia = productoModel.DosisFrecuencia,
            DosisDuracion = productoModel.DosisDuracion,
            UsaDosisRescate = productoModel.UsaDosisRescateEnum == UsaDosisRescate.Si,
            DetDosisRescate = productoModel.UsaDosisRescateEnum == UsaDosisRescate.Si
                ? productoModel.DetDosisRescate
                : null
        };

        _context.TbNombreProductoPaciente.Add(productoPaciente);
        await _context.SaveChangesAsync();
    }

    private async Task GuardarComorbilidadesAsync(PacienteComorbilidadModel comorbilidadModel, int pacienteId)
    {
        if (!string.IsNullOrWhiteSpace(comorbilidadModel.NombreDiagnostico))
        {
            var comorbilidad = new TbPacienteComorbilidad
            {
                PacienteId = pacienteId,
                NombreDiagnostico = comorbilidadModel.NombreDiagnostico,
                DetalleTratamiento = comorbilidadModel.DetalleTratamiento,
                // FechaDiagnostico = DateOnly.FromDateTime(DateTime.Now), // Usar DateOnly
                // TieneComorbilidad = true
            };

            _context.TbPacienteComorbilidad.Add(comorbilidad);
            await _context.SaveChangesAsync();
        }
    }

    private async Task<int> CrearSolicitudPrincipalAsync(int pacienteId)
    {
        var secuencia = await _context.TbSolSecuencia
            .FirstOrDefaultAsync(s => s.Anio == DateTime.Now.Year && s.IsActivo == true);

        if (secuencia == null)
        {
            secuencia = new TbSolSecuencia
            {
                Anio = DateTime.Now.Year,
                Numeracion = 1,
                IsActivo = true
            };
            _context.TbSolSecuencia.Add(secuencia);
        }
        else
        {
            secuencia.Numeracion++;
            _context.TbSolSecuencia.Update(secuencia);
        }

        await _context.SaveChangesAsync();

        var estadoPendiente = await _context.TbEstadoSolicitud
            .FirstOrDefaultAsync(e => e.NombreEstado == "Pendiente");

        if (estadoPendiente == null)
        {
            throw new InvalidOperationException("No se encontró el estado 'Pendiente' en la base de datos");
        }

        var solicitud = new TbSolRegCannabis
        {
            PacienteId = pacienteId,
            FechaSolicitud = DateTime.Now,
            EstadoSolicitudId = estadoPendiente.IdEstado,
            CreadaPor = "Sistema",
            NumSolAnio = DateTime.Now.Year,
            NumSolMes = DateTime.Now.Month,
            NumSolSecuencia = secuencia.Numeracion,
            NumSolCompleta = $"{secuencia.Numeracion:0000}-{DateTime.Now.Year}"
        };

        _context.TbSolRegCannabis.Add(solicitud);
        await _context.SaveChangesAsync();

        return solicitud.Id;
    }

    private async Task CrearHistorialSolicitudAsync(int solicitudId)
    {
        var estadoPendiente = await _context.TbEstadoSolicitud
            .FirstOrDefaultAsync(e => e.NombreEstado == "Pendiente");

        if (estadoPendiente == null)
        {
            throw new InvalidOperationException("No se encontró el estado 'Pendiente' en la base de datos");
        }

        var historial = new TbSolRegCannabisHistorial
        {
            SolRegCannabisId = solicitudId,
            EstadoSolicitudIdHistorial = estadoPendiente.IdEstado,
            Comentario = "Solicitud creada",
            UsuarioRevisor = "Sistema",
            FechaCambio = DateOnly.FromDateTime(DateTime.Now)
        };

        _context.TbSolRegCannabisHistorial.Add(historial);
        await _context.SaveChangesAsync();
    }
}