using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class EstadoSolicitudViewModel
{
    public int IdEstado { get; set; }
    
    [Required(ErrorMessage = "El nombre del estado es requerido")]
    [StringLength(50)]
    public string NombreEstado { get; set; }
    
    // Colores para UI
    public string ColorEstado => NombreEstado switch
    {
        "Pendiente" => "warning",
        "Aprobado" => "success",
        "Rechazado" => "danger",
        "En Revisión" => "info",
        "Vencido" => "secondary",
        _ => "light"
    };
}