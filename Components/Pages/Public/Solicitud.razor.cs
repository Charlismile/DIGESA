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
    
    private RegistroCanabisUnionModel registro { get; set; } = new();
    private List<ListModel> pacienteRegioneslist { get; set; } = new();
    private List<ListModel> pacienteProvincicaslist { get; set; } = new();
    private List<ListModel> pacienteDistritolist { get; set; } = new();
    private List<ListModel> pacienteCorregimientolist { get; set; } = new();
    private List<PacienteDiagnosticoModel> pacienteDiagnosticolist { get; set; } = new();
    private List<ProductoPacienteModel> productoFormaList { get; set; } = new();
    private List<ProductoPacienteModel> productoUnidadList { get; set; } = new();
    private List<ProductoPacienteModel> productoViaConsumoList { get; set; } = new();
    
    private EditContext editContext;

    #endregion
    
    protected override async Task OnInitializedAsync()
    {
        editContext = new EditContext(registro);
        
        pacienteProvincicaslist = await _Commonservice.GetProvincias();
        await CargarDiagnosticoList();
        await CargarFormaList();
        await CargarUnidadList();
        await CargarViaConsumoList();
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
    
    private async Task CargarViaConsumoList()
    {
        // 1) Traes los diagnósticos activos
        var raw = await _context.TbViaAdministracion
            .Where(x => x.IsActivo)
            .ToListAsync();

        // 2) Proyectas a tu modelo y ordenas alfabéticamente
        productoViaConsumoList = raw
            .Select(x => new ProductoPacienteModel() {
                Id = x.Id,
                ViaConsumoProducto = x.Nombre!,
                IsSelectedViaConsumo = false
            })
            .OrderBy(d => d.ViaConsumoProducto)    // ← aquí ordenas
            .ToList();

        // 3) Agregas “Otro” al final
        productoViaConsumoList.Add(new ProductoPacienteModel() {
            Id = 0,
            ViaConsumoProducto = string.Empty,
            IsSelectedViaConsumo = false
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

    private void NextStep()
    {
        if (currentStepNumber < steps.Count)
        {
            currentStepNumber++;
        }
        else
        {
            // Handle finish action
            // Navigate to completion page or generate document
        }
    }

    private void PreviousStep()
    {
        if (currentStepNumber > 1)
        {
            currentStepNumber--;
        }
    }
}