using Microsoft.AspNetCore.Identity;

namespace DIGESA.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty; // Paciente, Médico, Administrador
}