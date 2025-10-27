using DIGESA.Components.Pages.Public;
using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Interfaces;
using DIGESA.Repositorios.Services;
using Microsoft.AspNetCore.Components;

namespace DIGESA.Components.Pages.Admin;

public partial class Solicitudes : ComponentBase
{
    [Inject] private ISolicitudService SolicitudService { get; set; } = default!;


    private List<SolicitudModel>? SolicitudesLista;
    private string? Mensaje;

    protected override async Task OnInitializedAsync()
    {
        SolicitudesLista = await SolicitudService.ObtenerSolicitudesPendientesORevisionAsync();
    }

    private async Task CambiarEstado(int id, string nuevoEstado)
    {
        var ok = await SolicitudService.ActualizarEstadoSolicitudAsync(id, nuevoEstado);

        if (ok)
        {
            Mensaje = $"Solicitud {id} actualizada a '{nuevoEstado}'.";
            SolicitudesLista = await SolicitudService.ObtenerSolicitudesPendientesORevisionAsync();
        }
        else
        {
            Mensaje = $"No se pudo actualizar la solicitud {id}.";
        }
    }
}