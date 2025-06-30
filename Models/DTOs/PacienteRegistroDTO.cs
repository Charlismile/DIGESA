using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.DTOs
{
    public class PacienteRegistroDTO
    {
        [Required(ErrorMessage = "Nombre completo es requerido.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipo de documento es requerido.")]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número de documento es requerido.")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nacionalidad es requerida.")]
        public string Nacionalidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fecha de nacimiento es requerida.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Sexo es requerido.")]
        public string Sexo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Dirección es requerida.")]
        public string DireccionResidencia { get; set; } = string.Empty;

        public string? TelefonoResidencial { get; set; }
        public string? TelefonoPersonal { get; set; }
        public string? TelefonoLaboral { get; set; }
        public string? CorreoElectronico { get; set; }

        [Required(ErrorMessage = "Instalación de salud es requerida.")]
        public string InstalacionSalud { get; set; } = string.Empty;

        [Required(ErrorMessage = "Región de salud es requerida.")]
        public string RegionSalud { get; set; } = string.Empty;

        public bool RequiereAcompanante { get; set; }
        public string? MotivoRequerimientoAcompanante { get; set; }
        public string? TipoDiscapacidad { get; set; }

        // Datos anidados
        public AcompananteRegistroDTO Acompanante { get; set; } = new();
    }
}