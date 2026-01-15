using BlazorBootstrap;
using DIGESA.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace DIGESA.Components.Pages.Admin;

public partial class UserRegistrados : ComponentBase
{
    private Grid<ApplicationUser> GridUsers = default!;
    private List<ApplicationUser> ListUsers = new();
    private string Filter { get; set; } = "";

    private async Task<GridDataProviderResult<ApplicationUser>> UsersDataProvider(GridDataProviderRequest<ApplicationUser> request)
    {
        ListUsers = await _UserService.GetAllUsers(Filter);
        int total = ListUsers.Count;
        var page = ListUsers.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

        return new GridDataProviderResult<ApplicationUser>
        {
            Data = page,
            TotalCount = total
        };
    }

    private async Task ReloadGrid()
    {
        Filter = Filter.Trim();
        await GridUsers.RefreshDataAsync();
    }
    private async Task ExportarUsuariosExcel()
    {
        var usuarios = await _UserService.GetAllUsers(Filter);

        if (!usuarios.Any())
            return;

        var bytes = await ExcelService.ExportUsuariosAsync(usuarios);
        var base64 = Convert.ToBase64String(bytes);

        await JS.InvokeVoidAsync(
            "downloadFileFromBytes",
            $"Usuarios_{DateTime.Now:yyyyMMddHHmm}.xlsx",
            base64);
    }
}