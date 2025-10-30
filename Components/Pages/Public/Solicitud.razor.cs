using System.ComponentModel.DataAnnotations;
using BlazorBootstrap;
using DIGESA.Models.CannabisModels;
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

    private string tipoTramite;
    private DocumentosModel documentos = new();
    private bool mostrarOtraInstalacionPaciente { get; set; }
    private bool mostrarOtraInstalacionMedico { get; set; }
    private Dictionary<string, int> tipoDocumentoMap = new Dictionary<string, int>();

    private string instalacionFilterPaciente = "";
    private string instalacionFilterMedico = "";

    [Required(ErrorMessage = "Debe especificar la unidad si seleccionó 'Otro'.")]
    private ValidationMessageStore? messageStore;

    private RegistroCanabisUnionModel registro { get; set; } = new();
    private List<ListModel> pacienteRegioneslist { get; set; } = new();
    private List<ListModel> pacienteProvincicaslist { get; set; } = new();
    private List<ListModel> pacienteDistritolist { get; set; } = new();
    private List<ListModel> pacienteCorregimientolist { get; set; } = new();
    private List<ListaDiagnostico> pacienteDiagnosticolist { get; set; } = new();
    private List<TbFormaFarmaceutica> productoFormaList { get; set; } = new();
    private List<ListModel> productoUnidadList { get; set; } = new();
    private List<TbViaAdministracion> productoViaConsumoList { get; set; } = new();

    private EditContext editContext;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        editContext = new EditContext(registro);
        messageStore = new ValidationMessageStore(editContext);

        pacienteProvincicaslist = await _Commonservice.GetProvincias();
        pacienteRegioneslist = await _Commonservice.GetRegiones();

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

            Console.WriteLine($"Tipos de documento cargados: {tipoDocumentoMap.Count}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar tipos de documento: {ex.Message}");
        }
    }

    private async Task<AutoCompleteDataProviderResult<ListModel>> AutoCompleteDataProvider(
        AutoCompleteDataProviderRequest<ListModel> request)
    {
        var filtro = (request.Filter?.Value?.ToString() ?? "").ToUpper();
        var lista = await _Commonservice.GetInstalaciones(filtro);

        var filtradas = lista.Select(x => new ListModel
        {
            Id = x.Id,
            Name = x.Name.Trim()
        });

        return new AutoCompleteDataProviderResult<ListModel>
        {
            Data = filtradas,
            TotalCount = filtradas.Count()
        };
    }

    private void OnAutoCompletePacienteChanged(ListModel? sel)
    {
        if (sel != null)
        {
            registro.paciente.pacienteInstalacionId = sel.Id;
            mostrarOtraInstalacionPaciente = false;
        }
    }

    private void OnAutoCompleteMedicoChanged(ListModel? sel)
    {
        if (sel != null)
        {
            registro.medico.medicoInstalacionId = sel.Id;
            mostrarOtraInstalacionMedico = false;
            registro.medico.MedicoInstalacion = null;
        }
    }

    private async Task pacienteProvinciaChanged(int id)
    {
        registro.paciente.pacienteProvinciaId = id;
        pacienteDistritolist = await _Commonservice.GetDistritos(id);
        pacienteCorregimientolist.Clear();
        registro.paciente.pacienteCorregimientoId = null;
    }

    private async Task pacienteDistritoChanged(int id)
    {
        registro.paciente.pacienteDistritoId = id;
        pacienteCorregimientolist = await _Commonservice.GetCorregimientos(id);
        registro.paciente.pacienteCorregimientoId = null;
    }

    private void OnUnidadSeleccionadaChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int selectedId))
        {
            registro.productoPaciente.ProductoUnidadId = selectedId;

            if (selectedId == 6)
            {
                registro.productoPaciente.ProductoUnidad = string.Empty;
            }
        }
        else
        {
            registro.productoPaciente.ProductoUnidadId = null;
        }
    }

    private void OnMostrarOtraInstalacionPacienteChanged(ChangeEventArgs e)
    {
        mostrarOtraInstalacionPaciente = (bool)e.Value;
        if (mostrarOtraInstalacionPaciente)
        {
            registro.paciente.pacienteInstalacionId = null;
            instalacionFilterPaciente = "";
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
            registro.medico.medicoInstalacionId = null;
            instalacionFilterMedico = string.Empty;
        }
        else
        {
            registro.medico.MedicoInstalacion = null;
        }

        messageStore?.Clear(new FieldIdentifier(registro.medico, nameof(registro.medico.medicoInstalacionId)));
        messageStore?.Clear(new FieldIdentifier(registro.medico, nameof(registro.medico.MedicoInstalacion)));

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
        if (registro.paciente.RequiereAcompananteEnum == RequiereAcompanante.No && currentStepNumber == 2)
        {
            currentStepNumber = 3;
        }
    }

    private void PreviousStep()
    {
        if (currentStepNumber > 1)
        {
            currentStepNumber--;

            if (currentStepNumber == 2 && registro.paciente.RequiereAcompananteEnum == RequiereAcompanante.No)
            {
                currentStepNumber = 1;
            }
        }
    }

    private async Task NextStep()
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
            await SaveFormData();
            return;
        }

        if (currentStepNumber < steps.Count)
        {
            currentStepNumber++;

            if (currentStepNumber == 2 && registro.paciente.RequiereAcompananteEnum == RequiereAcompanante.No)
            {
                currentStepNumber = 3;
            }
        }
    }

    public void DiagnosticoCheckboxChanged(ChangeEventArgs e, PacienteDiagnosticoModel block, int categoryId)
    {
        bool isChecked = e.Value?.ToString()?.ToLower() == "true" || e.Value?.ToString() == "on";
        if (isChecked)
        {
            if (!block.SelectedDiagnosticosIds.Contains(categoryId))
                block.SelectedDiagnosticosIds.Add(categoryId);
        }
        else
        {
            block.SelectedDiagnosticosIds.Remove(categoryId);
        }
    }

    public void FormaCheckboxChanged(ChangeEventArgs e, ProductoPacienteModel block, int categoryId)
    {
        bool isChecked = e.Value?.ToString()?.ToLower() == "true" || e.Value?.ToString() == "on";
        if (isChecked)
        {
            if (!block.SelectedFormaIds.Contains(categoryId))
                block.SelectedFormaIds.Add(categoryId);
        }
        else
        {
            block.SelectedFormaIds.Remove(categoryId);
        }
    }

    public void ViaAdmCheckboxChanged(ChangeEventArgs e, ProductoPacienteModel block, int categoryId)
    {
        bool isChecked = e.Value?.ToString()?.ToLower() == "true" || e.Value?.ToString() == "on";
        if (isChecked)
        {
            if (!block.SelectedViaAdmIds.Contains(categoryId))
                block.SelectedViaAdmIds.Add(categoryId);
        }
        else
        {
            block.SelectedViaAdmIds.Remove(categoryId);
        }
    }

    private bool ValidateStep1()
    {
        messageStore?.Clear();
        var subModel = registro.paciente;
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

        if (registro.paciente.pacienteInstalacionId == null && !mostrarOtraInstalacionPaciente)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.pacienteInstalacionId)),
                "Seleccione o especifique una instalación de salud donde es atendido.");
            isValid = false;
        }

        if (mostrarOtraInstalacionPaciente && string.IsNullOrEmpty(registro.paciente.pacienteInstalacion))
        {
            messageStore?.Add(new FieldIdentifier(this, nameof(registro.paciente.pacienteInstalacion)),
                "Especifique el nombre de la instalación de salud donde es atendido.");
            isValid = false;
        }

        if (registro.paciente.pacienteProvinciaId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.pacienteProvinciaId)),
                "La provincia es requerida.");
            isValid = false;
        }

        if (registro.paciente.pacienteDistritoId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.pacienteDistritoId)),
                "El distrito es requerido.");
            isValid = false;
        }

        if (registro.paciente.pacienteRegionId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.pacienteRegionId)),
                "La región de salud es requerida.");
            isValid = false;
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private bool ValidateStep2()
    {
        messageStore?.Clear();
        var subModel = registro.acompanante;
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

        var subModel = registro.medico;
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

        messageStore?.Clear(new FieldIdentifier(subModel, nameof(subModel.medicoInstalacionId)));
        messageStore?.Clear(new FieldIdentifier(subModel, nameof(subModel.MedicoInstalacion)));

        if (mostrarOtraInstalacionMedico)
        {
            if (string.IsNullOrWhiteSpace(subModel.MedicoInstalacion))
            {
                messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.MedicoInstalacion)),
                    "Especifique el nombre de la instalación de salud.");
                isValid = false;
            }
        }
        else
        {
            if (subModel.medicoInstalacionId == null)
            {
                messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.medicoInstalacionId)),
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
        var subModel = registro.productoPaciente;
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

        if (registro.productoPaciente.ProductoUnidadId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.ProductoUnidadId)),
                "Seleccione una unidad.");
            isValid = false;
        }

        if (registro.productoPaciente.ProductoUnidadId == 6 &&
            string.IsNullOrEmpty(registro.productoPaciente.ProductoUnidad))
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.ProductoUnidad)),
                "Debe especificar la unidad.");
            isValid = false;
        }

        if (registro.productoPaciente.SelectedFormaIds.Count == 0 &&
            !registro.productoPaciente.IsOtraFormaSelected)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.SelectedFormaIds)),
                "Seleccione al menos una forma farmacéutica.");
            isValid = false;
        }

        if (registro.productoPaciente.SelectedViaAdmIds.Count == 0 &&
            !registro.productoPaciente.IsOtraViaAdmSelected)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.SelectedViaAdmIds)),
                "Seleccione al menos una vía de administración.");
            isValid = false;
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private bool ValidateStep5()
    {
        messageStore?.Clear();
        var subModel = registro.pacienteComorbilidad;
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
                if (mostrarOtraInstalacionPaciente && !string.IsNullOrEmpty(registro.paciente.pacienteInstalacion))
                {
                    var instalacionId =
                        await CrearOGuardarInstalacionPersonalizada(registro.paciente.pacienteInstalacion);
                    registro.paciente.pacienteInstalacionId = instalacionId;
                }

                if (mostrarOtraInstalacionMedico && !string.IsNullOrEmpty(registro.medico.MedicoInstalacion))
                {
                    var instalacionId = await CrearOGuardarInstalacionPersonalizada(registro.medico.MedicoInstalacion);
                    registro.medico.medicoInstalacionId = instalacionId;
                }

                // 1. Guardar Paciente
                var paciente = new TbPaciente
                {
                    PrimerNombre = registro.paciente.PrimerNombre,
                    SegundoNombre = registro.paciente.SegundoNombre,
                    PrimerApellido = registro.paciente.PrimerApellido,
                    SegundoApellido = registro.paciente.SegundoApellido,
                    TipoDocumento = registro.paciente.TipoDocumentoPacienteEnum.ToString(),
                    DocumentoCedula = registro.paciente.NumDocCedula,
                    DocumentoPasaporte = registro.paciente.NumDocPasaporte,
                    Nacionalidad = registro.paciente.Nacionalidad,
                    FechaNacimiento = registro.paciente.FechaNacimiento.HasValue
                        ? DateOnly.FromDateTime(registro.paciente.FechaNacimiento.Value)
                        : null,
                    Sexo = registro.paciente.SexoEnum.ToString(),
                    TelefonoPersonal = registro.paciente.TelefonoResidencialPersonal,
                    TelefonoLaboral = registro.paciente.TelefonoLaboral,
                    CorreoElectronico = registro.paciente.CorreoElectronico,
                    ProvinciaId = registro.paciente.pacienteProvinciaId,
                    DistritoId = registro.paciente.pacienteDistritoId,
                    CorregimientoId = registro.paciente.pacienteCorregimientoId,
                    RegionId = registro.paciente.pacienteRegionId,
                    InstalacionId = registro.paciente.pacienteInstalacionId,
                    DireccionExacta = registro.paciente.DireccionExacta,
                    RequiereAcompanante = registro.paciente.RequiereAcompananteEnum == RequiereAcompanante.Si,
                    MotivoRequerimientoAcompanante = registro.paciente.MotivoRequerimientoAcompananteEnum?.ToString(),
                    TipoDiscapacidad = registro.paciente.TipoDiscapacidad
                };

                _context.TbPaciente.Add(paciente);
                await _context.SaveChangesAsync();

                // 2. Guardar Acompañante si es necesario
                if (registro.paciente.RequiereAcompananteEnum == RequiereAcompanante.Si)
                {
                    var acompanante = new TbAcompanantePaciente
                    {
                        PacienteId = paciente.Id,
                        PrimerNombre = registro.acompanante.PrimerNombre,
                        SegundoNombre = registro.acompanante.SegundoNombre,
                        PrimerApellido = registro.acompanante.PrimerApellido,
                        SegundoApellido = registro.acompanante.SegundoApellido,
                        TipoDocumento = registro.acompanante.TipoDocumentoAcompañanteEnum.ToString(),
                        NumeroDocumento = registro.acompanante.TipoDocumentoAcompañanteEnum ==
                                          TipoDocumentoAcompañante.Cedula
                            ? registro.acompanante.NumDocCedula
                            : registro.acompanante.NumDocPasaporte,
                        Nacionalidad = registro.acompanante.Nacionalidad,
                        Parentesco = registro.acompanante.ParentescoEnum?.ToString() ?? registro.acompanante.Parentesco,
                        TelefonoMovil = registro.acompanante.TelefonoPersonal
                    };

                    _context.TbAcompanantePaciente.Add(acompanante);
                    await _context.SaveChangesAsync();
                }

                // 3. Guardar Médico
                var medico = new TbMedicoPaciente
                {
                    PrimerNombre = registro.medico.PrimerNombre,
                    PrimerApellido = registro.medico.PrimerApellido,
                    MedicoDisciplina = registro.medico.MedicoDisciplinaEnum.ToString(),
                    MedicoIdoneidad = registro.medico.MedicoIdoneidad,
                    MedicoTelefono = registro.medico.MedicoTelefono,
                    InstalacionId = registro.medico.medicoInstalacionId,
                    RegionId = registro.medico.medicoRegionId,
                    DetalleMedico = registro.medico.DetalleMedico,
                    PacienteId = paciente.Id // Agregar relación con paciente
                };

                _context.TbMedicoPaciente.Add(medico);
                await _context.SaveChangesAsync();

                // 4. Guardar Diagnósticos del Paciente
                foreach (var diagnosticoId in registro.pacienteDiagnostico.SelectedDiagnosticosIds)
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

                if (registro.pacienteDiagnostico.IsOtroDiagSelected &&
                    !string.IsNullOrEmpty(registro.pacienteDiagnostico.NombreOtroDiagnostico))
                {
                    var otroDiagnostico = new TbPacienteDiagnostico
                    {
                        PacienteId = paciente.Id,
                        NombreDiagnostico = registro.pacienteDiagnostico.NombreOtroDiagnostico
                    };
                    _context.TbPacienteDiagnostico.Add(otroDiagnostico);
                }

                await _context.SaveChangesAsync();

                // 5. Guardar Formas Farmacéuticas
                var formasSeleccionadas = new List<string>();
                foreach (var formaId in registro.productoPaciente.SelectedFormaIds)
                {
                    var formaNombre = productoFormaList.FirstOrDefault(f => f.Id == formaId)?.Nombre;
                    if (!string.IsNullOrEmpty(formaNombre))
                    {
                        formasSeleccionadas.Add(formaNombre);
                    }
                }

                if (registro.productoPaciente.IsOtraFormaSelected &&
                    !string.IsNullOrEmpty(registro.productoPaciente.NombreOtraForma))
                {
                    formasSeleccionadas.Add(registro.productoPaciente.NombreOtraForma);
                }

                // 6. Guardar Vías de Administración
                var viasSeleccionadas = new List<string>();
                foreach (var viaId in registro.productoPaciente.SelectedViaAdmIds)
                {
                    var viaNombre = productoViaConsumoList.FirstOrDefault(v => v.Id == viaId)?.Nombre;
                    if (!string.IsNullOrEmpty(viaNombre))
                    {
                        viasSeleccionadas.Add(viaNombre);
                    }
                }

                if (registro.productoPaciente.IsOtraViaAdmSelected &&
                    !string.IsNullOrEmpty(registro.productoPaciente.NombreOtraViaAdm))
                {
                    viasSeleccionadas.Add(registro.productoPaciente.NombreOtraViaAdm);
                }

                // 7. Guardar Producto del Paciente
                var productoUnidadFinal = registro.productoPaciente.ProductoUnidadId == 6
                    ? registro.productoPaciente.ProductoUnidad
                    : productoUnidadList.FirstOrDefault(u => u.Id == registro.productoPaciente.ProductoUnidadId)?.Name;

                var productoPaciente = new TbNombreProductoPaciente
                {
                    PacienteId = paciente.Id,
                    NombreProducto = registro.productoPaciente.NombreProductoEnum == NombreProductoE.OTRO
                        ? registro.productoPaciente.NombreProducto
                        : registro.productoPaciente.NombreProductoEnum.ToString(),
                    NombreComercialProd = registro.productoPaciente.NombreComercialProd,
                    FormaFarmaceutica = string.Join(", ", formasSeleccionadas),
                    CantidadConcentracion = registro.productoPaciente.CantidadConcentracion,
                    NombreConcentracion = registro.productoPaciente.ConcentracionEnum == ConcentracionE.OTRO
                        ? registro.productoPaciente.NombreConcentracion
                        : registro.productoPaciente.ConcentracionEnum.ToString(),
                    ViaConsumoProducto = string.Join(", ", viasSeleccionadas),
                    ProductoUnidad = productoUnidadFinal,
                    ProductoUnidadId = registro.productoPaciente.ProductoUnidadId == 6
                        ? null
                        : registro.productoPaciente.ProductoUnidadId,
                    DetDosisPaciente = registro.productoPaciente.DetDosisPaciente,
                    DosisFrecuencia = registro.productoPaciente.DosisFrecuencia,
                    DosisDuracion = registro.productoPaciente.DosisDuracion,
                    UsaDosisRescate = registro.productoPaciente.UsaDosisRescateEnum == UsaDosisRescate.Si,
                    DetDosisRescate = registro.productoPaciente.UsaDosisRescateEnum == UsaDosisRescate.Si
                        ? registro.productoPaciente.DetDosisRescate
                        : null
                };

                _context.TbNombreProductoPaciente.Add(productoPaciente);
                await _context.SaveChangesAsync();

                // 8. Guardar Comorbilidades
                if (registro.pacienteComorbilidad.TieneComorbilidadEnum == TieneComorbilidad.Si &&
                    !string.IsNullOrEmpty(registro.pacienteComorbilidad.NombreDiagnostico))
                {
                    var comorbilidad = new TbPacienteComorbilidad
                    {
                        NombreDiagnostico = registro.pacienteComorbilidad.NombreDiagnostico,
                        DetalleTratamiento = registro.pacienteComorbilidad.DetalleTratamiento,
                        PacienteId = paciente.Id // Agregar relación con paciente
                    };

                    _context.TbPacienteComorbilidad.Add(comorbilidad);
                    await _context.SaveChangesAsync();
                }

                // 9. Crear Solicitud de Registro
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
                    EstadoSolicitudId = estadoPendiente.IdEstado, // CORREGIDO: Usar ID del estado
                    CreadaPor = "Sistema",
                    NumSolAnio = DateTime.Now.Year,
                    NumSolMes = DateTime.Now.Month,
                    NumSolSecuencia = secuencia.Numeracion,
                    NumSolCompleta = $"{secuencia.Numeracion:0000}-{DateTime.Now.Year}"
                };

                _context.TbSolRegCannabis.Add(solicitud);
                await _context.SaveChangesAsync();

                // 10. GUARDAR ARCHIVOS ADJUNTOS
                await SaveAttachedFiles(solicitud.Id);

                // 11. Crear historial
                var historial = new TbSolRegCannabisHistorial
                {
                    SolRegCannabisId = solicitud.Id,
                    EstadoSolicitudIdHistorial = estadoPendiente.IdEstado, // CORREGIDO: Usar ID del estado
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

        if (registro.paciente.RequiereAcompananteEnum == RequiereAcompanante.Si && !ValidateStep2())
        {
            isValid = false;
        }

        if (!ValidateStep3()) isValid = false;
        if (!ValidateStep4()) isValid = false;
        if (!ValidateStep5()) isValid = false;

        if (registro.productoPaciente.ProductoUnidadId == null)
        {
            messageStore?.Add(
                new FieldIdentifier(registro.productoPaciente, nameof(registro.productoPaciente.ProductoUnidadId)),
                "Seleccione una unidad.");
            isValid = false;
        }

        if (registro.productoPaciente.ProductoUnidadId == 6 &&
            string.IsNullOrEmpty(registro.productoPaciente.ProductoUnidad))
        {
            messageStore?.Add(
                new FieldIdentifier(registro.productoPaciente, nameof(registro.productoPaciente.ProductoUnidad)),
                "Debe especificar la unidad.");
            isValid = false;
        }

        if (registro.paciente.pacienteInstalacionId == null && !mostrarOtraInstalacionPaciente)
        {
            messageStore?.Add(new FieldIdentifier(registro.paciente, nameof(registro.paciente.pacienteInstalacionId)),
                "Seleccione o especifique una instalación de salud.");
            isValid = false;
        }

        if (mostrarOtraInstalacionPaciente && string.IsNullOrEmpty(registro.paciente.pacienteInstalacion))
        {
            messageStore?.Add(new FieldIdentifier(this, nameof(registro.paciente.pacienteInstalacion)),
                "Especifique el nombre de la instalación de salud.");
            isValid = false;
        }

        if (registro.medico.medicoInstalacionId == null && !mostrarOtraInstalacionMedico)
        {
            messageStore?.Add(new FieldIdentifier(registro.medico, nameof(registro.medico.medicoInstalacionId)),
                "Seleccione o especifique una instalación de salud.");
            isValid = false;
        }

        if (mostrarOtraInstalacionMedico && string.IsNullOrEmpty(registro.medico.MedicoInstalacion))
        {
            messageStore?.Add(new FieldIdentifier(this, nameof(registro.medico.MedicoInstalacion)),
                "Especifique el nombre de la instalación de salud.");
            isValid = false;
        }

        if (registro.pacienteDiagnostico.SelectedDiagnosticosIds.Count == 0 &&
            !registro.pacienteDiagnostico.IsOtroDiagSelected)
        {
            messageStore?.Add(
                new FieldIdentifier(registro.pacienteDiagnostico,
                    nameof(registro.pacienteDiagnostico.SelectedDiagnosticosIds)),
                "Seleccione al menos un diagnóstico.");
            isValid = false;
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }

    private void OnFileChange(InputFileChangeEventArgs e, string campo)
    {
        var files = e.GetMultipleFiles();
        const long maxFileSize = 5 * 1024 * 1024; // 512KB

        foreach (var file in files)
        {
            Console.WriteLine($"Archivo recibido para {campo}: {file.Name} - Tamaño: {file.Size} bytes");

            // Validar tamaño máximo
            if (file.Size > maxFileSize)
            {
                // Mostrar advertencia al usuario
                MostrarAdvertencia($"El archivo '{file.Name}' excede el tamaño máximo permitido de 5MB. " +
                                   $"Tamaño actual: {(file.Size / 2048f):F2}KB");
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
        // Puedes usar JavaScript para mostrar un alert o implementar un sistema de notificaciones
        try
        {
            // Si estás usando JavaScript interop
            await JSRuntime.InvokeVoidAsync("alert", mensaje);
        }
        catch
        {
            // Fallback: mostrar en consola
            Console.WriteLine($"ADVERTENCIA: {mensaje}");
        }
    }

    private void OnSubmit()
    {
        Console.WriteLine("Formulario enviado con éxito.");
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