using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DIGESA.Components.Pages.Public;

public partial class Verificacion : ComponentBase
{
    [Inject]
    public IPaciente PacienteService { get; set; }

    private string documentoBusqueda = string.Empty;
    private PacienteEstadoModel pacienteEstado = new();
    private bool busquedaRealizada = false;

    private async Task BuscarPaciente()
    {
        pacienteEstado = await PacienteService.GetEstadoPacienteAsync(documentoBusqueda);
        busquedaRealizada = true;
    }
}