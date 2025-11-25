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

    // IMPLEMENTACIÓN DE TODOS LOS MÉTODOS DE LA INTERFACE

    public async Task<int> CrearSolicitudCompletaAsync(RegistroCanabisUnionModel registro,
        Solicitud.DocumentosModel documentos)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Procesar instalaciones personalizadas
            await ProcesarInstalacionesPersonalizadasAsync(registro);

            // 2. Guardar paciente
            var pacienteId = await GuardarPacienteAsync(registro.paciente);

            // 3. Guardar acompañante si aplica
            if (registro.paciente.RequiereAcompananteEnum == RequiereAcompanante.Si)
                await GuardarAcompananteAsync(registro.acompanante, pacienteId);

            // 4. Guardar médico
            var medicoId = await GuardarMedicoAsync(registro.medico, pacienteId);

            // 5. Guardar diagnósticos
            await GuardarDiagnosticosAsync(registro.pacienteDiagnostico, pacienteId);

            // 6. Guardar producto del paciente
            await GuardarProductoPacienteAsync(registro.productoPaciente, pacienteId);

            // 7. Guardar comorbilidades
            await GuardarComorbilidadesAsync(registro.pacienteComorbilidad, pacienteId);

            // 8. Crear solicitud principal
            var solicitudId = await CrearSolicitudPrincipalAsync(pacienteId);

            // 9. Obtener tipos de documento
            var tipoDocumentoMap = await ObtenerTiposDocumentoAsync();

            // 10. Guardar archivos
            await _fileService.GuardarArchivosAdjuntosAsync(documentos, solicitudId, tipoDocumentoMap);

            // 11. Crear historial
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

    public async Task<bool> ValidarSolicitudCompletaAsync(RegistroCanabisUnionModel registro)
    {
        // Validaciones básicas
        if (registro.paciente == null)
            return false;

        if (string.IsNullOrWhiteSpace(registro.paciente.PrimerNombre) ||
            string.IsNullOrWhiteSpace(registro.paciente.PrimerApellido))
            return false;

        if (registro.paciente.FechaNacimiento == null)
            return false;

        // Validar que tenga al menos un diagnóstico
        if (registro.pacienteDiagnostico?.SelectedDiagnosticosIds?.Any() != true &&
            string.IsNullOrWhiteSpace(registro.pacienteDiagnostico?.NombreOtroDiagnostico))
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
                NumSolCompleta = s.NumSolCompleta,
                FechaSolicitud = s.FechaSolicitud,
                EstadoSolicitud = s.EstadoSolicitud != null ? s.EstadoSolicitud.NombreEstado : "Desconocido",
                PacienteNombre = $"{s.Paciente.PrimerNombre} {s.Paciente.PrimerApellido}",
                PacienteCedula = s.Paciente.DocumentoCedula ?? s.Paciente.DocumentoPasaporte ?? "N/A"
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

    private async Task ProcesarInstalacionesPersonalizadasAsync(RegistroCanabisUnionModel registro)
    {
        // Procesar instalación del paciente
        if (!string.IsNullOrWhiteSpace(registro.paciente.pacienteInstalacion))
        {
            var instalacionId = await CrearOGuardarInstalacionPersonalizadaAsync(registro.paciente.pacienteInstalacion);
            registro.paciente.pacienteInstalacionId = instalacionId;
        }

        // Procesar instalación del médico
        if (!string.IsNullOrWhiteSpace(registro.medico.MedicoInstalacion))
        {
            var instalacionId = await CrearOGuardarInstalacionPersonalizadaAsync(registro.medico.MedicoInstalacion);
            registro.medico.medicoInstalacionId = instalacionId;
        }
    }

    private async Task<int> GuardarPacienteAsync(PacienteModel pacienteModel)
    {
        var paciente = new TbPaciente
        {
            PrimerNombre = pacienteModel.PrimerNombre ?? "",
            SegundoNombre = pacienteModel.SegundoNombre,
            PrimerApellido = pacienteModel.PrimerApellido ?? "",
            SegundoApellido = pacienteModel.SegundoApellido,
            TipoDocumento = pacienteModel.TipoDocumentoPacienteEnum.ToString(),
            DocumentoCedula = pacienteModel.TipoDocumentoPacienteEnum == TipoDocumentoPaciente.Cedula
                ? pacienteModel.NumDocCedula
                : null,
            DocumentoPasaporte = pacienteModel.TipoDocumentoPacienteEnum == TipoDocumentoPaciente.Pasaporte
                ? pacienteModel.NumDocPasaporte
                : null,
            Nacionalidad = pacienteModel.Nacionalidad ?? "",
            FechaNacimiento = pacienteModel.FechaNacimiento.HasValue
                ? DateOnly.FromDateTime(pacienteModel.FechaNacimiento.Value)
                : null,
            Sexo = pacienteModel.SexoEnum?.ToString() ?? "",
            TelefonoPersonal = pacienteModel.TelefonoResidencialPersonal,
            TelefonoLaboral = pacienteModel.TelefonoLaboral,
            CorreoElectronico = pacienteModel.CorreoElectronico ?? "",
            ProvinciaId = pacienteModel.pacienteProvinciaId,
            DistritoId = pacienteModel.pacienteDistritoId,
            CorregimientoId = pacienteModel.pacienteCorregimientoId,
            RegionId = pacienteModel.pacienteRegionId,
            InstalacionId = pacienteModel.pacienteInstalacionId,
            DireccionExacta = pacienteModel.DireccionExacta ?? "",
            RequiereAcompanante = pacienteModel.RequiereAcompananteEnum == RequiereAcompanante.Si,
            MotivoRequerimientoAcompanante = pacienteModel.MotivoRequerimientoAcompananteEnum?.ToString(),
            TipoDiscapacidad = pacienteModel.TipoDiscapacidad
        };

        _context.TbPaciente.Add(paciente);
        await _context.SaveChangesAsync();
        return paciente.Id;
    }

    private async Task GuardarAcompananteAsync(AcompananteModel acompananteModel, int pacienteId)
    {
        string numeroDocumento = acompananteModel.TipoDocumentoAcompañanteEnum == TipoDocumentoAcompañante.Cedula
            ? acompananteModel.NumDocCedula ?? ""
            : acompananteModel.NumDocPasaporte ?? "";

        var acompanante = new TbAcompanantePaciente
        {
            PacienteId = pacienteId,
            PrimerNombre = acompananteModel.PrimerNombre ?? "",
            SegundoNombre = acompananteModel.SegundoNombre,
            PrimerApellido = acompananteModel.PrimerApellido ?? "",
            SegundoApellido = acompananteModel.SegundoApellido,
            TipoDocumento = acompananteModel.TipoDocumentoAcompañanteEnum.ToString(),
            NumeroDocumento = numeroDocumento,
            Nacionalidad = acompananteModel.Nacionalidad ?? "",
            Parentesco = acompananteModel.ParentescoEnum?.ToString() ?? acompananteModel.Parentesco ?? "",
            TelefonoMovil = acompananteModel.TelefonoPersonal
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
            MedicoDisciplina = medicoModel.MedicoDisciplinaEnum?.ToString() ?? "",
            MedicoIdoneidad = medicoModel.MedicoIdoneidad ?? "",
            MedicoTelefono = medicoModel.MedicoTelefono ?? "",
            InstalacionId = medicoModel.medicoInstalacionId,
            RegionId = medicoModel.medicoRegionId,
            DetalleMedico = medicoModel.DetalleMedico ?? "Sin detalle",
            PacienteId = pacienteId
        };

        _context.TbMedicoPaciente.Add(medico);
        await _context.SaveChangesAsync();
        return medico.Id;
    }

    private async Task GuardarDiagnosticosAsync(PacienteDiagnosticoModel diagnosticoModel, int pacienteId)
    {
        var diagnosticosList = await _commonService.GetAllDiagnosticsAsync();

        foreach (var diagnosticoId in diagnosticoModel.SelectedDiagnosticosIds)
        {
            var diagnosticoNombre = diagnosticosList.FirstOrDefault(d => d.Id == diagnosticoId)?.Nombre;
            if (!string.IsNullOrEmpty(diagnosticoNombre))
            {
                var diagnostico = new TbPacienteDiagnostico
                {
                    PacienteId = pacienteId,
                    NombreDiagnostico = diagnosticoNombre
                };
                _context.TbPacienteDiagnostico.Add(diagnostico);
            }
        }

        if (diagnosticoModel.IsOtroDiagSelected && !string.IsNullOrWhiteSpace(diagnosticoModel.NombreOtroDiagnostico))
        {
            var otroDiagnostico = new TbPacienteDiagnostico
            {
                PacienteId = pacienteId,
                NombreDiagnostico = diagnosticoModel.NombreOtroDiagnostico
            };
            _context.TbPacienteDiagnostico.Add(otroDiagnostico);
        }

        await _context.SaveChangesAsync();
    }

    private async Task GuardarProductoPacienteAsync(ProductoPacienteModel productoModel, int pacienteId)
    {
        var formasList = await _commonService.GetAllFormasAsync();
        var viasList = await _commonService.GetAllViaAdmAsync();
        var unidadesList = await _commonService.GetAllUnidadesAsync();

        var formasSeleccionadas = formasList
            .Where(f => productoModel.SelectedFormaIds.Contains(f.Id))
            .Select(f => f.Nombre)
            .ToList();

        if (productoModel.IsOtraFormaSelected && !string.IsNullOrWhiteSpace(productoModel.NombreOtraForma))
        {
            formasSeleccionadas.Add(productoModel.NombreOtraForma);
        }

        var viasSeleccionadas = viasList
            .Where(v => productoModel.SelectedViaAdmIds.Contains(v.Id))
            .Select(v => v.Nombre)
            .ToList();

        if (productoModel.IsOtraViaAdmSelected && !string.IsNullOrWhiteSpace(productoModel.NombreOtraViaAdm))
        {
            viasSeleccionadas.Add(productoModel.NombreOtraViaAdm);
        }

        var productoUnidadFinal = productoModel.ProductoUnidadId == 6
            ? productoModel.ProductoUnidad
            : unidadesList.FirstOrDefault(u => u.Id == productoModel.ProductoUnidadId)?.Name;

        var productoPaciente = new TbNombreProductoPaciente
        {
            PacienteId = pacienteId,
            NombreProducto = productoModel.NombreProductoEnum == NombreProductoE.OTRO
                ? productoModel.NombreProducto
                : productoModel.NombreProductoEnum.ToString(),
            NombreComercialProd = productoModel.NombreComercialProd,
            FormaFarmaceutica = string.Join(", ", formasSeleccionadas),
            CantidadConcentracion = productoModel.CantidadConcentracion,
            NombreConcentracion = productoModel.ConcentracionEnum == ConcentracionE.OTRO
                ? productoModel.NombreConcentracion
                : productoModel.ConcentracionEnum.ToString(),
            ViaConsumoProducto = string.Join(", ", viasSeleccionadas),
            ProductoUnidad = productoUnidadFinal,
            ProductoUnidadId = productoModel.ProductoUnidadId == 6 ? null : productoModel.ProductoUnidadId,
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
        if (comorbilidadModel.TieneComorbilidadEnum == TieneComorbilidad.Si &&
            !string.IsNullOrWhiteSpace(comorbilidadModel.NombreDiagnostico))
        {
            var comorbilidad = new TbPacienteComorbilidad
            {
                NombreDiagnostico = comorbilidadModel.NombreDiagnostico,
                DetalleTratamiento = comorbilidadModel.DetalleTratamiento,
                PacienteId = pacienteId
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