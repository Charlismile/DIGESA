using System.ComponentModel.DataAnnotations;
using BlazorBootstrap;
using DIGESA.Components.Pages.Componentes;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Catalogos;
using DIGESA.Models.CannabisModels.Common;
using DIGESA.Models.CannabisModels.Documentos;
using DIGESA.Models.CannabisModels.Formularios;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace DIGESA.Components.Pages.Public;

public partial class Solicitud : ComponentBase
{
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private ICommon _Commonservice { get; set; } = default!;
    [Inject] private DbContextDigesa _context { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    #region variables

    private string tipoTramite = string.Empty;
    private DocumentoAdjuntoViewModel documentos = new();
    private bool mostrarOtraInstalacionPaciente;
    private bool mostrarOtraInstalacionMedico;
    private Dictionary<string, int> tipoDocumentoMap = new();

    private string instalacionFilterPaciente = string.Empty;
    private string instalacionFilterMedico = string.Empty;

    private ValidationMessageStore? messageStore;

    private SolicitudCannabisFormViewModel registro = new();
    private List<ListSustModel> pacienteRegioneslist = new();
    private List<ListSustModel> pacienteProvinciaslist = new();
    private List<ListSustModel> pacienteDistritolist = new();
    private List<ListSustModel> pacienteCorregimientolist = new();
    private List<ListaDiagnostico> pacienteDiagnosticolist = new();
    private List<TbFormaFarmaceutica> productoFormaList = new();
    private List<ListSustModel> productoUnidadList = new();
    private List<TbViaAdministracion> productoViaConsumoList = new();
    private EditContext editContext = default!;
    private bool isProcessing = false;

    private string? instalacionPersonalizadaPaciente;
    private string? instalacionPersonalizadaMedico;

    // MODALS
    private ConsentimientoCannabisModal ConsentimientoModal = default!;
    private bool consentimientoAceptado = false;
    private DynamicModal ModalForm = default!;

    // Referencias a componentes
    private PacienteStep? pacienteComponent;
    private AcompananteStep? acompananteComponent;
    private MedicoStep? medicoComponent;
    private DiagnosticoStep? diagnosticoComponent;
    private ComorbilidadStep? comorbilidadComponent;
    private ArchivoStep? archivosComponent;

    // Steps
    private AutoCompleteDataProviderDelegate<ListSustModel> AutoCompleteDataProvider { get; set; } = default!;
    private int CurrentStepNumber = 1;

    private List<string> Steps = new()
    {
        "DATOS PERSONALES DEL SOLICITANTE",
        "DATOS PERSONALES DEL ACOMPAÑANTE AUTORIZADO",
        "DATOS DEL EL MÉDICO QUE PRESCRIBE",
        "DATOS DEL PACIENTE Y USO DEL CANNABIS MEDICINAL O DERIVADOS",
        "DATOS DE OTRAS ENFERMEDADES QUE PADECE EL PACIENTE",
        "ADJUNTAR DOCUMENTOS REQUERIDOS"
    };

    private int CurrentStepPercentage => (int)Math.Round((double)CurrentStepNumber / Steps.Count * 100);

    #endregion

    protected override async Task OnInitializedAsync()
    {
        registro = new SolicitudCannabisFormViewModel();
        registro.Acompanante ??= new DatosAcompananteVM();
        editContext = new EditContext(registro);
        messageStore = new ValidationMessageStore(editContext);

        pacienteProvinciaslist = await _Commonservice.GetProvincias();
        pacienteRegioneslist = await _Commonservice.GetRegionesSalud();

        pacienteDiagnosticolist = await _Commonservice.GetAllDiagnosticsAsync();
        productoFormaList = await _Commonservice.GetAllFormasAsync();
        productoViaConsumoList = await _Commonservice.GetAllViaAdmAsync();
        productoUnidadList = await _Commonservice.GetUnidadId();

        AutoCompleteDataProvider = AutoCompleteDataProviderHandler;

        await CargarTiposDocumento();
        await InvokeAsync(() => { ConsentimientoModal.Show(); });
    }

    private async Task CargarTiposDocumento()
    {
        try
        {
            var tipos = await _context.TbTipoDocumentoAdjunto
                .Where(t => t.IsActivo == true)
                .ToListAsync();

            tipoDocumentoMap = tipos.ToDictionary(
                t => t.Nombre.Trim(),
                t => t.Id
            );
        }
        catch (Exception ex)
        {
            ModalForm.ShowError($"Error al cargar tipos de documento: {ex.Message}");
        }
    }

    private async Task<AutoCompleteDataProviderResult<ListSustModel>> AutoCompleteDataProviderHandler(
        AutoCompleteDataProviderRequest<ListSustModel> request)
    {
        try
        {
            var filtro = (request.Filter?.Value?.ToString() ?? "").ToUpper();
            var lista = await _Commonservice.GetInstalacionesSalud();

            // Filter the list based on search term
            var filtradas = lista
                .Where(x => string.IsNullOrEmpty(filtro) ||
                            x.Name.Trim().ToUpper().Contains(filtro))
                .Select(x => new ListSustModel
                {
                    Id = x.Id,
                    Name = x.Name.Trim()
                })
                .ToList();

            // Return the result
            return new AutoCompleteDataProviderResult<ListSustModel>
            {
                Data = filtradas,
                TotalCount = filtradas.Count
            };
        }
        catch (Exception ex)
        {
            // Log error and return empty result
            Console.WriteLine($"Error in AutoCompleteDataProviderHandler: {ex.Message}");
            return new AutoCompleteDataProviderResult<ListSustModel>
            {
                Data = new List<ListSustModel>(),
                TotalCount = 0
            };
        }
    }

    private void HandleAutoCompletePacienteChanged(ListSustModel? sel)
    {
        if (sel != null)
        {
            registro.Paciente.InstalacionSaludId = sel.Id;
            mostrarOtraInstalacionPaciente = false;
            instalacionPersonalizadaPaciente = null;
        }
    }

    private void HandleAutoCompleteMedicoChanged(ListSustModel? sel)
    {
        if (sel != null)
        {
            registro.Medico.InstalacionSaludId = sel.Id;
            mostrarOtraInstalacionMedico = false;
            instalacionPersonalizadaMedico = null;
        }
    }

    private async Task HandlePacienteProvinciaChanged(int id)
    {
        registro.Paciente.ProvinciaId = id;
        pacienteDistritolist = await _Commonservice.GetDistritos(id);
        pacienteCorregimientolist.Clear();
        registro.Paciente.CorregimientoId = null;
        StateHasChanged();
    }

    private async Task HandlePacienteDistritoChanged(int id)
    {
        registro.Paciente.DistritoId = id;
        pacienteCorregimientolist = await _Commonservice.GetCorregimientos(id);
        registro.Paciente.CorregimientoId = null;
        StateHasChanged();
    }

    private async Task HandleUnidadSeleccionadaChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int selectedId))
        {
            registro.Producto.ProductoUnidadId = selectedId;
        }
        else
        {
            registro.Producto.ProductoUnidadId = null;
        }

        await Task.CompletedTask;
    }

    private void HandleMostrarOtraInstalacionPacienteChanged(ChangeEventArgs e)
    {
        mostrarOtraInstalacionPaciente = (bool)(e.Value ?? false);
        if (mostrarOtraInstalacionPaciente)
        {
            registro.Paciente.InstalacionSaludId = null;
            instalacionFilterPaciente = string.Empty;
        }
        else
        {
            registro.Paciente.InstalacionSaludPersonalizada = null;
        }
    }

    private void HandleMostrarOtraInstalacionMedicoChanged(ChangeEventArgs e)
    {
        mostrarOtraInstalacionMedico = (bool)(e.Value ?? false);
        if (mostrarOtraInstalacionMedico)
        {
            registro.Medico.InstalacionSaludId = null;
            instalacionFilterMedico = string.Empty;
        }
        else
        {
            registro.Medico.InstalacionSaludPersonalizada = null;
        }
    }

    private void HandlePreviousStep()
    {
        if (CurrentStepNumber > 1)
        {
            // Si vamos al paso 2 pero no requiere acompañante, ir al paso 1
            if (CurrentStepNumber == 3 && registro.Paciente.RequiereAcompanante != EnumViewModel.RequiereAcompanante.Si)
            {
                CurrentStepNumber = 1;
            }
            else
            {
                CurrentStepNumber--;
            }
        }
    }

    private async Task HandleNextStep()
    {
        if (!consentimientoAceptado)
        {
            ModalForm.ShowWarning(
                "Debe aceptar el consentimiento informado antes de continuar.",
                "Acción no permitida"
            );
            return;
        }

        if (isProcessing) return;

        isProcessing = true;
        try
        {
            bool isValid = true;

            switch (CurrentStepNumber)
            {
                case 1:
                    isValid = pacienteComponent != null && await pacienteComponent.ValidateAsync();
                    if (isValid)
                    {
                        // Si no requiere acompañante, saltar al paso 3 directamente
                        if (registro.Paciente.RequiereAcompanante != EnumViewModel.RequiereAcompanante.Si)
                        {
                            CurrentStepNumber = 3;
                            return;
                        }
                    }

                    break;
                case 2:
                    if (acompananteComponent != null &&
                        registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si)
                        isValid = await acompananteComponent.ValidateAsync();
                    break;
                case 3:
                    isValid = medicoComponent != null && await medicoComponent.ValidateAsync();
                    break;
                case 4:
                    isValid = diagnosticoComponent != null && await diagnosticoComponent.ValidateAsync();
                    break;
                case 5:
                    isValid = comorbilidadComponent != null && await comorbilidadComponent.ValidateAsync();
                    break;
                case 6:
                    isValid = archivosComponent != null && await archivosComponent.ValidateAsync();
                    break;
            }

            if (!isValid) return;

            if (CurrentStepNumber == Steps.Count)
            {
                await SaveFormData();
                return;
            }

            // Actualizar la lógica de navegación entre pasos
            int nextStep = CurrentStepNumber;

            // Si estamos en el paso 1 y no requiere acompañante, saltar al paso 3
            if (CurrentStepNumber == 1 && registro.Paciente.RequiereAcompanante != EnumViewModel.RequiereAcompanante.Si)
            {
                nextStep = 3;
            }
            else if (CurrentStepNumber == 2 &&
                     registro.Paciente.RequiereAcompanante != EnumViewModel.RequiereAcompanante.Si)
            {
                // Si por algún motivo estamos en el paso 2 sin requerir acompañante, ir al paso 3
                nextStep = 3;
            }
            else
            {
                // Avanzar normalmente
                nextStep = CurrentStepNumber + 1;
            }

            // Asegurarse de no saltarse el paso 4 (diagnóstico)
            if (nextStep == 4)
            {
                CurrentStepNumber = 4;
            }
            else if (nextStep <= Steps.Count)
            {
                CurrentStepNumber = nextStep;
            }
        }
        finally
        {
            isProcessing = false;
            StateHasChanged();
        }
    }

    private async void HandleConsentimientoDecision(bool aceptado)
    {
        consentimientoAceptado = aceptado;

        if (!aceptado)
        {
            ModalForm.ShowWarning(
                "Debe aceptar el consentimiento informado para poder continuar con la solicitud.",
                "Consentimiento requerido"
            );

            await Task.Delay(1200);
            NavigationManager.NavigateTo("/", true);
        }
    }

    private async Task HandleFileChange((InputFileChangeEventArgs e, string campo) args)
    {
        var e = args.e;
        var campo = args.campo;
        var files = e.GetMultipleFiles();
        const long maxFileSize = 5 * 1024 * 1024;

        foreach (var file in files)
        {
            if (file.Size > maxFileSize)
            {
                await MostrarAdvertencia($"El archivo '{file.Name}' excede el tamaño máximo permitido de 5MB. " +
                                         $"Tamaño actual: {(file.Size / 1024f / 1024f):F2}MB");
                continue;
            }

            switch (campo)
            {
                case "CedulaPaciente":
                    documentos.CedulaPaciente.Add(file);
                    break;
                case "CertificacionMedica":
                    documentos.CertificacionMedica.Add(file);
                    break;
                case "FotoPaciente":
                    documentos.FotoPaciente.Add(file);
                    break;
                case "CedulaAcompanante":
                    documentos.CedulaAcompanante.Add(file);
                    break;
                case "SentenciaTutor":
                    documentos.SentenciaTutor.Add(file);
                    break;
                case "Antecedentes":
                    documentos.Antecedentes.Add(file);
                    break;
                case "IdentidadMenor":
                    documentos.IdentidadMenor.Add(file);
                    break;
                case "ConsentimientoPadres":
                    documentos.ConsentimientoPadres.Add(file);
                    break;
                case "CertificadoNacimientoMenor":
                    documentos.CertificadoNacimientoMenor.Add(file);
                    break;
                case "FotoAcompanante":
                    documentos.FotoAcompanante.Add(file);
                    break;
            }
        }

        StateHasChanged();
    }

    private async Task MostrarAdvertencia(string mensaje)
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("alert", mensaje);
        }
        catch
        {
            Console.WriteLine($"ADVERTENCIA: {mensaje}");
        }
    }

    private async Task SaveFormData()
    {
        try
        {
            // Primero validar todos los pasos
            bool allValid = await ValidateAllSteps();
            if (!allValid)
            {
                ModalForm.ShowError("Por favor, complete todos los campos requeridos correctamente.");
                return;
            }

            // Validar específicamente el tipo de trámite
            if (string.IsNullOrEmpty(tipoTramite))
            {
                ModalForm.ShowError("Debe seleccionar el tipo de trámite antes de continuar.");
                CurrentStepNumber = 6; // Ir al paso de archivos
                StateHasChanged();
                return;
            }

            // Validar que se hayan subido los archivos requeridos según el tipo de trámite
            if (archivosComponent != null)
            {
                bool archivosValidos = await archivosComponent.ValidateAsync();
                if (!archivosValidos)
                {
                    ModalForm.ShowError("Debe subir todos los documentos requeridos según el tipo de trámite seleccionado.");
                    return;
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Procesar instalaciones personalizadas
                if (mostrarOtraInstalacionPaciente &&
                    !string.IsNullOrEmpty(registro.Paciente.InstalacionSaludPersonalizada))
                {
                    var instalacionId =
                        await CrearOGuardarInstalacionPersonalizada(registro.Paciente.InstalacionSaludPersonalizada);
                    registro.Paciente.InstalacionSaludId = instalacionId;
                }

                if (mostrarOtraInstalacionMedico &&
                    !string.IsNullOrEmpty(registro.Medico.InstalacionSaludPersonalizada))
                {
                    var instalacionId =
                        await CrearOGuardarInstalacionPersonalizada(registro.Medico.InstalacionSaludPersonalizada);
                    registro.Medico.InstalacionSaludId = instalacionId;
                }

                // 1. Guardar Paciente
                var paciente = new TbPaciente
                {
                    PrimerNombre = registro.Paciente.PrimerNombre,
                    SegundoNombre = registro.Paciente.SegundoNombre,
                    PrimerApellido = registro.Paciente.PrimerApellido,
                    SegundoApellido = registro.Paciente.SegundoApellido,
                    TipoDocumento = registro.Paciente.TipoDocumento.ToString(),
                    DocumentoCedula = registro.Paciente.TipoDocumento == EnumViewModel.TipoDocumento.Cedula
                        ? registro.Paciente.NumeroDocumento
                        : null,
                    DocumentoPasaporte = registro.Paciente.TipoDocumento == EnumViewModel.TipoDocumento.Pasaporte
                        ? registro.Paciente.NumeroDocumento
                        : null,
                    Nacionalidad = registro.Paciente.Nacionalidad,
                    FechaNacimiento = registro.Paciente.FechaNacimiento.HasValue
                        ? DateOnly.FromDateTime(registro.Paciente.FechaNacimiento.Value)
                        : null,
                    Sexo = registro.Paciente.Sexo.ToString(),
                    RequiereAcompanante = registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si,
                    MotivoRequerimientoAcompanante = registro.Paciente.MotivoRequerimientoAcompanante?.ToString(),
                    TipoDiscapacidad = registro.Paciente.TipoDiscapacidad,
                    TelefonoPersonal = registro.Paciente.TelefonoPersonal,
                    TelefonoLaboral = registro.Paciente.TelefonoLaboral,
                    CorreoElectronico = registro.Paciente.CorreoElectronico,
                    ProvinciaId = registro.Paciente.ProvinciaId,
                    DistritoId = registro.Paciente.DistritoId,
                    CorregimientoId = registro.Paciente.CorregimientoId,
                    DireccionExacta = registro.Paciente.DireccionExacta,
                    RegionId = registro.Paciente.RegionSaludId,
                    InstalacionId = registro.Paciente.InstalacionSaludId,
                    InstalacionPersonalizada = registro.Paciente.InstalacionSaludPersonalizada
                };

                _context.TbPaciente.Add(paciente);
                await _context.SaveChangesAsync();

                // 2. Guardar Solicitud
                var numeroSecuencia = await ObtenerNumeroSecuenciaAsync();
                var fechaActual = DateTime.Now;
                var numeroCompleto = $"{numeroSecuencia:000}-{fechaActual.Year}-{fechaActual.Month:00}";

                var solicitud = new TbSolRegCannabis
                {
                    PacienteId = paciente.Id,
                    FechaSolicitud = fechaActual,
                    EstadoSolicitudId = 1,
                    EsRenovacion = registro.EsRenovacion,
                    FechaRevision = null,
                    NumSolSecuencia = numeroSecuencia,
                    NumSolAnio = fechaActual.Year,
                    NumSolMes = fechaActual.Month,
                    NumSolCompleta = numeroCompleto,
                    CreadaPor = "Usuario Sistema",
                    ModificadaEn = DateOnly.FromDateTime(fechaActual),
                    ModificadaPor = "Usuario Sistema",
                    FechaAprobacion = null,
                    CarnetActivo = false,
                    VersionCarnet = 1
                };

                _context.TbSolRegCannabis.Add(solicitud);
                await _context.SaveChangesAsync();

                // 3. Guardar Múltiples Productos
                await GuardarProductosPacienteAsync(solicitud.Id, paciente.Id, registro.Producto);

                // 4. Guardar Diagnóstico
                await GuardarDiagnosticosAsync(paciente.Id, registro.Diagnostico);

                // 5. Guardar Médico
                await GuardarMedicoAsync(paciente.Id, registro.Medico);

                // 6. Guardar Acompañante si es necesario
                if (registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si &&
                    registro.Acompanante != null)
                {
                    await GuardarAcompananteAsync(paciente.Id, registro.Acompanante);
                }

                // 7. Guardar Comorbilidad
                await GuardarComorbilidadAsync(paciente.Id, registro.Comorbilidad);

                // 8. Guardar Declaración Jurada
                await GuardarDeclaracionJuradaAsync(solicitud.Id, registro.Declaracion);

                // 9. Guardar Documentos (archivos)
                await GuardarDocumentosAsync(solicitud.Id, documentos);

                await transaction.CommitAsync();

                ModalForm.ShowSuccess($"Solicitud guardada exitosamente. Número de solicitud: {numeroCompleto}");
                await Task.Delay(2000);
                NavigationManager.NavigateTo("/", forceLoad: true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModalForm.ShowError($"Error al guardar: {ex.Message}");
                Console.WriteLine($"Error detallado: {ex}");
            }
        }
        catch (Exception ex)
        {
            ModalForm.ShowError($"Error general: {ex.Message}");
        }
    }

    // NUEVO MÉTODO: Guardar múltiples productos
    private async Task GuardarProductosPacienteAsync(int solicitudId, int pacienteId, DatosProductoVM productoVM)
    {
        for (int i = 0; i < productoVM.Productos.Count; i++)
        {
            var producto = productoVM.Productos[i];

            // Determinar nombre del producto basado en el enum
            string nombreProducto = producto.NombreProducto;
            if (producto.NombreProductoEnum != EnumViewModel.NombreProductoE.OTRO)
            {
                nombreProducto = producto.NombreProductoEnum.ToString();
            }

            var productoEntity = new TbNombreProductoPaciente
            {
                SolicitudId = solicitudId,
                PacienteId = pacienteId,
                EsProductoPrincipal = i == 0,
                Orden = i + 1,
                NombreProducto = nombreProducto,
                NombreComercialProd = producto.NombreComercialProd,
                FormaFarmaceutica = ObtenerNombresFormasFarmaceuticas(producto),
                CantidadConcentracion = producto.CantidadConcentracion,
                NombreConcentracion = ObtenerNombresConcentraciones(producto),
                ViaConsumoProducto = ObtenerNombresViasAdministracion(producto),
                DetDosisPaciente = producto.DetDosisPaciente,
                UsaDosisRescate = producto.UsaDosisRescateEnum == EnumViewModel.UsaDosisRescate.Si,
                ProductoUnidad = await ObtenerNombreUnidadAsync(producto.ProductoUnidadId),
                DosisDuracion = producto.DosisDuracion,
                DosisFrecuencia = producto.DosisFrecuencia,
                DetDosisRescate = producto.DetDosisRescate,
                ProductoUnidadId = producto.ProductoUnidadId,
                FormaFarmaceuticaId = producto.SelectedFormaIds.FirstOrDefault(),
                ViaAdministracionId = producto.SelectedViaAdmIds.FirstOrDefault()
            };

            _context.TbNombreProductoPaciente.Add(productoEntity);
        }

        await _context.SaveChangesAsync();
    }

    // Métodos auxiliares
    private string ObtenerNombresFormasFarmaceuticas(ProductoIndividualVM producto)
    {
        var nombres = new List<string>();

        // Obtener nombres de formas seleccionadas
        foreach (var formaId in producto.SelectedFormaIds)
        {
            var forma = productoFormaList.FirstOrDefault(f => f.Id == formaId);
            if (forma != null)
                nombres.Add(forma.Nombre);
        }

        // Agregar forma "Otro" si está especificada
        if (!string.IsNullOrEmpty(producto.NombreOtraForma))
            nombres.Add(producto.NombreOtraForma);

        return string.Join(", ", nombres);
    }

    private string ObtenerNombresConcentraciones(ProductoIndividualVM producto)
    {
        var nombres = new List<string>();

        if (producto.ConcentracionesSeleccionadas.Contains(EnumViewModel.ConcentracionE.CBD))
            nombres.Add("CBD");
        if (producto.ConcentracionesSeleccionadas.Contains(EnumViewModel.ConcentracionE.THC))
            nombres.Add("THC");
        if (!string.IsNullOrEmpty(producto.NombreOtraConcentracion))
            nombres.Add(producto.NombreOtraConcentracion);

        return string.Join(", ", nombres);
    }

    private string ObtenerNombresViasAdministracion(ProductoIndividualVM producto)
    {
        var nombres = new List<string>();

        // Obtener nombres de vías seleccionadas
        foreach (var viaId in producto.SelectedViaAdmIds)
        {
            var via = productoViaConsumoList.FirstOrDefault(v => v.Id == viaId);
            if (via != null)
                nombres.Add(via.Nombre);
        }

        // Agregar vía "Otro" si está especificada
        if (!string.IsNullOrEmpty(producto.NombreOtraViaAdm))
            nombres.Add(producto.NombreOtraViaAdm);

        return string.Join(", ", nombres);
    }

    private async Task<string?> ObtenerNombreUnidadAsync(int? unidadId)
    {
        if (unidadId.HasValue)
        {
            var unidad = await _context.TbUnidades
                .FirstOrDefaultAsync(u => u.Id == unidadId.Value);
            return unidad?.NombreUnidad;
        }

        return null;
    }

    // Método para obtener número de secuencia
    private async Task<int> ObtenerNumeroSecuenciaAsync()
    {
        try
        {
            var anioActual = DateTime.Now.Year;
            var secuencia = await _context.TbSolSecuencia
                .FirstOrDefaultAsync(s => s.IdEntidad == 1 && s.Anio == anioActual);

            if (secuencia == null)
            {
                secuencia = new TbSolSecuencia
                {
                    IdEntidad = 1,
                    Anio = anioActual,
                    Numeracion = 1,
                    IsActivo = true
                };
                _context.TbSolSecuencia.Add(secuencia);
            }
            else
            {
                secuencia.Numeracion++;
            }

            await _context.SaveChangesAsync();
            return secuencia.Numeracion ?? 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener secuencia: {ex.Message}");
            // Retornar un número temporal basado en timestamp
            return (int)(DateTime.Now.Ticks % 1000000);
        }
    }

    // Método para guardar diagnósticos
    private async Task GuardarDiagnosticosAsync(int pacienteId, DiagnosticoViewModel diagnostico)
    {
        // Guardar diagnósticos seleccionados
        foreach (var diagnosticoId in diagnostico.SelectedDiagnosticosIds)
        {
            var diag = new TbPacienteDiagnostico
            {
                PacienteId = pacienteId,
                DiagnosticoId = diagnosticoId,
                Tipo = "PRINCIPAL",
                FechaDiagnostico = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.TbPacienteDiagnostico.Add(diag);
        }

        // Guardar diagnóstico "Otro" si existe
        if (!string.IsNullOrEmpty(diagnostico.NombreOtroDiagnostico))
        {
            var diagOtro = new TbPacienteDiagnostico
            {
                PacienteId = pacienteId,
                NombreDiagnostico = diagnostico.NombreOtroDiagnostico,
                Tipo = "OTRO",
                FechaDiagnostico = DateOnly.FromDateTime(DateTime.Now)
            };
            _context.TbPacienteDiagnostico.Add(diagOtro);
        }

        await _context.SaveChangesAsync();
    }

    // Método para guardar médico
    private async Task GuardarMedicoAsync(int pacienteId, DatosMedicosVM medico)
    {
        var medicoEntity = new TbMedicoPaciente
        {
            PacienteId = pacienteId,
            PrimerNombre = medico.PrimerNombre,
            PrimerApellido = medico.PrimerApellido,
            MedicoDisciplina = medico.MedicoDisciplina.ToString(),
            MedicoIdoneidad = medico.MedicoIdoneidad,
            MedicoTelefono = medico.TelefonoPersonal,
            RegionId = medico.RegionSaludId,
            InstalacionId = medico.InstalacionSaludId,
            InstalacionPersonalizada = medico.InstalacionSaludPersonalizada,
            DetalleMedico = medico.DetalleEspecialidad ?? "Sin detalle"
        };

        _context.TbMedicoPaciente.Add(medicoEntity);
        await _context.SaveChangesAsync();
    }

    // Método para guardar acompañante
    private async Task GuardarAcompananteAsync(int pacienteId, DatosAcompananteVM acompanante)
    {
        var acompananteEntity = new TbAcompanantePaciente
        {
            PacienteId = pacienteId,
            PrimerNombre = acompanante.PrimerNombre,
            SegundoNombre = acompanante.SegundoNombre,
            PrimerApellido = acompanante.PrimerApellido,
            SegundoApellido = acompanante.SegundoApellido,
            TipoDocumento = acompanante.TipoDocumento.ToString(),
            NumeroDocumento = acompanante.NumeroDocumento,
            Nacionalidad = acompanante.Nacionalidad,
            Parentesco = acompanante.Parentesco.ToString(),
            TelefonoMovil = acompanante.TelefonoMovil
        };

        _context.TbAcompanantePaciente.Add(acompananteEntity);
        await _context.SaveChangesAsync();
    }

    // Método para guardar comorbilidad
    private async Task GuardarComorbilidadAsync(int pacienteId, ComorbilidadViewModel comorbilidad)
    {
        if (comorbilidad.TieneComorbilidadEnum == EnumViewModel.TieneComorbilidad.Si &&
            !string.IsNullOrEmpty(comorbilidad.NombreDiagnostico))
        {
            var comorbilidadEntity = new TbPacienteComorbilidad
            {
                PacienteId = pacienteId,
                NombreDiagnostico = comorbilidad.NombreDiagnostico,
                DetalleTratamiento = comorbilidad.DetalleTratamiento,
                FechaDiagnostico = DateOnly.FromDateTime(DateTime.Now),
                TieneComorbilidad = true
            };

            _context.TbPacienteComorbilidad.Add(comorbilidadEntity);
            await _context.SaveChangesAsync();
        }
    }

    // Método para guardar declaración jurada
    private async Task GuardarDeclaracionJuradaAsync(int solicitudId, DeclaracionJuradaViewModel declaracion)
    {
        if (declaracion.Aceptada)
        {
            var declaracionEntity = new TbDeclaracionJurada
            {
                SolRegCannabisId = solicitudId,
                Detalle = "Declaración jurada de veracidad de información",
                Fecha = DateOnly.FromDateTime(DateTime.Now),
                NombreDeclarante = registro.Paciente.NombreCompleto,
                Aceptada = true
            };

            _context.TbDeclaracionJurada.Add(declaracionEntity);
            await _context.SaveChangesAsync();
        }
    }

    // Método para guardar documentos
    private async Task GuardarDocumentosAsync(int solicitudId, DocumentoAdjuntoViewModel documentosVM)
    {
        // Crear directorio para la solicitud si no existe
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", solicitudId.ToString());
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Mapeo de tipos de documentos
        var tipoDocumentoLocalMap = new Dictionary<string, (int tipoId, string categoria)>
        {
            { "CedulaPaciente", (ObtenerTipoDocumentoId("Cédula del Paciente"), "Paciente") },
            { "CertificacionMedica", (ObtenerTipoDocumentoId("Certificación Médica"), "Paciente") },
            { "FotoPaciente", (ObtenerTipoDocumentoId("Foto Carnet"), "Paciente") },
            { "CedulaAcompanante", (ObtenerTipoDocumentoId("Cédula del Acompañante"), "Acompañante") },
            { "SentenciaTutor", (ObtenerTipoDocumentoId("Sentencia Judicial"), "Acompañante") },
            { "Antecedentes", (ObtenerTipoDocumentoId("Antecedentes Penales"), "Acompañante") },
            { "IdentidadMenor", (ObtenerTipoDocumentoId("Identidad del Menor"), "Paciente") },
            { "ConsentimientoPadres", (ObtenerTipoDocumentoId("Consentimiento de Padres"), "Acompañante") },
            { "CertificadoNacimientoMenor", (ObtenerTipoDocumentoId("Certificado de Nacimiento"), "Paciente") },
            { "FotoAcompanante", (ObtenerTipoDocumentoId("Foto Carnet Acompañante"), "Acompañante") }
        };

        // Función para guardar un archivo
        async Task GuardarArchivo(IBrowserFile file, string campo, int version = 1)
        {
            if (file == null) return;

            try
            {
                // Generar nombre único para el archivo
                var extension = Path.GetExtension(file.Name);
                var nombreUnico = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(uploadsFolder, nombreUnico);
                var rutaRelativa = $"/uploads/{solicitudId}/{nombreUnico}";

                // Guardar archivo en disco
                using var stream = new FileStream(rutaCompleta, FileMode.Create);
                await file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024).CopyToAsync(stream);

                // Obtener tipo de documento
                if (tipoDocumentoLocalMap.TryGetValue(campo, out var tipoInfo))
                {
                    var documento = new TbDocumentoAdjunto
                    {
                        SolRegCannabisId = solicitudId,
                        TipoDocumentoId = tipoInfo.tipoId,
                        NombreOriginal = file.Name,
                        NombreGuardado = nombreUnico,
                        Url = rutaRelativa,
                        FechaSubidaUtc = DateTime.UtcNow,
                        SubidoPor = "Usuario Sistema",
                        IsValido = true,
                        EsDocumentoMedico = false,
                        Categoria = tipoInfo.categoria,
                        Version = version
                    };

                    _context.TbDocumentoAdjunto.Add(documento);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar archivo {file.Name}: {ex.Message}");
            }
        }

        // Guardar todos los documentos
        int version = 1;

        // Cédula del Paciente
        foreach (var file in documentosVM.CedulaPaciente)
        {
            await GuardarArchivo(file, "CedulaPaciente", version);
        }

        // Certificación Médica
        foreach (var file in documentosVM.CertificacionMedica)
        {
            await GuardarArchivo(file, "CertificacionMedica", version);
        }

        // Foto Paciente
        foreach (var file in documentosVM.FotoPaciente)
        {
            await GuardarArchivo(file, "FotoPaciente", version);
        }

        // Cédula Acompañante
        foreach (var file in documentosVM.CedulaAcompanante)
        {
            await GuardarArchivo(file, "CedulaAcompanante", version);
        }

        // Sentencia Judicial
        foreach (var file in documentosVM.SentenciaTutor)
        {
            await GuardarArchivo(file, "SentenciaTutor", version);
        }

        // Antecedentes Penales
        foreach (var file in documentosVM.Antecedentes)
        {
            await GuardarArchivo(file, "Antecedentes", version);
        }

        // Identidad Menor (si aplica)
        foreach (var file in documentosVM.IdentidadMenor)
        {
            await GuardarArchivo(file, "IdentidadMenor", version);
        }

        // Consentimiento Padres (si aplica)
        foreach (var file in documentosVM.ConsentimientoPadres)
        {
            await GuardarArchivo(file, "ConsentimientoPadres", version);
        }

        // Certificado Nacimiento Menor (si aplica)
        foreach (var file in documentosVM.CertificadoNacimientoMenor)
        {
            await GuardarArchivo(file, "CertificadoNacimientoMenor", version);
        }

        // Foto Acompañante
        foreach (var file in documentosVM.FotoAcompanante)
        {
            await GuardarArchivo(file, "FotoAcompanante", version);
        }

        await _context.SaveChangesAsync();
    }

    // Método auxiliar para obtener ID de tipo de documento
    private int ObtenerTipoDocumentoId(string nombreTipo)
    {
        if (string.IsNullOrEmpty(nombreTipo))
            return 0;

        try
        {
            var tipo = _context.TbTipoDocumentoAdjunto
                .FirstOrDefault(t => t.Nombre.Trim().ToLower() == nombreTipo.ToLower());

            return tipo?.Id ?? 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener tipo de documento '{nombreTipo}': {ex.Message}");
            return 0;
        }
    }

    private async Task<bool> ValidateAllSteps()
    {
        bool isValid = true;

        if (pacienteComponent != null)
            isValid &= await pacienteComponent.ValidateAsync();

        if (acompananteComponent != null &&
            registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si)
            isValid &= await acompananteComponent.ValidateAsync();

        if (medicoComponent != null)
            isValid &= await medicoComponent.ValidateAsync();

        if (diagnosticoComponent != null)
            isValid &= await diagnosticoComponent.ValidateAsync();

        if (comorbilidadComponent != null)
            isValid &= await comorbilidadComponent.ValidateAsync();

        if (archivosComponent != null)
            isValid &= await archivosComponent.ValidateAsync();

        return isValid;
    }

    private async Task<int?> CrearOGuardarInstalacionPersonalizada(string nombreInstalacion)
    {
        if (string.IsNullOrEmpty(nombreInstalacion))
            return null;

        var instalacionExistente = await _context.TbInstalacionSalud
            .FirstOrDefaultAsync(i => i.Nombre.ToLower() == nombreInstalacion.ToLower());

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

    private void HandleValidSubmit()
    {
        // Este método se llama cuando el formulario es válido
        // La lógica de guardado ya se maneja en HandleNextStep para el último paso
    }
}