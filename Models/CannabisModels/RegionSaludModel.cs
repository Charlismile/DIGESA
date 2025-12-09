using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class RegionSaludModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(10)]
    public string? Codigo { get; set; }
    
    public bool IsActivo { get; set; } = true;
    public string? DirectorRegional { get; set; }
    public string? TelefonoContacto { get; set; }
    public string? EmailContacto { get; set; }
    
    // Regiones predefinidas (ajusta según tu país)
    public static List<RegionSaludModel> RegionesPredefinidas()
    {
        return new List<RegionSaludModel>
        {
            new() { Id = 1, Nombre = "Región de Salud de Panamá Metro", Codigo = "RS01" },
            new() { Id = 2, Nombre = "Región de Salud de Panamá Oeste", Codigo = "RS02" },
            new() { Id = 3, Nombre = "Región de Salud de San Miguelito", Codigo = "RS03" },
            new() { Id = 4, Nombre = "Región de Salud de Colón", Codigo = "RS04" },
            new() { Id = 5, Nombre = "Región de Salud de Chiriquí", Codigo = "RS05" },
            new() { Id = 6, Nombre = "Región de Salud de Veraguas", Codigo = "RS06" },
            new() { Id = 7, Nombre = "Región de Salud de Bocas del Toro", Codigo = "RS07" },
            new() { Id = 8, Nombre = "Región de Salud de Coclé", Codigo = "RS08" },
            new() { Id = 9, Nombre = "Región de Salud de Herrera", Codigo = "RS09" },
            new() { Id = 10, Nombre = "Región de Salud de Los Santos", Codigo = "RS10" },
            new() { Id = 11, Nombre = "Región de Salud de Darién", Codigo = "RS11" },
            new() { Id = 12, Nombre = "Región de Salud de Guna Yala", Codigo = "RS12" },
            new() { Id = 13, Nombre = "Región de Salud de Ngäbe-Buglé", Codigo = "RS13" },
            new() { Id = 14, Nombre = "Región de Salud de Emberá-Wounaan", Codigo = "RS14" }
        };
    }
}