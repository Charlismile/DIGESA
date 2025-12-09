using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class EstadoSolicitudModel
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(200)]
    public string? Descripcion { get; set; }
    
    public bool EsEstadoFinal { get; set; } = false;
    public bool EsEstadoActivo { get; set; } = true;
    public int OrdenFlujo { get; set; } = 0;
    
    // Estados predefinidos (según tu tabla)
    public static List<EstadoSolicitudModel> EstadosPredefinidos()
    {
        return new List<EstadoSolicitudModel>
        {
            new() { Id = 1, Nombre = "Pendiente", Descripcion = "Solicitud ingresada, pendiente de revisión", OrdenFlujo = 1 },
            new() { Id = 2, Nombre = "En Revisión", Descripcion = "Solicitud en proceso de revisión", OrdenFlujo = 2 },
            new() { Id = 3, Nombre = "Documentación Incompleta", Descripcion = "Faltan documentos por subir", OrdenFlujo = 3 },
            new() { Id = 4, Nombre = "Aprobada", Descripcion = "Solicitud aprobada", EsEstadoFinal = true, OrdenFlujo = 4 },
            new() { Id = 5, Nombre = "Rechazada", Descripcion = "Solicitud rechazada", EsEstadoFinal = true, OrdenFlujo = 5 },
            new() { Id = 6, Nombre = "Cancelada", Descripcion = "Solicitud cancelada por el usuario", EsEstadoFinal = true, OrdenFlujo = 6 }
        };
    }
}