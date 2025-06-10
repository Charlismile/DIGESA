using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DIGESA.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> CreateRoles()
    {
        string[] roleNames = { "Administrador", "Médico", "Paciente" };

        foreach (var roleName in roleNames)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        return Ok(new { Message = "Roles creados correctamente." });
    }
}