using System.ComponentModel.DataAnnotations;
using BlazorBootstrap;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Components.Pages.Public;

public partial class Solicitud : ComponentBase
{
    [Inject] private ICommon _Commonservice { get; set; } = default!;

    [Inject] private DbContextDigesa _context { get; set; } = default!;

    #region variables

    private string tipoTramite;
    private DocumentosModel documentos = new();
    
    private string instalacionFilterPaciente = "";
    private string instalacionFilterMedico = "";
    private int? unidadSeleccionadaId { get; set; }

    [Required(ErrorMessage = "Debe especificar la unidad si seleccionó 'Otro'.")]
    private string? unidadOtraTexto { get; set; }

    private ValidationMessageStore? messageStore;

    private RegistroCanabisUnionModel registro { get; set; } = new();
    private List<ListModel> pacienteRegioneslist { get; set; } = new();
    private List<ListModel> pacienteProvincicaslist { get; set; } = new();
    private List<ListModel> pacienteDistritolist { get; set; } = new();
    private List<ListModel> pacienteCorregimientolist { get; set; } = new();
    private List<ListaDiagnostico> pacienteDiagnosticolist { get; set; } = new();
    private List<TbFormaFarmaceutica> productoFormaList { get; set; } = new();
    private List<ProductoPacienteModel> productoUnidadList { get; set; } = new();
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

