using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DIGESA.Models.ActiveDirectory;
using DIGESA.Repositorios.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DIGESA.Repositorios.Services;

public class ActiveDirectoryService : IActiveDirectory
{
    private readonly HttpClient _http;
    private readonly ActiveDirectoryApiModel _cfg;
    private readonly ILogger<ActiveDirectoryService> _log;

    public ActiveDirectoryService(HttpClient http, IOptions<ActiveDirectoryApiModel> cfg, ILogger<ActiveDirectoryService> log)
    {
        _http = http;
        _cfg = cfg.Value;
        _log = log;
        if (!string.IsNullOrEmpty(_cfg.Token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _cfg.Token);
    }

    public async Task<IEnumerable<ActiveDirectoryUserModel>> SearchUsersAsync(string query, int page = 1, int pageSize = 25, CancellationToken ct = default)
    {
        var url = $"{_cfg.BaseUrl.TrimEnd('/')}/users/search?q={Uri.EscapeDataString(query)}&page={page}&pageSize={pageSize}";
        var res = await _http.GetAsync(url, ct);
        if (!res.IsSuccessStatusCode) {
            _log.LogWarning("SearchUsersAsync failed {status}", res.StatusCode);
            return Enumerable.Empty<ActiveDirectoryUserModel>();
        }
        var json = await res.Content.ReadAsStringAsync(ct);
        // Asume que devuelve { data: [ ... ] } o similar. Ajusta según la API.
        var wrapper = JsonSerializer.Deserialize<ApiListResponse<ActiveDirectoryUserModel>>(json, new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
        return wrapper?.Data ?? Enumerable.Empty<ActiveDirectoryUserModel>();
    }

    public async Task<ActiveDirectoryUserModel?> GetUserByIdAsync(string id, CancellationToken ct = default)
    {
        var res = await _http.GetAsync($"{_cfg.BaseUrl.TrimEnd('/')}/users/{id}", ct);
        if (!res.IsSuccessStatusCode) return null;
        var json = await res.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<ActiveDirectoryUserModel>(json, new JsonSerializerOptions{ PropertyNameCaseInsensitive = true });
    }

    public async Task<bool> EnableUserAsync(string id, bool enable, CancellationToken ct = default)
    {
        var body = JsonSerializer.Serialize(new { enabled = enable });
        var res = await _http.PutAsync($"{_cfg.BaseUrl.TrimEnd('/')}/users/{id}/enabled", new StringContent(body, Encoding.UTF8, "application/json"), ct);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUserAsync(string id, ActiveDirectoryUserModel updateModel, CancellationToken ct = default)
    {
        var body = JsonSerializer.Serialize(updateModel);
        var res = await _http.PutAsync($"{_cfg.BaseUrl.TrimEnd('/')}/users/{id}", new StringContent(body, Encoding.UTF8, "application/json"), ct);
        return res.IsSuccessStatusCode;
    }

    private class ApiListResponse<T>
    {
        public List<T> Data { get; set; } = new();
        public int Total { get; set; }
    }
}
