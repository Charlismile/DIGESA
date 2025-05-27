using System.Security.Claims;
using DIGESA.Models.Entities.DIGESA;

namespace DIGESA.Services;

public class AuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthState> GetAuthState()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated == true)
        {
            var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
            return new AuthState
            {
                IsAuthenticated = true,
                Role = roleClaim,
                Email = user.FindFirst(ClaimTypes.Email)?.Value
            };
        }

        return new AuthState { IsAuthenticated = false };
    }
}