        await CargarUnidadList();
    }

    private async Task CargarUnidadList()
    {
        var raw = await _context.TbUnidades
            .Where(x => x.IsActivo)
            .ToListAsync();

        productoUnidadList = raw
            .Select(x => new ProductoPacienteModel
            {
                Id = x.Id,
                ProductoUnidad = x.NombreUnidad!,
                IsSelectedUnidad = false
            })
            .OrderBy(d => d.ProductoUnidad)
            .ToList();

        // Agrega la opción "Otro"
        productoUnidadList.Add(new ProductoPacienteModel
        {
            Id = 0,
            ProductoUnidad = string.Empty,
            IsSelectedUnidad = false
        });
    }

    // Ejemplo de cómo obtener el valor final



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
        registro.paciente.pacienteInstalacionId = sel?.Id;
    }

    private void OnAutoCompleteMedicoChanged(ListModel? sel)
    {
        registro.medico.medicoInstalacionId = sel?.Id;
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

    private async Task RegisterForm()
    {
    }

    private int currentStepNumber = 1; // Starting at step 3 to match the image
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
        // Validaciones por paso
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
        if (currentStepNumber == 5)
        {
            if (!ValidateStep5()) return;
        }

        // Si es el último paso, guardar en BD
        if (currentStepNumber == steps.Count)
        {
            await SaveFormData();
            return;
        }

        // Avanzar al siguiente paso
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
        // Borra errores previos relacionados
        messageStore?.Clear();

        var subModel = registro.paciente;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        // Ejecuta DataAnnotations + IValidatableObject
        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        // Reportar errores al EditContext
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
                // error: modelo general
                messageStore?.Add(new FieldIdentifier(subModel, string.Empty), r.ErrorMessage);
            }
        }

        // Validaciones UI-only (no están en el modelo):
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

        // Instalación (autocomplete) — validamos que se haya seleccionado una opción
        if (registro.paciente.pacienteInstalacionId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.pacienteInstalacionId)),
                "Seleccione la instalación de salud donde es atendido.");
            isValid = false;
        }

        // Notificar a Blazor que los mensajes cambiaron
        editContext?.NotifyValidationStateChanged();

        return isValid;
    }
    
    private bool ValidateStep2()
    {
        // Borra errores previos relacionados
        messageStore?.Clear();

        var subModel = registro.acompanante;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        // Ejecuta DataAnnotations + IValidatableObject
        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        // Reportar errores al EditContext
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
                // error: modelo general
                messageStore?.Add(new FieldIdentifier(subModel, string.Empty), r.ErrorMessage);
            }
        }

        // Notificar a Blazor que los mensajes cambiaron
        editContext?.NotifyValidationStateChanged();

        return isValid;
    }
    
    private bool ValidateStep3()
    {
        // Borra errores previos relacionados
        messageStore?.Clear();

        var subModel = registro.medico;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        // Ejecuta DataAnnotations + IValidatableObject
        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        // Reportar errores al EditContext
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
                // error: modelo general
                messageStore?.Add(new FieldIdentifier(subModel, string.Empty), r.ErrorMessage);
            }
        }

        // Instalación (autocomplete) — validamos que se haya seleccionado una opción
        if (registro.paciente.pacienteInstalacionId == null)
        {
            messageStore?.Add(new FieldIdentifier(subModel, nameof(subModel.medicoInstalacionId)),
                "Seleccione la instalación de salud donde labora el medico.");
            isValid = false;
        }

        // Notificar a Blazor que los mensajes cambiaron
        editContext?.NotifyValidationStateChanged();

        return isValid;
    }
    
    private bool ValidateStep5()
    {
        // Borra errores previos relacionados
        messageStore?.Clear();

        var subModel = registro.pacienteComorbilidad;
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(subModel, serviceProvider: null, items: null);

        // Ejecuta DataAnnotations + IValidatableObject
        bool isValid = Validator.TryValidateObject(subModel, ctx, results, validateAllProperties: true);

        // Reportar errores al EditContext
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
                // error: modelo general
                messageStore?.Add(new FieldIdentifier(subModel, string.Empty), r.ErrorMessage);
            }
        }
        // Notificar a Blazor que los mensajes cambiaron
        editContext?.NotifyValidationStateChanged();

        return isValid;
    }
    
    private async Task SaveFormData()
{
    try
    {
        // Validar el formulario completo
        if (!ValidateCompleteForm())
        {
            // Mostrar mensaje de error
            return;
        }

        // Iniciar transacción
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Guardar Paciente
            var paciente = new TbPaciente
            {
                PrimerNombre = registro.paciente.PrimerNombre,
                SegundoNombre = registro.paciente.SegundoNombre,
                PrimerApellido = registro.paciente.PrimerApellido,
                SegundoApellido = registro.paciente.SegundoApellido,
                TipoDocumento = registro.paciente.TipoDocumentoPacienteEnum.ToString(),
                NumDocCedula = registro.paciente.NumDocCedula,
                NumDocPasaporte = registro.paciente.NumDocPasaporte,
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
                MotivoRequerimientoAcompanante = registro.paciente.MotivoRequerimientoAcompanante?.ToString(),
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
                    NumeroDocumento = registro.acompanante.TipoDocumentoAcompañanteEnum == TipoDocumentoAcompañante.Cedula 
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
                DetalleMedico = registro.medico.DetalleMedico
            };

            _context.TbMedicoPaciente.Add(medico);
            await _context.SaveChangesAsync();

            // 4. Guardar Diagnósticos del Paciente
            foreach (var diagnosticoId in registro.pacienteDiagnostico.SelectedDiagnosticosIds)
            {
                var diagnostico = new TbPacienteDiagnostico
                {
                    PacienteId = paciente.Id,
                    NombreDiagnostico = pacienteDiagnosticolist.FirstOrDefault(d => d.Id == diagnosticoId)?.Nombre
                };

                _context.TbPacienteDiagnostico.Add(diagnostico);
            }

            // Guardar diagnóstico "Otro" si existe
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

            // 5. Guardar Producto del Paciente
            var productoPaciente = new TbNombreProductoPaciente
            {
                PacienteId = paciente.Id,
                NombreProducto = registro.productoPaciente.NombreProductoEnum == NombreProductoE.OTRO 
                    ? registro.productoPaciente.NombreProducto 
                    : registro.productoPaciente.NombreProductoEnum.ToString(),
                NombreComercialProd = registro.productoPaciente.NombreComercialProd,
                CantidadConcentracion = registro.productoPaciente.CantidadConcentracion,
                NombreConcentracion = registro.productoPaciente.ConcentracionEnum == ConcentracionE.OTRO 
                    ? registro.productoPaciente.NombreConcentracion 
                    : registro.productoPaciente.ConcentracionEnum.ToString(),
                ProductoUnidad = unidadSeleccionadaId == 0 ? unidadOtraTexto : 
                    productoUnidadList.FirstOrDefault(u => u.Id == unidadSeleccionadaId)?.ProductoUnidad,
                DetDosisPaciente = registro.productoPaciente.DetDosisPaciente,
                DosisFrecuencia = registro.productoPaciente.DosisFrecuencia,
                DosisDuracion = registro.productoPaciente.DosisDuracion,
                UsaDosisRescate = registro.productoPaciente.UsaDosisRescateEnum == UsaDosisRescate.Si
            };

            _context.TbNombreProductoPaciente.Add(productoPaciente);
            await _context.SaveChangesAsync();

            // 6. Guardar Comorbilidades si existen
            if (registro.pacienteComorbilidad.TieneComorbilidadEnum == TieneComorbilidad.Si)
            {
                var comorbilidad = new TbPacienteComorbilidad
                {
                    NombreDiagnostico = registro.pacienteComorbilidad.NombreDiagnostico,
                    DetalleTratamiento = registro.pacienteComorbilidad.DetalleTratamiento
                };

                _context.TbPacienteComorbilidad.Add(comorbilidad);
                await _context.SaveChangesAsync();
            }

            // 7. Crear Solicitud de Registro
            var solicitud = new TbSolRegCannabis
            {
                PacienteId = paciente.Id,
                FechaSolicitud = DateTime.Now,
                EstadoSolicitud = "Pendiente",
                CreadaPor = "Sistema", // Aquí puedes poner el usuario actual si tienes autenticación
                NumSolAnio = DateTime.Now.Year,
                NumSolMes = DateTime.Now.Month
            };

            // Generar número de solicitud
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
            }

            solicitud.NumSolSecuencia = secuencia.Numeracion;
            solicitud.NumSolCompleta = $"{secuencia.Numeracion:0000}-{DateTime.Now.Year}";

            _context.TbSolRegCannabis.Add(solicitud);
            await _context.SaveChangesAsync();

            // Confirmar transacción
            await transaction.CommitAsync();

            // Mostrar mensaje de éxito
            Console.WriteLine("Solicitud guardada exitosamente. Número: " + solicitud.NumSolCompleta);
            
            // Aquí puedes redirigir a una página de éxito o mostrar un mensaje
            // NavigationManager.NavigateTo("/exito");

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"Error al guardar: {ex.Message}");
            // Mostrar mensaje de error al usuario
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error general: {ex.Message}");
    }
}
    private void OnFileChange(InputFileChangeEventArgs e, string campo)
    {
        var file = e.File;
        // Aquí podrías guardarlo en memoria o en base de datos
        Console.WriteLine($"Archivo recibido para {campo}: {file.Name}");
    }

    private void OnSubmit()
    {
        Console.WriteLine("Formulario enviado con éxito.");
    }

    private bool ValidateCompleteForm()
    {
        messageStore?.Clear();
        bool isValid = true;

        // Validar todos los pasos
        if (!ValidateStep1()) isValid = false;
    
        // Corregir esta línea
        bool requiereAcompanante = registro.paciente.RequiereAcompananteEnum == RequiereAcompanante.Si;
        if (requiereAcompanante && !ValidateStep2()) 
        {
            isValid = false;
        }
    
        if (!ValidateStep3()) isValid = false;
        if (!ValidateStep5()) isValid = false;

        // Validaciones adicionales para el último paso
        if (unidadSeleccionadaId == null)
        {
            messageStore?.Add(new FieldIdentifier(registro.productoPaciente, nameof(registro.productoPaciente.ProductoUnidad)),
                "Seleccione una unidad.");
            isValid = false;
        }

        if (unidadSeleccionadaId == 0 && string.IsNullOrEmpty(unidadOtraTexto))
        {
            messageStore?.Add(new FieldIdentifier(registro.productoPaciente, nameof(registro.productoPaciente.ProductoUnidad)),
                "Debe especificar la unidad.");
            isValid = false;
        }

        editContext?.NotifyValidationStateChanged();
        return isValid;
    }
    public class DocumentosModel
    {
        public IBrowserFile CedulaPaciente { get; set; }
        public IBrowserFile CertificacionMedica { get; set; }
        public IBrowserFile FotoPaciente { get; set; }
        public IBrowserFile CedulaAcompanante { get; set; }
        public IBrowserFile SentenciaTutor { get; set; }
        public IBrowserFile Antecedentes { get; set; }
        public IBrowserFile IdentidadMenor { get; set; }
        public IBrowserFile ConsentimientoPadres { get; set; }
        public IBrowserFile CertificadoNacimientoMenor { get; set; }
        public IBrowserFile FotoAcompanante { get; set; }
    }
}