using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class AuditoriaMedicoViewModel
{
    public int Id { get; set; }
    
    [Required]
    public int MedicoId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string TipoAccion { get; set; } // Creacion, Actualizacion, Verificacion, Inactivacion
    
    [Required]
    [StringLength(450)]
    public string UsuarioAccion { get; set; }
    
    [Required]
    public DateTime FechaAccion { get; set; }
    
    public string DetallesAnteriores { get; set; }
    public string DetallesNuevos { get; set; }
    
    [StringLength(500)]
    public string Comentarios { get; set; }
    
    [StringLength(50)]
    public string IpOrigen { get; set; }
    
    // Propiedades de navegación
    public MedicoViewModel Medico { get; set; }
    
    // Propiedades calculadas
    public string NombreMedico => Medico?.NombreCompleto;
    public string DescripcionAccion => GetDescripcionAccion();
    
    private string GetDescripcionAccion()
    {
        return TipoAccion switch
        {
            "Creacion" => "Médico registrado en el sistema",
            "Actualizacion" => "Datos del médico actualizados",
            "Verificacion" => "Médico verificado por el Ministerio",
            "Inactivacion" => "Médico inactivado en el sistema",
            _ => TipoAccion
        };
    }
}