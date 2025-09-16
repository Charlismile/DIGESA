using System.ComponentModel.DataAnnotations;
using BlazorBootstrap;
using DIGESA.Data;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Models.CannabisModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DIGESA.Components.Pages.Admin;

public partial class Create : ComponentBase
{
    [Inject] protected ToastService _ToastService { get; set; } = default!;
    [Parameter] public string? UserName { get; set; }

    #region Model

    private class RegisterModel
    {
        public string UserName { get; set; } = "";

        public string Id { get; set; } = "";

        [Required(ErrorMessage = "El correo es requerido.")]
        [EmailAddress(ErrorMessage = "El correo no tiene formato correcto.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "El nombre es requerido.")]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "El apellido es requerido.")]
        public string LastName { get; set; } = "";
        
        public bool IsMedico { get; set; } = false;
        public bool IsPaciente { get; set; } = false;
        public bool IsAdministrador { get; set; } = false;
        public bool IsExterno { get; set; } = false; 
        public bool IsAdminUser { get; set; } = false;
        public bool IsFromActiveDirectory { get; set; } = true;
    }

    #endregion

    #region Variables

    private RegisterModel FormData { get; set; } = new();
    private EditContext EditContext;
    private List<string> ErrorsFormMessages { get; set; } = new();
    private bool UserFound { get; set; } = false;
    private bool UpdatingUser { get; set; } = false;

    #endregion

    #region Events

    protected override async Task OnInitializedAsync()
    {
        ErrorsFormMessages = new();
        FormData = new();
        EditContext = new EditContext(FormData);

        if (!String.IsNullOrEmpty(UserName))
        {
            ApplicationUser UserData = await _UserService.GetUser(UserName);
            if (UserData != null)
            {
                UserFound = true;
                FormData.UserName = UserName;
                FormData.Id = UserData.Id;
                FormData.Email = UserData.Email ?? "";
                FormData.FirstName = UserData.FirstName ?? "";
                FormData.LastName = UserData.LastName ?? "";
                
                var IdentityUserData = await _UserManager.FindByEmailAsync(UserName);
                if (IdentityUserData != null)
                {
                    var roles = await _UserManager.GetRolesAsync(IdentityUserData);
                    FormData.IsMedico = roles.Contains("user_medico");
                    FormData.IsPaciente = roles.Contains("user_paciente");
                    FormData.IsAdministrador = roles.Contains("user_administrador");
                    FormData.IsExterno = roles.Contains("user_externo");
                }
                UpdatingUser = true;
            }
        }
    }

    private async Task ValidateEmail()
    {
        ErrorsFormMessages.Clear();

        var SearchUser = await _UserManager.FindByEmailAsync(FormData.Email);
        if (SearchUser != null)
        {
            ErrorsFormMessages.Add("El usuario ya esta registrado en el sistema.");
            return;
        }

        ResultGenericModel<ActiveDirectoryUserModel> ActiveDirectoryUserModelData =
            await _UserService.FindUserByEmail(FormData.Email);

        if (ActiveDirectoryUserModelData.Data != null)
        {
            UserFound = true;
            FormData.LastName = ActiveDirectoryUserModelData.Data.LastName;
            FormData.FirstName = ActiveDirectoryUserModelData.Data.FirstName;
            FormData.IsFromActiveDirectory = true;
        }
        else
        {
            // Usuario externo permitido
            UserFound = true;
            FormData.IsFromActiveDirectory = false;
            _ToastService.Notify(new(ToastType.Info, "", $"Usuario no encontrado en el AD", $"{DateTime.Now}", "Será registrado como usuario local."));
        }

    }
    private async Task RegisterUser()
    {
        ApplicationUser UserData = new ApplicationUser()
        {
            UserName = FormData.Email,
            Email = FormData.Email,
            FirstName = FormData.FirstName,
            LastName = FormData.LastName,
            EmailConfirmed = true,
            CreatedOn = DateTime.Now,
            IsAproved = true,
            IsFromActiveDirectory = FormData.IsFromActiveDirectory,
        };

        List<string> Roles = new List<string>();
        if (FormData.IsMedico)
        {
            Roles.Add("user_medico");
        }
        
        if (FormData.IsPaciente)
        {
            Roles.Add("user_paciente");
        }
        
        if (FormData.IsAdministrador)
        {
            Roles.Add("user_administrador");
        }
        
        if (FormData.IsExterno)
        {
            Roles.Add("user_externo");
        }

        ResultModel Resultado;
        if (UpdatingUser)
        {
            Resultado = await _UserService.UpdateUser(UserData, Roles);
        }
        else
        {
            Resultado = await _UserService.CreateUser(UserData, Roles);
        }

        _ToastService.Notify(new(Resultado.Success ? ToastType.Success : ToastType.Danger, "", $"Mensaje",
            $"{DateTime.Now}",
            Resultado.Message));

        if (Resultado.Success)
        {
            _NavigationProvider.NavigateTo("/UserRegistrados");
        }
    }

    #endregion
}