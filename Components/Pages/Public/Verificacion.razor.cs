using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DIGESA.Components.Pages.Public;

public partial class Verificacion : ComponentBase
{
    [Inject]
    public IPaciente PacienteService { get; set; }

    private string documentoBusqueda = string.Empty;
    private PacienteEstadoModel pacienteEstado = new();
    private PacienteModel pacienteDetalle = new();
    private bool busquedaRealizada = false;
    private bool pacienteEncontrado = false;
    private bool loading = false;

    private void HandleKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            BuscarPaciente();
        }
    }

    private async Task BuscarPaciente()
    {
        if (string.IsNullOrWhiteSpace(documentoBusqueda))
            return;

        loading = true;
        busquedaRealizada = false;
        pacienteEncontrado = false;
        pacienteEstado = new();
        pacienteDetalle = new();

        try
        {
            // Buscar estado de la solicitud
            pacienteEstado = await PacienteService.GetEstadoPacienteAsync(documentoBusqueda);
            
            // Buscar detalles del paciente
            pacienteDetalle = await PacienteService.BuscarPorDocumentoAsync(documentoBusqueda);
            
            pacienteEncontrado = pacienteEstado != null && !string.IsNullOrEmpty(pacienteEstado.Documento);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al buscar paciente: {ex.Message}");
            // Puedes mostrar un mensaje de error al usuario
        }
        finally
        {
            loading = false;
            busquedaRealizada = true;
        }
    }
}