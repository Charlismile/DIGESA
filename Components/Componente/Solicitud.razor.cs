using BlazorBootstrap;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Components.Componente;

public partial class Solicitud : ComponentBase
{
    [Inject] private ICommon _Commonservice { get; set; } = default!;
    
    [Inject] private DbContextDigesa _context { get; set; } = default!;

    #region variables

    private string instalacionFilterPaciente = "";
    private string instalacionFilterMedico = "";

    private bool tieneComorbilidad = false;
    private PacienteModel paciente { get; set; } = new();
    private AcompañanteModel acompañante { get; set; } = new();
    
    private MedicoModel medico { get; set; } = new();
    private ProductoPacienteModel productoPaciente { get; set; } = new();
    
    private PacienteComorbilidadModel pacienteComorbilidad { get; set; } = new();
    private List<ListModel> pacienteRegioneslist { get; set; } = new();
    private List<ListModel> pacienteProvincicaslist { get; set; } = new();
    private List<ListModel> pacienteDistritolist { get; set; } = new();
    private List<ListModel> pacienteCorregimientolist { get; set; } = new();
    private List<PacienteDiagnosticoModel> pacienteDiagnosticolist { get; set; } = new();
    private List<ProductoPacienteModel> productoFormaList { get; set; } = new();
    


    #endregion
    
    protected override async Task OnInitializedAsync()
    {
        pacienteProvincicaslist = await _Commonservice.GetProvincias();
        await CargarDiagnosticoList();
        await CargarFormaList();
    }

    private async Task CargarDiagnosticoList()
    {
        // 1) Traes los diagnósticos activos
        var raw = await _context.ListaDiagnostico
            .Where(x => x.IsActivo)
            .ToListAsync();

        // 2) Proyectas a tu modelo y ordenas alfabéticamente
        pacienteDiagnosticolist = raw
            .Select(x => new PacienteDiagnosticoModel {
                Id = x.Id,
                NombreDiagnostico = x.Nombre!,
                IsSelected = false
            })
            .OrderBy(d => d.NombreDiagnostico)    // ← aquí ordenas
            .ToList();

        // 3) Agregas “Otro” al final
        pacienteDiagnosticolist.Add(new PacienteDiagnosticoModel {
            Id = 0,
            NombreDiagnostico = string.Empty,
            IsSelected = false
        });
    }
    
    private async Task CargarFormaList()
    {
        // 1) Traes los diagnósticos activos
        var raw = await _context.TbFormaFarmaceutica
            .Where(x => x.IsActivo)
            .ToListAsync();
    
        // 2) Proyectas a tu modelo y ordenas alfabéticamente
        productoFormaList = raw
            .Select(x => new ProductoPacienteModel() {
                Id = x.Id,
                NombreForma = x.Nombre!,
                IsSelectedForma = false
            })
            .OrderBy(d => d.NombreForma)   
            .ToList();
    
        // 3) Agregas “Otro” al final
        productoFormaList.Add(new ProductoPacienteModel() {
            Id = 0,
            NombreForma = string.Empty,
            IsSelectedForma = false
        });
    }


    private async Task<AutoCompleteDataProviderResult<ListModel>> AutoCompleteDataProvider(
        AutoCompleteDataProviderRequest<ListModel> request)
    {
       
        var filtro = (request.Filter?.Value?.ToString() ?? "").ToUpper();

        var lista = await _Commonservice.GetInstalaciones(filtro);

     
        var filtradas = lista.Select(x => new ListModel {
            Id   = x.Id,
            Name = x.Name.Trim()
        });

        return new AutoCompleteDataProviderResult<ListModel> {
            Data       = filtradas,
            TotalCount = filtradas.Count()
        };
    }

    private void OnAutoCompletePacienteChanged(ListModel? sel)
    {
        paciente.pacienteInstalacionId = sel?.Id;
    }

    private void OnAutoCompleteMedicoChanged(ListModel? sel)
    {
        medico.medicoInstalacionId = sel?.Id;
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