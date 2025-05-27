namespace DIGESA.Models.Entities.DIGESA;

public class AuthState
{
    public bool IsAuthenticated { get; set; }
    public string? Role { get; set; }
    public string? Email { get; set; }
}