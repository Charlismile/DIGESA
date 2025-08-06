using System.ComponentModel.DataAnnotations;
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
    
    private int? unidadSeleccionadaId { get; set; }
    
    [Required(ErrorMessage = "Debe especificar la unidad si seleccionó 'Otro'.")]
    private string? unidadOtraTexto { get; set; }
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
    
    private List<ProductoPacienteModel> productoUnidadList { get; set; } = new();
    
    private List<ProductoPacienteModel> productoViaConsumoList { get; set; } = new();
    


    #endregion
    
    protected override async Task OnInitializedAsync()
    {
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
                 IsSelectedConsumo = false
            })
            .OrderBy(d => d.ViaConsumoProducto)    // ← aquí ordenas
            .ToList();

        // 3) Agregas “Otro” al final
        productoViaConsumoList.Add(new ProductoPacienteModel() {
            Id = 0,
            ViaConsumoProducto = string.Empty,
            IsSelectedConsumo = false
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

    //     #region RENGLONES
    //
    // // VARIABLES
    // private ProductosModel RenglonForm { get; set; } = new();
    // private List<ProductosModel> Renglones { get; set; } = new();
    //
    // private ResultModel ResultRenglon { get; set; } = new ResultModel()
    // {
    //     Success = true,
    // };
    //
    // // private bool RenglonFormValid { get; set; } = true;
    // private bool RenglonEditing { get; set; } = false;
    //
    // // EVENTOS
    // private void InsertItemSubmit()
    // {
    //     ResultRenglon.Errores.Clear();
    //     ResultRenglon.Success = true;
    //     
    //     if (String.IsNullOrEmpty(RenglonForm.ProductoId))
    //     {
    //         ResultRenglon.Errores.Add("Debe ingresar la unidad.");
    //         ResultRenglon.Success = false;
    //     }
    //     
    //     if (String.IsNullOrEmpty(RenglonForm.NombreProducto))
    //     {
    //         ResultRenglon.Errores.Add("Debe ingresar la unidad.");
    //         ResultRenglon.Success = false;
    //     }
    //     if (String.IsNullOrEmpty(RenglonForm.NombreProductoEnum))
    //     {
    //         ResultRenglon.Errores.Add("Debe ingresar la descripción.");
    //         ResultRenglon.Success = false;
    //     }
    //
    //     if (RenglonForm.CantidadConcentracion == 0)
    //     {
    //         ResultRenglon.Errores.Add("Debe ingresar la cantidad.");
    //         ResultRenglon.Success = false;
    //     }
    //
    //     if (String.IsNullOrEmpty(RenglonForm.NombreProductoEnum))
    //     {
    //         ResultRenglon.Errores.Add("Debe ingresar la descripción.");
    //         ResultRenglon.Success = false;
    //     }
    //
    //     if (RenglonForm.PrecioUnitario == 0)
    //     {
    //         ResultRenglon.Errores.Add("Debe ingresar el precio unitario.");
    //         ResultRenglon.Success = false;
    //     }
    //
    //     if (ResultRenglon.Success)
    //     {
    //         if (RenglonForm.ITBMS)
    //         {
    //             decimal itbms = Convert.ToDecimal(0.07);
    //             RenglonForm.ValorSubTotal = (decimal)RenglonForm.Cantidad * RenglonForm.PrecioUnitario;
    //             RenglonForm.ValorImpuesto = RenglonForm.ValorSubTotal * itbms;
    //             RenglonForm.ValorTotal = RenglonForm.ValorSubTotal + RenglonForm.ValorImpuesto;
    //         }
    //         else
    //         {
    //             RenglonForm.ValorImpuesto = 0;
    //             RenglonForm.ValorSubTotal = (decimal)RenglonForm.Cantidad * RenglonForm.PrecioUnitario;
    //             RenglonForm.ValorTotal = RenglonForm.ValorSubTotal;
    //         }
    //
    //         RenglonForm.Codigo = RenglonForm.Codigo.ToUpper();
    //         RenglonForm.Unidad = RenglonForm.Unidad.ToUpper();
    //         RenglonForm.Descripcion = RenglonForm.Descripcion.ToUpper();
    //
    //         if (RenglonEditing)
    //         {
    //             int index = Renglones.FindIndex(x => x.TempId == RenglonForm.TempId);
    //             if (index != -1)
    //             {
    //                 Renglones[index].Cantidad = RenglonForm.Cantidad;
    //                 Renglones[index].Codigo = RenglonForm.Codigo;
    //                 Renglones[index].Unidad = RenglonForm.Unidad;
    //                 Renglones[index].Descripcion = RenglonForm.Descripcion;
    //                 Renglones[index].PrecioUnitario = RenglonForm.PrecioUnitario;
    //                 Renglones[index].ValorSubTotal = RenglonForm.ValorSubTotal;
    //                 Renglones[index].ValorImpuesto = RenglonForm.ValorImpuesto;
    //                 Renglones[index].ValorTotal = RenglonForm.ValorTotal;
    //                 Renglones[index].ITBMS = RenglonForm.ITBMS;
    //                 Renglones[index].UpdateRow = true;
    //                 Renglones[index].InsertRow = false;
    //                 Renglones[index].DeleteRow = false;
    //                 Renglones[index].ShowRow = true;
    //             }
    //
    //             RenglonEditing = false;
    //         }
    //         else
    //         {
    //             RenglonForm.UpdateRow = false;
    //             RenglonForm.InsertRow = true;
    //             RenglonForm.DeleteRow = false;
    //             RenglonForm.ShowRow = true;
    //             Renglones.Add(RenglonForm);
    //         }
    //
    //         RequisicionData.Items = Renglones;
    //         UpdateTotales();
    //         RenglonForm = new();
    //         ResultRenglon.Success = true;
    //     }
    // }
    //
    // private void EditRenglon(string Id)
    // {
    //     RenglonEditing = false;
    //     var data = Renglones.Where(x => x.TempId == Id).FirstOrDefault();
    //     if (data != null)
    //     {
    //         RenglonForm = new RequisicionItemsModel()
    //         {
    //             Id = data.Id,
    //             TempId = data.TempId,
    //             IdRequisicion = data.IdRequisicion,
    //             Cantidad = data.Cantidad,
    //             Codigo = data.Codigo,
    //             Unidad = data.Unidad,
    //             Descripcion = data.Descripcion,
    //             PrecioUnitario = data.PrecioUnitario,
    //             ValorImpuesto = data.ValorImpuesto,
    //             ValorSubTotal = data.ValorSubTotal,
    //             ValorTotal = data.ValorTotal,
    //             ITBMS = data.ITBMS,
    //             ShowRow = data.ShowRow,
    //             UpdateRow = data.UpdateRow,
    //             DeleteRow = data.DeleteRow,
    //             InsertRow = data.InsertRow,
    //         };
    //         RenglonEditing = true;
    //     }
    // }
    //
    // private void DeleteRenglon(string Id)
    // {
    //     var Renglon = Renglones.Where(x => x.TempId == Id).FirstOrDefault();
    //     Renglon.DeleteRow = true;
    //     Renglon.ShowRow = false;
    //     RequisicionData.Items = Renglones;
    //     UpdateTotales();
    // }
    //
    // private void CancelEditRenglon()
    // {
    //     RenglonForm = new();
    //     Renglones = RequisicionData.Items;
    //     RenglonEditing = false;
    // }
    //
    // #endregion
    //
    // #region GUARDAR REQUISICION
    //
    // private ResultModel RequisicionResult { get; set; } = new ResultModel()
    // {
    //     Success = true,
    // };
    //
    // private async Task CreateRequisicionSubmit()
    // {
    //     RequisicionResult.Success = true;
    //     RequisicionResult.Errores.Clear();
    //     if (Renglones == null || Renglones.Count == 0)
    //     {
    //         RequisicionResult.Success = false;
    //         RequisicionResult.Errores.Add("Debe ingresar los renglones.");
    //
    //         await ModalFormulario.ShowAsync();
    //         return;
    //     }
    //
    //     RequisicionData.Items = Renglones;
    //     int IdDirector = Convert.ToInt32(RequisicionData.IdDirector);
    //     RequisicionData.DirectorName = Directores
    //         .Where(x => x.Id == IdDirector)
    //         .Select(x => x.Name)
    //         .FirstOrDefault();
    //
    //     ResultModel Resultado = new ResultModel();
    //
    //     if (RequisicionData.Id == 0)
    //     {
    //         RequisicionData.IdUnidadEjecutora = RequisicionData.IdDireccion;
    //         Resultado = await _RequisicionService.SaveRequisicion(RequisicionData);
    //     }
    //     else
    //     {
    //         Resultado = await _RequisicionService.UpdateRequisicion(RequisicionData);
    //     }
    //
    //     _ToastService.Notify(new(Resultado.Success ? ToastType.Success : ToastType.Danger, "", $"Mensaje",
    //         $"{DateTime.Now}",
    //         Resultado.Message));
    //
    //     if (RequisicionData.EditMode == false)
    //     {
    //         NavigationProvider.NavigateTo("/requisiciones/index");
    //     }
    // }
    //
    // private async Task UpdateTotales()
    // {
    //     decimal impuesto = Convert.ToDecimal(0.07);
    //     decimal subtotal = 0;
    //     decimal total = 0;
    //     decimal impuestoTotal = 0;
    //
    //     foreach (var item in Renglones.Where(x => x.DeleteRow == false))
    //     {
    //         subtotal += item.ValorSubTotal;
    //         impuestoTotal += item.ValorImpuesto;
    //     }
    //
    //     total = subtotal + impuestoTotal;
    //     RequisicionData.PrecioITBMSRequisicion = impuestoTotal;
    //     RequisicionData.PrecioSubTotalRequisicion = subtotal;
    //     RequisicionData.PrecioTotalRequisicion = total;
    //     RequisicionData.TotalTexto = await _CommonServices.GetTotalTexto(RequisicionData.PrecioTotalRequisicion);
    // }
    //
    // #endregion
    private async Task RegisterForm()
    {
    }
}