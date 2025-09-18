using DIGESA.Components.Pages.Public;
using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Services;
using Microsoft.AspNetCore.Components;

namespace DIGESA.Components.Pages.Admin;

public partial class Solicitudes : ComponentBase
{
    [Inject] private SolicitudService SolicitudService { get; set; } = default!;

    protected List<SolicitudModel>? SolicitudesLista { get; set; }

    protected override async Task OnInitializedAsync()
    {
        SolicitudesLista = await SolicitudService.ObtenerSolicitudesAsync();
    }
}