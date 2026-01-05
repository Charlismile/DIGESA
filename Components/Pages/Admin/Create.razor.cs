using System.ComponentModel.DataAnnotations;
using BlazorBootstrap;
using DIGESA.Data;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Models.CannabisModels;
using DIGESA.Models.CannabisModels.Common;
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
        public bool IsFromActiveDirectory { get; set; } = true;
    }

    #endregion

    #region Variables

    private RegisterModel FormData { get; set; } = new();
    private EditContext EditContext;
    private List<string> ErrorsFormMessages { get; set; } = new();
    private bool UserFound { get; set; } = false;
    private bool UpdatingUser { get; set; } = false;
    
    private DynamicModal ModalForm = default!;

    #endregion

    #region Events

    protected override async Task OnInitializedAsync()
    {
        ErrorsFormMessages = new();
        FormData = new();
        EditContext = new EditContext(FormData);

        if (!string.IsNullOrEmpty(UserName))
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
                    // FormData.IsMedico = roles.Contains("Medico");
                    // FormData.IsPaciente = roles.Contains("Paciente");
                    FormData.IsAdministrador = roles.Contains("Administrador");
                    // FormData.IsExterno = roles.Contains("Externo");
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
            ModalForm.ShowError("El usuario ya está registrado en el sistema.");
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

            ModalForm.ShowInfo("El usuario no se encontró en el directorio activo. Se procederá a registrarlo como usuario externo.");
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

        List<string> Roles = new();
        // if (FormData.IsMedico)
        // {
        //     Roles.Add("Medico");
        // }
        //
        // if (FormData.IsPaciente)
        // {
        //     Roles.Add("Paciente");
        // }
        
        if (FormData.IsAdministrador)
        {
            Roles.Add("Administrador");
        }
        
        // if (FormData.IsExterno)
        // {
        //     Roles.Add("Externo");
        // }g

        ResultModel Resultado;
        if (UpdatingUser)
        {
            Resultado = await _UserService.UpdateUser(UserData, Roles);
        }
        else
        {
            Resultado = await _UserService.CreateUser(UserData, Roles);
        }

        ModalForm.ShowSuccess("Registro con el usuario exitoso.");

        if (Resultado.Success)
        {
            _NavigationProvider.NavigateTo("/Admin/UserRegistrados");
        }
    }

    #endregion
}
