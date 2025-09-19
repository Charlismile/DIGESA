using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DIGESA.Components.Pages.Public;

public partial class Verificacion : ComponentBase
{
    [Inject]
    public IPaciente PacienteService { get; set; } = default!;

    private string documentoBusqueda = string.Empty;
    private PacienteEstadoModel? pacienteEstado;
    private PacienteModel? pacienteDetalle;
    private bool busquedaRealizada;
    private bool pacienteEncontrado;
    private bool loading;

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
            pacienteEstado = await PacienteService.GetEstadoPacienteAsync(documentoBusqueda);
            pacienteDetalle = await PacienteService.BuscarPorDocumentoAsync(documentoBusqueda);
            pacienteEncontrado = pacienteEstado is not null && !string.IsNullOrEmpty(pacienteEstado.Documento);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al buscar paciente: {ex.Message}");
        }
        finally
        {
            loading = false;
            busquedaRealizada = true;
        }
    }
}