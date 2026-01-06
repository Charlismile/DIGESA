using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class ModeloEvaluacion
{
    [Required(ErrorMessage = "El comentario es obligatorio")]
    [StringLength(500, ErrorMessage = "El comentario no puede exceder 500 caracteres")]
    public string Comentario { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe seleccionar una decisión")]
    public string Decision { get; set; } = string.Empty;

    public bool EsValido => !string.IsNullOrWhiteSpace(Comentario) && 
                            !string.IsNullOrWhiteSpace(Decision);
}