using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DIGESA.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string email, string password, string role)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Asignar rol
        await _userManager.AddToRoleAsync(user, role);

        return Ok(new { Message = "Usuario creado correctamente." });
    }
}