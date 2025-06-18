using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.DTOs
{
    public class PacienteRegistroDTO
    {
        // Datos del paciente
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

        // Datos del acompañante (opcional)
        public AcompananteRegistroDTO? Acompanante { get; set; } = new();

        // Datos del médico tratante
        [Required(ErrorMessage = "Nombre del médico es requerido.")]
        public string MedicoNombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Disciplina del médico es requerida.")]
        public string MedicoDisciplina { get; set; } = string.Empty;

        [Required(ErrorMessage = "Número de registro es requerido.")]
        public string MedicoRegistroIdoneidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Teléfono del médico es requerido.")]
        public string MedicoTelefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "Instalación de salud del médico es requerida.")]
        public string MedicoInstalacionSalud { get; set; } = string.Empty;

        // Tratamiento y diagnóstico
        [Required(ErrorMessage = "Diagnóstico es requerido.")]
        public string Diagnostico { get; set; } = string.Empty;

        public decimal? ConcentracionCBD { get; set; }
        public decimal? ConcentracionTHC { get; set; }
        public string? OtrosCannabinoides { get; set; }

        [Required(ErrorMessage = "Dosis es requerida.")]
        public string Dosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Frecuencia es requerida.")]
        public string FrecuenciaAdministracion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duración del tratamiento es requerida.")]
        public int DuracionTratamientoDias { get; set; }
    }
}