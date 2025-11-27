using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class PacienteComorbilidadModel
{
    public TieneComorbilidad TieneComorbilidadEnum { get; set; }
    public string? NombreDiagnostico { get; set; }
    public string? DetalleTratamiento { get; set; }
}