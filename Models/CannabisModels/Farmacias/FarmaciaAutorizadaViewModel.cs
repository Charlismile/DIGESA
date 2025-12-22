using DIGESA.Models.CannabisModels.Catalogos;

namespace DIGESA.Models.CannabisModels.Farmacias;

public class FarmaciaAutorizadaViewModel
{
    public int Id { get; set; }
    public string? CodigoFarmacia { get; set; }
    public string? NombreFarmacia { get; set; }
    public string? RUC { get; set; }
    public string? Direccion { get; set; }

    public int ProvinciaId { get; set; }
    public int DistritoId { get; set; }

    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Responsable { get; set; }

    public DateTime FechaAutorizacion { get; set; }
    public DateTime? FechaVencimientoAutorizacion { get; set; }

    public bool Activo { get; set; }
    public string? UsuarioRegistro { get; set; }
    public DateTime? FechaRegistro { get; set; }

    // Navegación
    public ProvinciaViewModel? Provincia { get; set; }
    public DistritoViewModel? Distrito { get; set; }
}