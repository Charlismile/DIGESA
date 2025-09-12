using DIGESA.Models.ActiveDirectory;

namespace DIGESA.Repositorios.Interfaces;

public interface IActiveDirectory
{
    Task<IEnumerable<ActiveDirectoryUserModel>> SearchUsersAsync(string query, int page = 1, int pageSize = 25, CancellationToken ct = default);
    Task<ActiveDirectoryUserModel?> GetUserByIdAsync(string id, CancellationToken ct = default);
    Task<bool> EnableUserAsync(string id, bool enable, CancellationToken ct = default);
    Task<bool> UpdateUserAsync(string id, ActiveDirectoryUserModel updateModel, CancellationToken ct = default);
}