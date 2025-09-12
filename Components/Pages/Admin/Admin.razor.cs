using DIGESA.Models.ActiveDirectory;
using Microsoft.AspNetCore.Components;

namespace DIGESA.Components.Pages.Admin;

public partial class Admin : ComponentBase
{
    private string searchTerm = "";
    private List<ActiveDirectoryUserModel> users = new();
    private bool isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        await LoadDefault();
    }

    private async Task LoadDefault()
    {
        isLoading = true;
        users = (await ADService.SearchUsersAsync("", 1, 50)).ToList();
        isLoading = false;
    }

    private async Task Search()
    {
        isLoading = true;
        users = (await ADService.SearchUsersAsync(searchTerm, 1, 50)).ToList();
        isLoading = false;
    }

    private async Task ToggleEnable(ActiveDirectoryUserModel u)
    {
        var success = await ADService.EnableUserAsync(u.Id, !u.Enabled);
        if (success) u.Enabled = !u.Enabled;
    }

    private void EditUser(ActiveDirectoryUserModel u)
    {
        // abrir modal de edición — implementar UI
    }
}