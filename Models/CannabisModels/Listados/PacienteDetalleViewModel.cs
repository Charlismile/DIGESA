
namespace DIGESA.Models.CannabisModels.Listados;

    public class PacienteDetalleViewModel : PacienteListadoViewModel
    {
        // Agrega propiedades adicionales para el detalle si es necesario
        public string? DireccionCompleta { get; set; }
        public string? Email { get; set; }
        public string? MedicoTratante { get; set; }
        public string? Diagnostico { get; set; }
        public List<string>? DocumentosAdjuntos { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string? UsuarioRegistro { get; set; }
    }
