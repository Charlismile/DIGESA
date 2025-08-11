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

    private string instalacionFilterPaciente = "";
    private string instalacionFilterMedico = "";
    private bool tieneComorbilidad = false;
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
        "DATOS DE OTRAS ENFERMEDADES QUE PADECE EL PACIENTE"
    };

    private int currentStep => (int)Math.Round((double)currentStepNumber / steps.Count * 100);

    private void OnRequiereAcompananteChanged()
    {
        if (registro.paciente.RequiereAcompanante == RequiereAcompanante.No && currentStepNumber == 2)
        {
            currentStepNumber = 3;
        }
    }

    private void PreviousStep()
    {
        if (currentStepNumber > 1)
        {
            currentStepNumber--;

            if (currentStepNumber == 2 && registro.paciente.RequiereAcompanante == RequiereAcompanante.No)
            {
                currentStepNumber = 1;
            }
        }
    }

    private void NextStep()
    {
        // Si estamos en el step 1, validar completo
        if (currentStepNumber == 1)
        {
            if (!ValidateStep1())
            {
                // no avanzamos, los errores se muestran en UI
                return;
            }
        }

        // Si pasa la validación (o no es step1), avanzar como antes
        if (currentStepNumber < steps.Count)
        {
            currentStepNumber++;

            if (currentStepNumber == 2 && registro.paciente.RequiereAcompanante == RequiereAcompanante.No)
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
}