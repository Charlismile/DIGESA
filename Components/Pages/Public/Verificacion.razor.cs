using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Renovaciones;
using DIGESA.Repositorios.Interfaces;
using DIGESA.Repositorios.InterfacesCannabis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DIGESA.Components.Pages.Public;

public partial class Verificacion : ComponentBase
{
    [Inject]
    public IPaciente PacienteService { get; set; } = default!;

    private string documentoBusqueda = string.Empty;
    private EstadoSolicitudViewModel? pacienteEstado;
    private PacienteViewModel? pacienteDetalle;
    private bool busquedaRealizada;
    private bool pacienteEncontrado;
    private bool loading;
    
    private DynamicModal ModalForm = default!;

    private async Task HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await BuscarPaciente();
        }
    }

    private async Task BuscarPaciente()
    {
        if (string.IsNullOrWhiteSpace(documentoBusqueda))
            return;

        loading = true;
        busquedaRealizada = false;
        pacienteEncontrado = false;
        pacienteEstado = null;
        pacienteDetalle = null;

        try
        {
            ModalForm.ShowInfo("Buscando paciente...");
            pacienteEstado = await PacienteService.GetEstadoPacienteAsync(documentoBusqueda);
            pacienteDetalle = await PacienteService.BuscarPorDocumentoAsync(documentoBusqueda);
            pacienteEncontrado = pacienteEstado is not null && !string.IsNullOrEmpty(pacienteEstado.Documento);
        }
        catch (Exception ex)
        {
            ModalForm.ShowError($"Error al buscar paciente. No encontrado.");
        }
        finally
        {
            loading = false;
            busquedaRealizada = true;
        }
    }

    // Métodos auxiliares para mostrar texto amigable
    private string GetNombreCompleto()
    {
        if (pacienteDetalle == null) return string.Empty;
        
        var nombres = new List<string> 
        { 
            pacienteDetalle.PrimerNombre ?? "",
            pacienteDetalle.SegundoNombre ?? "",
            pacienteDetalle.PrimerApellido ?? "",
            pacienteDetalle.SegundoApellido ?? ""
        };
        
        return string.Join(" ", nombres.Where(n => !string.IsNullOrEmpty(n)));
    }

    private string GetTipoDocumentoTexto()
    {
        return pacienteDetalle?.TipoDocumento switch
        {
            EnumViewModel.TipoDocumento.Cedula => "Cédula",
            EnumViewModel.TipoDocumento.Pasaporte => "Pasaporte",
            _ => "No especificado"
        };
    }

    private string GetSexoTexto()
    {
        return pacienteDetalle?.Sexo switch
        {
            EnumViewModel.Sexo.Masculino => "Masculino",
            EnumViewModel.Sexo.Femenino => "Femenino",
            _ => "No especificado"
        };
    }

    private string GetMotivoAcompananteTexto()
    {
        return pacienteDetalle?.MotivoRequerimientoAcompanante switch
        {
            EnumViewModel.MotivoRequerimientoAcompanante.PacienteMenorEdad => "Paciente menor de edad",
            EnumViewModel.MotivoRequerimientoAcompanante.PacienteDiscapacidad => "Paciente con discapacidad",
            _ => "No especificado"
        };
    }
}