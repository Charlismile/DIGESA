using BlazorBootstrap;
using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DIGESA.Components.Componente;

public partial class Solicitud : ComponentBase
{
    [Inject] private ICommon _Commonservice { get; set; } = default!;

    #region variables

    private string instalacionFilter = "";
    private PacienteModel paciente { get; set; } = new();

    private AcompañanteModel acompañante { get; set; } = new();

private List<ListModel> pacienteRegioneslist { get; set; } = new();
    private List<ListModel> pacienteProvincicaslist { get; set; } = new();
    private List<ListModel> pacienteDistritolist { get; set; } = new();
    private List<ListModel> pacienteCorregimientolist { get; set; } = new();


    #endregion
    
    protected override async Task OnInitializedAsync()
    {
        pacienteProvincicaslist = await _Commonservice.GetProvincias();
        pacienteRegioneslist = await _Commonservice.GetRegiones();
        
    }
    
    private async Task<AutoCompleteDataProviderResult<ListModel>> AutoCompleteDataProvider(
        AutoCompleteDataProviderRequest<ListModel> request)
    {
        // Obtengo el texto (en mayúsculas para búsqueda insensible a mayúsc/minúsc)
        var filtro = (request.Filter?.Value?.ToString() ?? "").ToUpper();

        // Llamo a tu servicio que ahora acepta filtro
        var lista = await _Commonservice.GetInstalaciones(filtro);

        // Proyección al ListModel que usa el AutoComplete
        var filtradas = lista.Select(x => new ListModel {
            Id   = x.Id,
            Name = x.Name.Trim()
        });

        return new AutoCompleteDataProviderResult<ListModel> {
            Data       = filtradas,
            TotalCount = filtradas.Count()
        };
    }

// Se dispara al seleccionar un ítem
    private void OnAutoCompleteChanged(ListModel? sel)
    {
        if (sel is null)
        {
            paciente.pacienteInstalacionId = null;
            return;
        }

        paciente.pacienteInstalacionId = sel.Id;
    }
    private async Task pacienteProvinciaChanged(int id)
    {
        paciente.pacienteProvinciaId = id;
        pacienteDistritolist = await _Commonservice.GetDistritos(id);
        pacienteCorregimientolist.Clear();
        paciente.pacienteCorregimientoId = null;
    }

    private async Task pacienteDistritoChanged(int id)
    {
        paciente.pacienteDistritoId = id;
        pacienteCorregimientolist = await _Commonservice.GetCorregimientos(id);
        paciente.pacienteCorregimientoId = null;
    }

    private async Task RegisterForm()
    {
    }
}