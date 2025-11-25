using System.Net;
using System.Net.Http.Headers;
using DIGESA.Data;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Models.CannabisModels;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Repositorios.Services;

public class UserDataService : IUserData
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly HttpClient _httpClient;
    private readonly ActiveDirectoryApiModel _adModel;
    private readonly string _fakePassword;

    public UserDataService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _userManager = userManager;
        _context = context;
        _httpClient = httpClient;

        _fakePassword = configuration["FakePass"] ?? "";
        _adModel = new ActiveDirectoryApiModel
        {
            BaseUrl = configuration["API_INFO:URL"] ?? "",
            Token = configuration["API_INFO:Token"] ?? ""
        };

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _adModel.Token);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    // Obtiene un usuario por email
    public async Task<ApplicationUser> GetUser(string UserName)
    {
        var user = await _userManager.FindByEmailAsync(UserName);
        if (user == null) return null!;

        return new ApplicationUser
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserName = user.UserName,
            CreatedOn = user.CreatedOn,
            LastLoginDate = user.LastLoginDate,
            IsAproved = user.IsAproved
        };
    }

    // Obtiene todos los usuarios filtrando por nombre, apellido o email
    public async Task<List<ApplicationUser>> GetAllUsers(string Filter)
    {
        var query = _context.Users.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(Filter))
        {
            var lowerFilter = Filter.ToLower();
            query = query.Where(u =>
                (u.FirstName ?? "").ToLower().Contains(lowerFilter) ||
                (u.LastName ?? "").ToLower().Contains(lowerFilter) ||
                (u.Email ?? "").ToLower().Contains(lowerFilter));
        }

        return await query.Select(u => new ApplicationUser
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Email,
            UserName = u.UserName,
            CreatedOn = u.CreatedOn,
            LastLoginDate = u.LastLoginDate,
            IsAproved = u.IsAproved
        }).ToListAsync();
    }

    // Crear usuario
    public async Task<ResultModel> CreateUser(ApplicationUser UserData, List<string> Roles)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(UserData.Email);
            if (existingUser != null)
                return new ResultModel { Success = false, Message = "El usuario ya existe" };

            var result = await _userManager.CreateAsync(UserData, _fakePassword);
            if (!result.Succeeded)
                return new ResultModel { Success = false, Message = "Error creando el usuario" };

            foreach (var role in Roles)
            {
                var roleResult = await _userManager.AddToRoleAsync(UserData, role);
                if (!roleResult.Succeeded)
                    return new ResultModel
                    {
                        Success = false,
                        Message = $"Error agregando el usuario al rol: {role}"
                    };
            }

            return new ResultModel { Success = true, Message = "El usuario fue creado correctamente." };
        }
        catch (Exception ex)
        {
            return new ResultModel { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    // Actualizar usuario
    public async Task<ResultModel> UpdateUser(ApplicationUser UserData, List<string> Roles)
    {
        try
        {
            var identityUser = await _userManager.FindByEmailAsync(UserData.Email);
            if (identityUser == null)
                return new ResultModel { Success = false, Message = "Usuario no encontrado." };

            identityUser.FirstName = UserData.FirstName;
            identityUser.LastName = UserData.LastName;
            identityUser.IsAproved = UserData.IsAproved;
            await _userManager.UpdateAsync(identityUser);

            var currentRoles = await _userManager.GetRolesAsync(identityUser);
            await _userManager.RemoveFromRolesAsync(identityUser, currentRoles);

            foreach (var role in Roles)
            {
                var addRoleResult = await _userManager.AddToRoleAsync(identityUser, role);
                if (!addRoleResult.Succeeded)
                    return new ResultModel
                    {
                        Success = false,
                        Message = $"Error agregando el usuario al rol: {role}"
                    };
            }

            return new ResultModel { Success = true, Message = "El usuario fue actualizado correctamente." };
        }
        catch (Exception ex)
        {
            return new ResultModel { Success = false, Message = $"Error: {ex.Message}" };
        }
    }

    // Login Active Directory
    public async Task<ResultModel> LoginAD(string UserName, string Password)
    {
        try
        {
            var EncodedUserName = WebUtility.UrlEncode(UserName);
            var EncodedPassword = WebUtility.UrlEncode(Password);
            var LoginUrl = $"{_adModel.BaseUrl}login/{EncodedUserName}/{EncodedPassword}";
            var Result = await _httpClient.GetFromJsonAsync<string>(LoginUrl) ?? "";
            return new ResultModel { Success = true, Message = Result };
        }
        catch (HttpRequestException ex)
        {
            return new ResultModel
            {
                Success = false,
                Message = ex.StatusCode == HttpStatusCode.Unauthorized ? "Error: Acceso denegado." : ex.Message
            };
        }
    }

    // Buscar usuario en AD por email
    public async Task<ResultModel<ActiveDirectoryUserModel>> FindUserByEmail(string Email)
    {
        var resultUserModel = new ResultModel<ActiveDirectoryUserModel>();
        try
        {
            var EncodedEmail = WebUtility.UrlEncode(Email);
            var FindUrl = $"{_adModel.BaseUrl}findbyemail/{EncodedEmail}";
            var data = await _httpClient.GetFromJsonAsync<ActiveDirectoryUserModel>(FindUrl);

            if (data == null)
            {
                resultUserModel.Success = false;
                resultUserModel.Message = "No se encontró el usuario.";
                resultUserModel.Data = null;
            }
            else
            {
                resultUserModel.Success = true;
                resultUserModel.Message = "Se encontró el usuario.";
                resultUserModel.Data = data;
            }
        }
        catch (HttpRequestException ex)
        {
            resultUserModel.Success = false;
            resultUserModel.Message =
                ex.StatusCode == HttpStatusCode.Unauthorized ? "Error: Acceso denegado." : ex.Message;
            resultUserModel.Data = null;
        }

        return resultUserModel;
    }
}
