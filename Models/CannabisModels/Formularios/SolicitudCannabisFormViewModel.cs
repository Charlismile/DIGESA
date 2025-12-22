using DIGESA.Models.CannabisModels.Documentos;

namespace DIGESA.Models.CannabisModels.Formularios;

public class SolicitudCannabisFormViewModel
{
    public DatosPacienteVM Paciente { get; set; } = new();
    public DatosMedicosVM Medico { get; set; } = new();
    public DatosProductoVM Producto { get; set; } = new();
    public DatosAcompananteVM Acompanante { get; set; }
    public DeclaracionJuradaViewModel Declaracion { get; set; } = new();
    public List<DocumentoAdjuntoViewModel> Documentos { get; set; } = new();

    // Control de flujo
    public bool EsRenovacion { get; set; }
    public int? SolicitudAnteriorId { get; set; }
}
