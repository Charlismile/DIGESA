using System.ComponentModel.DataAnnotations;
using BlazorBootstrap;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Common;
using DIGESA.Models.CannabisModels.Formularios;
using DIGESA.Models.CannabisModels.Renovaciones;
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
    private DocumentosModel documentos = new();
    private bool mostrarOtraInstalacionPaciente { get; set; }
    private bool mostrarOtraInstalacionMedico { get; set; }
    private Dictionary<string, int> tipoDocumentoMap = new Dictionary<string, int>();

    private string instalacionFilterPaciente = string.Empty;
    private string instalacionFilterMedico = string.Empty;

    [Required(ErrorMessage = "Debe especificar la unidad si seleccionó 'Otro'.")]
    private ValidationMessageStore? messageStore;

    private SolicitudCannabisFormViewModel registro { get; set; } = new();
    private List<ListSustModel> pacienteRegioneslist { get; set; } = new();
    private List<ListSustModel> pacienteProvincicaslist { get; set; } = new();
    private List<ListSustModel> pacienteDistritolist { get; set; } = new();
    private List<ListSustModel> pacienteCorregimientolist { get; set; } = new();
    private List<ListaDiagnostico> pacienteDiagnosticolist { get; set; } = new();
    private List<TbFormaFarmaceutica> productoFormaList { get; set; } = new();
    private List<ListSustModel> productoUnidadList { get; set; } = new();
    private List<TbViaAdministracion> productoViaConsumoList { get; set; } = new();

    private EditContext editContext = default!;
    
    private DynamicModal ModalForm = default!;
    
    private bool isProcessing = false;

    // Propiedades temporales para instalaciones personalizadas
    private string? instalacionPersonalizadaPaciente;
    private string? instalacionPersonalizadaMedico;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        registro = new SolicitudCannabisFormViewModel();
        registro.Acompanante ??= new DatosAcompananteVM();
        editContext = new EditContext(registro);
        messageStore = new ValidationMessageStore(editContext);


        editContext = new EditContext(registro);
        messageStore = new ValidationMessageStore(editContext);

        pacienteProvincicaslist = await _Commonservice.GetProvincias();
        pacienteRegioneslist = await _Commonservice.GetRegionesSalud();

        pacienteDiagnosticolist = await _Commonservice.GetAllDiagnosticsAsync();
        productoFormaList = await _Commonservice.GetAllFormasAsync();
        productoViaConsumoList = await _Commonservice.GetAllViaAdmAsync();
        productoUnidadList = await _Commonservice.GetUnidadId();

        // Cargar tipos de documento
        await CargarTiposDocumento();
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

            ModalForm.ShowSuccess($"Tipos de documento cargados: {tipoDocumentoMap.Count}");
        }
        catch (Exception ex)
        {
            ModalForm.ShowError($"Error al cargar tipos de documento: {ex.Message}");
        }
    }

    private async Task<AutoCompleteDataProviderResult<ListSustModel>> AutoCompleteDataProvider(
        AutoCompleteDataProviderRequest<ListSustModel> request)
    {
        var filtro = (request.Filter?.Value?.ToString() ?? "").ToUpper();
        var lista = await _Commonservice.GetInstalacionesSalud();

        var filtradas = lista.Select(x => new ListSustModel
        {
            Id = x.Id,
            Name = x.Name.Trim()
        });

        return new AutoCompleteDataProviderResult<ListSustModel>
        {
            Data = filtradas,
            TotalCount = filtradas.Count()
        };
    }

    private void OnAutoCompletePacienteChanged(ListSustModel? sel)
    {
        if (sel != null)
        {
            registro.Paciente.InstalacionSaludId = sel.Id;
            mostrarOtraInstalacionPaciente = false;
            instalacionPersonalizadaPaciente = null;
        }
    }

    private void OnAutoCompleteMedicoChanged(ListSustModel? sel)
    {
        if (sel != null)
        {
            registro.Medico.InstalacionSaludId = sel.Id;
            mostrarOtraInstalacionMedico = false;
            instalacionPersonalizadaMedico = null;
        }
    }

    private async Task pacienteProvinciaChanged(int id)
    {
        registro.Paciente.ProvinciaId = id;
        pacienteDistritolist = await _Commonservice.GetDistritos(id);
        pacienteCorregimientolist.Clear();
        registro.Paciente.CorregimientoId = null;
    }

    private async Task pacienteDistritoChanged(int id)
    {
        registro.Paciente.DistritoId = id;
        pacienteCorregimientolist = await _Commonservice.GetCorregimientos(id);
        registro.Paciente.CorregimientoId = null;
    }

    private void OnUnidadSeleccionadaChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int selectedId))
        {
            registro.Producto.ProductoUnidadId = selectedId;
        }
        else
        {
            registro.Producto.ProductoUnidadId = null;
        }
    }

    private void OnMostrarOtraInstalacionPacienteChanged(ChangeEventArgs e)
    {
        mostrarOtraInstalacionPaciente = (bool)(e.Value ?? false);
        if (mostrarOtraInstalacionPaciente)
        {
            registro.Paciente.InstalacionSaludId = null;
            instalacionFilterPaciente = string.Empty;
        }
        else
        {
            instalacionPersonalizadaPaciente = null;
        }
    }

    private void OnMostrarOtraInstalacionMedicoChanged(ChangeEventArgs e)
    {
        bool nuevoValor = false;
        if (e?.Value != null && bool.TryParse(e.Value.ToString(), out var b))
            nuevoValor = b;

        mostrarOtraInstalacionMedico = nuevoValor;

        if (mostrarOtraInstalacionMedico)
        {
            registro.Medico.InstalacionSaludId = null;
            instalacionFilterMedico = string.Empty;
        }
        else
        {
            instalacionPersonalizadaMedico = null;
        }

        messageStore?.Clear(new FieldIdentifier(registro.Medico, nameof(registro.Medico.InstalacionSaludId)));
        editContext?.NotifyValidationStateChanged();
    }

    private int currentStepNumber = 1;
    private bool hasMobileApp = false;

    private List<string> steps = new List<string>
    {
        "DATOS PERSONALES DEL SOLICITANTE",
        "DATOS PERSONALES DEL ACOMPAÑANTE AUTORIZADO",
        "DATOS DEL EL MÉDICO QUE PRESCRIBE",
        "DATOS DEL PACIENTE Y USO DEL CANNABIS MEDICINAL O DERIVADOS",
        "DATOS DE OTRAS ENFERMEDADES QUE PADECE EL PACIENTE",
        "ADJUNTAR DOCUMENTOS REQUERIDOS"
    };

    private int currentStep => (int)Math.Round((double)currentStepNumber / steps.Count * 100);

    private void OnRequiereAcompananteChanged()
    {
        if (registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.No && currentStepNumber == 2)
        {
            currentStepNumber = 3;
        }
    }

    private void PreviousStep()
    {
        if (currentStepNumber > 1)
        {
            currentStepNumber--;

            if (currentStepNumber == 2 && registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.No)
            {
                currentStepNumber = 1;
            }
        }
    }

    private async Task NextStep()
    {
        if (isProcessing)
        {
            Console.WriteLine("NextStep: ya se está procesando, ignorando reentrada.");
            return;
        }

        isProcessing = true;
        StateHasChanged();

        try
        {
            if (currentStepNumber == 1)
            {
                if (!ValidateStep1()) return;
            }

            if (currentStepNumber == 2)
            {
                if (!ValidateStep2()) return;
            }

            if (currentStepNumber == 3)
            {
                if (!ValidateStep3()) return;
            }

            if (currentStepNumber == 4)
            {
                if (!ValidateStep4()) return;
            }

            if (currentStepNumber == 5)
            {
                if (!ValidateStep5()) return;
            }

            if (currentStepNumber == steps.Count)
            {
                try
                {
                    Console.WriteLine("NextStep: iniciando SaveFormData...");
                    await SaveFormData();
                    Console.WriteLine("NextStep: SaveFormData finalizado.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"NextStep: excepción en SaveFormData: {ex.Message}");
                }
                return;
            }

            if (currentStepNumber < steps.Count)
            {
                currentStepNumber++;

                if (currentStepNumber == 2 && registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.No)
                {
                    currentStepNumber = 3;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"NextStep: excepción general: {ex.Message}");
        }
        finally
        {
            isProcessing = false;
            StateHasChanged();
        }
    }

    // Métodos corregidos para usar los modelos correctos
    public void DiagnosticoCheckboxChanged(ChangeEventArgs e, int categoryId)
    {
        bool isChecked = e.Value?.ToString()?.ToLower() == "true" || e.Value?.ToString() == "on";
        if (isChecked)
        {
            if (!registro.Diagnostico.SelectedDiagnosticosIds.Contains(categoryId))
                registro.Diagnostico.SelectedDiagnosticosIds.Add(categoryId);
        }
        else
        {
            registro.Diagnostico.SelectedDiagnosticosIds.Remove(categoryId);
        }
    }

    public void FormaCheckboxChanged(ChangeEventArgs e, int categoryId)
    {
        bool isChecked = e.Value?.ToString()?.ToLower() == "true" || e.Value?.ToString() == "on";
        if (isChecked)
        {
            if (!registro.Producto.SelectedFormaIds.Contains(categoryId))
                registro.Producto.SelectedFormaIds.Add(categoryId);
        }
        else
        {
            registro.Producto.SelectedFormaIds.Remove(categoryId);
        }
    }

    public void ViaAdmCheckboxChanged(ChangeEventArgs e, int categoryId)
    {
        bool isChecked = e.Value?.ToString()?.ToLower() == "true" || e.Value?.ToString() == "on";
        if (isChecked)
        {
            if (!registro.Producto.SelectedViaAdmIds.Contains(categoryId))
                registro.Producto.SelectedViaAdmIds.Add(categoryId);
        }
        else
        {
            registro.Producto.SelectedViaAdmIds.Remove(categoryId);
        }
    }

    private bool ValidateStep1()
    {
        messageStore?.Clear();
        var subModel = registro.Paciente;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        foreach (var r in results)
        {
            if (r.MemberNames != null && r.MemberNames.Any())
            {
                foreach (var member in r.MemberNames)
                {
                    messageStore?.Add(new FieldIdentifier(subModel, member), r.ErrorMessage);
                }
            }
            else
            {
                messageStore?.Add(new FieldIdentifier(subModel, string.Empty), r.ErrorMessage);
            }
        }

        if (registro.Paciente.InstalacionSaludId == null && !mostrarOtraInstalacionPaciente)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.InstalacionSaludId)),
                "Seleccione o especifique una instalación de salud donde es atendido.");
            isValid = false;
        }

        if (mostrarOtraInstalacionPaciente && string.IsNullOrEmpty(registro.Paciente.InstalacionSaludPersonalizada))
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.InstalacionSaludPersonalizada)),
                "Especifique el nombre de la instalación de salud donde es atendido.");
            isValid = false;
        }

        if (registro.Paciente.ProvinciaId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.ProvinciaId)),
                "La provincia es requerida.");
            isValid = false;
        }

        if (registro.Paciente.DistritoId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.DistritoId)),
                "El distrito es requerido.");
            isValid = false;
        }

        if (registro.Paciente.RegionSaludId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.RegionSaludId)),
                "La región de salud es requerida.");
            isValid = false;
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private bool ValidateStep2()
    {
        if (registro.Paciente.RequiereAcompanante != EnumViewModel.RequiereAcompanante.Si || registro.Acompanante == null)
            return true;

        messageStore?.Clear();
        var subModel = registro.Acompanante;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        foreach (var r in results)
        {
            if (r.MemberNames != null && r.MemberNames.Any())
            {
                foreach (var member in r.MemberNames)
                {
                    messageStore?.Add(new FieldIdentifier(subModel, member), r.ErrorMessage);
                }
            }
            else
            {
                messageStore?.Add(new FieldIdentifier(subModel, string.Empty), r.ErrorMessage);
            }
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private bool ValidateStep3()
    {
        messageStore?.Clear();

        var subModel = registro.Medico;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        foreach (var r in results)
        {
            if (r.MemberNames != null && r.MemberNames.Any())
            {
                foreach (var member in r.MemberNames)
                {
                    messageStore?.Add(new FieldIdentifier(subModel, member), r.ErrorMessage);
                }
            }
            else
            {
                messageStore?.Add(new FieldIdentifier(subModel, string.Empty), r.ErrorMessage);
            }
        }

        messageStore?.Clear(new FieldIdentifier(subModel, nameof(subModel.InstalacionSaludId)));

        if (mostrarOtraInstalacionMedico)
        {
            if (string.IsNullOrWhiteSpace(registro.Medico.InstalacionSaludPersonalizada))
            {
                messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.InstalacionSaludPersonalizada)),
                    "Especifique el nombre de la instalación de salud.");
                isValid = false;
            }
        }
        else
        {
            if (subModel.InstalacionSaludId == null)
            {
                messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.InstalacionSaludId)),
                    "Seleccione una instalación de salud de la lista.");
                isValid = false;
            }
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private bool ValidateStep4()
    {
        messageStore?.Clear();
        var subModel = registro.Producto;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        foreach (var r in results)
        {
            if (r.MemberNames != null && r.MemberNames.Any())
            {
                foreach (var member in r.MemberNames)
                {
                    messageStore?.Add(new FieldIdentifier(subModel, member), r.ErrorMessage);
                }
            }
        }

        if (registro.Producto.ProductoUnidadId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.ProductoUnidadId)),
                "Seleccione una unidad.");
            isValid = false;
        }

        if (registro.Producto.SelectedFormaIds.Count == 0 && string.IsNullOrEmpty(registro.Producto.NombreOtraForma))
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.SelectedFormaIds)),
                "Seleccione o especifique una forma farmacéutica.");
            isValid = false;
        }

        if (registro.Producto.SelectedViaAdmIds.Count == 0 && string.IsNullOrEmpty(registro.Producto.NombreOtraViaAdm))
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.SelectedViaAdmIds)),
                "Seleccione o especifique una vía de administración.");
            isValid = false;
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private bool ValidateStep5()
    {
        messageStore?.Clear();
        var subModel = registro.Diagnostico;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        foreach (var r in results)
        {
            if (r.MemberNames != null && r.MemberNames.Any())
            {
                foreach (var member in r.MemberNames)
                {
                    messageStore?.Add(new FieldIdentifier(subModel, member), r.ErrorMessage);
                }
            }
            else
            {
                messageStore?.Add(new FieldIdentifier(subModel, string.Empty), r.ErrorMessage);
            }
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private async Task SaveAttachedFiles(int solicitudId)
    {
        try
        {
            var archivosAGuardar = new List<TbDocumentoAdjunto>();

            async Task ProcessFiles(List<IBrowserFile> files, string tipoDocumentoNombre)
            {
                if (files == null || !files.Any()) return;

                var tipoDocumentoEntry = tipoDocumentoMap.FirstOrDefault(x =>
                    x.Key.Equals(tipoDocumentoNombre, StringComparison.OrdinalIgnoreCase));

                if (tipoDocumentoEntry.Value == 0)
                {
                    Console.WriteLine($"Tipo de documento no encontrado: {tipoDocumentoNombre}");
                    return;
                }

                foreach (var file in files)
                {
                    try
                    {
                        var rutaAlmacenada = await SaveFileAsync(file, solicitudId);
                        var nombreGuardado = Path.GetFileName(rutaAlmacenada);

                        var documento = new TbDocumentoAdjunto
                        {
                            SolRegCannabisId = solicitudId,
                            TipoDocumentoId = tipoDocumentoEntry.Value,
                            NombreOriginal = file.Name,
                            NombreGuardado = nombreGuardado,
                            Url = rutaAlmacenada,
                            FechaSubidaUtc = DateTime.UtcNow,
                            SubidoPor = "Sistema",
                            IsValido = false
                        };

                        archivosAGuardar.Add(documento);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar archivo {file.Name}: {ex.Message}");
                    }
                }
            }

            await ProcessFiles(documentos.CedulaPaciente, "CedulaPaciente");
            await ProcessFiles(documentos.CertificacionMedica, "CertificacionMedica");
            await ProcessFiles(documentos.FotoPaciente, "FotoPaciente");
            await ProcessFiles(documentos.CedulaAcompanante, "CedulaAcompanante");
            await ProcessFiles(documentos.SentenciaTutor, "SentenciaTutor");
            await ProcessFiles(documentos.Antecedentes, "Antecedentes");
            await ProcessFiles(documentos.IdentidadMenor, "IdentidadMenor");
            await ProcessFiles(documentos.ConsentimientoPadres, "ConsentimientoPadres");
            await ProcessFiles(documentos.CertificadoNacimientoMenor, "CertificadoNacimientoMenor");
            await ProcessFiles(documentos.FotoAcompanante, "FotoAcompanante");

            if (archivosAGuardar.Any())
            {
                await _context.TbDocumentoAdjunto.AddRangeAsync(archivosAGuardar);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Archivos guardados: {archivosAGuardar.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en SaveAttachedFiles: {ex.Message}");
            throw;
        }
    }

    private async Task<string> SaveFileAsync(IBrowserFile file, int solicitudId)
    {
        try
        {
            if (file.Size > 10 * 1024 * 1024)
                throw new Exception($"El archivo {file.Name} excede el tamaño máximo permitido (10MB)");

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads",
                solicitudId.ToString());

            if (!Directory.Exists(uploadDirectory))
                Directory.CreateDirectory(uploadDirectory);

            var fileExtension = Path.GetExtension(file.Name);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadDirectory, uniqueFileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(stream);

            return $"/uploads/{solicitudId}/{uniqueFileName}";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al guardar archivo {file.Name}: {ex.Message}");
            throw;
        }
    }

    private async Task SaveFormData()
    {
        try
        {
            if (!ValidateCompleteForm())
            {
                Console.WriteLine("Error en validación del formulario");
                return;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Procesar instalaciones personalizadas
                if (mostrarOtraInstalacionPaciente && !string.IsNullOrEmpty(registro.Paciente.InstalacionSaludPersonalizada))
                {
                    var instalacionId = await CrearOGuardarInstalacionPersonalizada(registro.Paciente.InstalacionSaludPersonalizada);
                    registro.Paciente.InstalacionSaludId = instalacionId;
                }

                if (mostrarOtraInstalacionMedico && !string.IsNullOrEmpty(registro.Medico.InstalacionSaludPersonalizada))
                {
                    var instalacionId = await CrearOGuardarInstalacionPersonalizada(registro.Medico.InstalacionSaludPersonalizada);
                    registro.Medico.InstalacionSaludId = instalacionId;
                }

                // 1. Guardar Paciente
                var paciente = new TbPaciente
                {
                    PrimerNombre = registro.Paciente.PrimerNombre ?? string.Empty,
                    SegundoNombre = registro.Paciente.SegundoNombre,
                    PrimerApellido = registro.Paciente.PrimerApellido ?? string.Empty,
                    SegundoApellido = registro.Paciente.SegundoApellido,
                    TipoDocumento = registro.Paciente.TipoDocumento.ToString(),
                    DocumentoCedula = registro.Paciente.TipoDocumento == EnumViewModel.TipoDocumento.Cedula ? registro.Paciente.NumeroDocumento : null,
                    DocumentoPasaporte = registro.Paciente.TipoDocumento == EnumViewModel.TipoDocumento.Pasaporte ? registro.Paciente.NumeroDocumento : null,
                    Nacionalidad = registro.Paciente.Nacionalidad ?? string.Empty,
                    FechaNacimiento = registro.Paciente.FechaNacimiento.HasValue
                        ? DateOnly.FromDateTime(registro.Paciente.FechaNacimiento.Value)
                        : null,
                    Sexo = registro.Paciente.Sexo.ToString(),
                    TelefonoPersonal = registro.Paciente.TelefonoPersonal,
                    TelefonoLaboral = registro.Paciente.TelefonoLaboral,
                    CorreoElectronico = registro.Paciente.CorreoElectronico ?? string.Empty,
                    ProvinciaId = registro.Paciente.ProvinciaId,
                    DistritoId = registro.Paciente.DistritoId,
                    CorregimientoId = registro.Paciente.CorregimientoId,
                    RegionId = registro.Paciente.RegionSaludId,
                    InstalacionId = registro.Paciente.InstalacionSaludId,
                    DireccionExacta = registro.Paciente.DireccionExacta ?? string.Empty,
                    RequiereAcompanante = registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si,
                    MotivoRequerimientoAcompanante = registro.Paciente.MotivoRequerimientoAcompanante?.ToString(),
                    TipoDiscapacidad = registro.Paciente.TipoDiscapacidad
                };

                _context.TbPaciente.Add(paciente);
                await _context.SaveChangesAsync();

                // 2. Guardar Acompañante si es necesario
                if (registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si && registro.Acompanante != null)
                {
                    var acompanante = new TbAcompanantePaciente
                    {
                        PacienteId = paciente.Id,
                        PrimerNombre = registro.Acompanante.PrimerNombre ?? string.Empty,
                        SegundoNombre = registro.Acompanante.SegundoNombre,
                        PrimerApellido = registro.Acompanante.PrimerApellido ?? string.Empty,
                        SegundoApellido = registro.Acompanante.SegundoApellido,
                        TipoDocumento = registro.Acompanante.TipoDocumento.ToString(),
                        NumeroDocumento = registro.Acompanante.NumeroDocumento ?? string.Empty,
                        Nacionalidad = registro.Acompanante.Nacionalidad ?? string.Empty,
                        Parentesco = registro.Acompanante.Parentesco.ToString(),
                        TelefonoMovil = registro.Acompanante.TelefonoMovil
                    };

                    _context.TbAcompanantePaciente.Add(acompanante);
                    await _context.SaveChangesAsync();
                }

                // 3. Guardar Médico
                var medico = new TbMedicoPaciente
                {
                    PrimerNombre = registro.Medico.PrimerNombre ?? string.Empty,
                    PrimerApellido = registro.Medico.PrimerApellido ?? string.Empty,
                    MedicoDisciplina = registro.Medico.MedicoDisciplina.ToString(),
                    MedicoIdoneidad = registro.Medico.MedicoIdoneidad ?? string.Empty,
                    MedicoTelefono = registro.Medico.TelefonoPersonal ?? string.Empty,
                    InstalacionId = registro.Medico.InstalacionSaludId,
                    RegionId = registro.Medico.RegionSaludId,
                    DetalleMedico = registro.Medico.DetalleEspecialidad ?? "Sin detalle",
                    PacienteId = paciente.Id
                };

                _context.TbMedicoPaciente.Add(medico);
                await _context.SaveChangesAsync();

                // 4. Guardar Diagnósticos del Paciente
                foreach (var diagnosticoId in registro.Diagnostico.SelectedDiagnosticosIds)
                {
                    var diagnosticoNombre = pacienteDiagnosticolist.FirstOrDefault(d => d.Id == diagnosticoId)?.Nombre;
                    if (!string.IsNullOrEmpty(diagnosticoNombre))
                    {
                        var diagnostico = new TbPacienteDiagnostico
                        {
                            PacienteId = paciente.Id,
                            NombreDiagnostico = diagnosticoNombre
                        };
                        _context.TbPacienteDiagnostico.Add(diagnostico);
                    }
                }

                if (!string.IsNullOrWhiteSpace(registro.Diagnostico.NombreOtroDiagnostico))
                {
                    var otroDiagnostico = new TbPacienteDiagnostico
                    {
                        PacienteId = paciente.Id,
                        NombreDiagnostico = registro.Diagnostico.NombreOtroDiagnostico
                    };
                    _context.TbPacienteDiagnostico.Add(otroDiagnostico);
                }

                await _context.SaveChangesAsync();

                // 5. Guardar Comorbilidades si existen
                if (registro.Comorbilidad.TieneComorbilidadEnum == EnumViewModel.TieneComorbilidad.Si && 
                    !string.IsNullOrWhiteSpace(registro.Comorbilidad.NombreDiagnostico))
                {
                    var comorbilidad = new TbPacienteComorbilidad
                    {
                        PacienteId = paciente.Id,
                        NombreDiagnostico = registro.Comorbilidad.NombreDiagnostico,
                        DetalleTratamiento = registro.Comorbilidad.DetalleTratamiento
                    };
                    _context.TbPacienteComorbilidad.Add(comorbilidad);
                    await _context.SaveChangesAsync();
                }

                // 6. Guardar Producto del Paciente
                // Obtener la primera forma seleccionada o usar la personalizada
                var formaFarmaceutica = registro.Producto.SelectedFormaIds.Count > 0 ?
                    productoFormaList.FirstOrDefault(f => f.Id == registro.Producto.SelectedFormaIds.First())?.Nombre :
                    registro.Producto.NombreOtraForma;

                // Obtener la primera vía de administración seleccionada o usar la personalizada
                var viaAdministracion = registro.Producto.SelectedViaAdmIds.Count > 0 ?
                    productoViaConsumoList.FirstOrDefault(v => v.Id == registro.Producto.SelectedViaAdmIds.First())?.Nombre :
                    registro.Producto.NombreOtraViaAdm;

                var unidad = productoUnidadList.FirstOrDefault(u => u.Id == registro.Producto.ProductoUnidadId)?.Name;

                int? dosisFrecuencia = registro.Producto.DosisFrecuencia;
                int? dosisDuracion   = registro.Producto.DosisDuracion;

                var productoPaciente = new TbNombreProductoPaciente
                {
                    PacienteId = paciente.Id,
                    NombreProducto = registro.Producto.NombreProducto ?? string.Empty,
                    NombreComercialProd = registro.Producto.NombreComercialProd,
                    FormaFarmaceutica = formaFarmaceutica,
                    CantidadConcentracion = registro.Producto.CantidadConcentracion,
                    NombreConcentracion = registro.Producto.ConcentracionEnum == EnumViewModel.ConcentracionE.OTRO
                        ? registro.Producto.NombreConcentracion
                        : registro.Producto.ConcentracionEnum.ToString(),
                    ViaConsumoProducto = viaAdministracion,
                    ProductoUnidad = unidad,
                    ProductoUnidadId = registro.Producto.ProductoUnidadId,
                    DetDosisPaciente = registro.Producto.DetDosisPaciente,
                    DosisFrecuencia = dosisFrecuencia,
                    DosisDuracion = dosisDuracion,
                    UsaDosisRescate = registro.Producto.UsaDosisRescateEnum == EnumViewModel.UsaDosisRescate.Si,
                    DetDosisRescate = registro.Producto.UsaDosisRescateEnum == EnumViewModel.UsaDosisRescate.Si
                        ? registro.Producto.DetDosisRescate
                        : null
                };


                _context.TbNombreProductoPaciente.Add(productoPaciente);
                await _context.SaveChangesAsync();

                // 7. Crear Solicitud de Registro
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

                // Obtener el estado "Pendiente" de la base de datos
                var estadoPendiente = await _context.TbEstadoSolicitud
                    .FirstOrDefaultAsync(e => e.NombreEstado == "Pendiente");

                if (estadoPendiente == null)
                {
                    throw new InvalidOperationException("No se encontró el estado 'Pendiente' en la base de datos");
                }

                var solicitud = new TbSolRegCannabis
                {
                    PacienteId = paciente.Id,
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

                // 8. GUARDAR ARCHIVOS ADJUNTOS
                await SaveAttachedFiles(solicitud.Id);

                // 9. Crear historial
                var historial = new TbSolRegCannabisHistorial
                {
                    SolRegCannabisId = solicitud.Id,
                    EstadoSolicitudIdHistorial = estadoPendiente.IdEstado,
                    Comentario = "Solicitud creada",
                    UsuarioRevisor = "Sistema",
                    FechaCambio = DateOnly.FromDateTime(DateTime.Now)
                };

                _context.TbSolRegCannabisHistorial.Add(historial);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                Console.WriteLine($"Solicitud guardada exitosamente. Número: {solicitud.NumSolCompleta}");
                NavigationManager.NavigateTo("/", forceLoad: true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Error al guardar: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error general: {ex.Message}");
        }
    }

    private bool ValidateCompleteForm()
    {
        messageStore?.Clear();
        bool isValid = true;

        if (!ValidateStep1()) isValid = false;

        if (registro.Paciente.RequiereAcompanante == EnumViewModel.RequiereAcompanante.Si && !ValidateStep2())
        {
            isValid = false;
        }

        if (!ValidateStep3()) isValid = false;
        if (!ValidateStep4()) isValid = false;
        if (!ValidateStep5()) isValid = false;

        if (registro.Paciente.InstalacionSaludId == null && !mostrarOtraInstalacionPaciente)
        {
            messageStore?.Add(new FieldIdentifier(registro.Paciente, nameof(registro.Paciente.InstalacionSaludId)),
                "Seleccione o especifique una instalación de salud.");
            isValid = false;
        }

        if (mostrarOtraInstalacionPaciente && string.IsNullOrEmpty(registro.Paciente.InstalacionSaludPersonalizada))
        {
            messageStore?.Add(new FieldIdentifier(registro.Paciente, nameof(registro.Paciente.InstalacionSaludPersonalizada)),
                "Especifique el nombre de la instalación de salud.");
            isValid = false;
        }

        if (registro.Medico.InstalacionSaludId == null && !mostrarOtraInstalacionMedico)
        {
            messageStore?.Add(new FieldIdentifier(registro.Medico, nameof(registro.Medico.InstalacionSaludId)),
                "Seleccione o especifique una instalación de salud.");
            isValid = false;
        }

        if (mostrarOtraInstalacionMedico && string.IsNullOrEmpty(registro.Medico.InstalacionSaludPersonalizada))
        {
            messageStore?.Add(new FieldIdentifier(registro.Medico, nameof(registro.Medico.InstalacionSaludPersonalizada)),
                "Especifique el nombre de la instalación de salud.");
            isValid = false;
        }

        if (registro.Diagnostico.SelectedDiagnosticosIds.Count == 0 &&
            string.IsNullOrWhiteSpace(registro.Diagnostico.NombreOtroDiagnostico))
        {
            messageStore?.Add(
                new FieldIdentifier(registro.Diagnostico, nameof(registro.Diagnostico.SelectedDiagnosticosIds)),
                "Seleccione al menos un diagnóstico o especifique uno personalizado.");
            isValid = false;
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private void OnFileChange(InputFileChangeEventArgs e, string campo)
    {
        var files = e.GetMultipleFiles();
        const long maxFileSize = 5 * 1024 * 1024; // 5MB

        foreach (var file in files)
        {
            Console.WriteLine($"Archivo recibido para {campo}: {file.Name} - Tamaño: {file.Size} bytes");

            // Validar tamaño máximo
            if (file.Size > maxFileSize)
            {
                // Mostrar advertencia al usuario
                MostrarAdvertencia($"El archivo '{file.Name}' excede el tamaño máximo permitido de 5MB. " +
                                   $"Tamaño actual: {(file.Size / 1024f / 1024f):F2}MB");
                continue; // Saltar este archivo
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
                default:
                    Console.WriteLine($"Campo de archivo desconocido: {campo}");
                    break;
            }
        }

        StateHasChanged();
    }

    private async void MostrarAdvertencia(string mensaje)
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

    private void OnSubmit()
    {
        ModalForm.ShowSuccess("Formulario enviado exitosamente.");
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

    public class DocumentosModel
    {
        public List<IBrowserFile> CedulaPaciente { get; set; } = new();
        public List<IBrowserFile> CertificacionMedica { get; set; } = new();
        public List<IBrowserFile> FotoPaciente { get; set; } = new();
        public List<IBrowserFile> CedulaAcompanante { get; set; } = new();
        public List<IBrowserFile> SentenciaTutor { get; set; } = new();
        public List<IBrowserFile> Antecedentes { get; set; } = new();
        public List<IBrowserFile> IdentidadMenor { get; set; } = new();
        public List<IBrowserFile> ConsentimientoPadres { get; set; } = new();
        public List<IBrowserFile> CertificadoNacimientoMenor { get; set; } = new();
        public List<IBrowserFile> FotoAcompanante { get; set; } = new();
    }
}