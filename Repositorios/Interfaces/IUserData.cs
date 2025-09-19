using DIGESA.Data;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface IUserData
{
    Task<ApplicationUser> GetUser(string UserName);
    Task<List<ApplicationUser>> GetAllUsers(string Filter);
    Task<ResultModel> CreateUser(ApplicationUser UserData, List<string> Roles);
    Task<ResultModel> UpdateUser(ApplicationUser UserData, List<string> Roles);

    // Active Directory
    Task<ResultModel> LoginAD(string UserName, string Password);
    Task<ResultGenericModel<ActiveDirectoryUserModel>> FindUserByEmail(string Email);
}