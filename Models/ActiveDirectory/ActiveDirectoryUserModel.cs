namespace DIGESA.Models.ActiveDirectory;

public class ActiveDirectoryUserModel
{
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string MiddleName { get; set; } = "";
    public bool Enabled { get; set; } = false;
    public DateTime? LastLoginDate { get; set; }
    public string Id { get; set; } = ""; // id en AD
    public string[] Groups { get; set; } = Array.Empty<string>();
}