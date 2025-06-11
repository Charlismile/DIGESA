using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.DTOs
{
    public class PacienteRegistroDTO
    {
        [Required(ErrorMessage = "Nombre completo es requerido.")]
        public string NombreCompleto { get; set; } = "";

        [Required(ErrorMessage = "Tipo de documento es requerido.")]
        public string TipoDocumento { get; set; } = ""; // Cedula / Pasaporte

        [Required(ErrorMessage = "Número de documento es requerido.")]
        public string NumeroDocumento { get; set; } = "";

        [Required(ErrorMessage = "Nacionalidad es requerida.")]
        public string Nacionalidad { get; set; } = "";

        [Required(ErrorMessage = "Fecha de nacimiento es requerida.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Sexo es requerido.")]
        public string Sexo { get; set; } = ""; // Femenino / Masculino

        [Required(ErrorMessage = "Dirección residencia es requerida.")]
        public string DireccionResidencia { get; set; } = "";

        public string? TelefonoResidencial { get; set; }
        public string? TelefonoPersonal { get; set; }
        public string? TelefonoLaboral { get; set; }
        public string? CorreoElectronico { get; set; }

        [Required(ErrorMessage = "Instalación de salud es requerida.")]
        public string InstalacionSalud { get; set; } = "";

        [Required(ErrorMessage = "Región de salud es requerida.")]
        public string RegionSalud { get; set; } = "";

        public bool RequiereAcompanante { get; set; }

        public string? MotivoRequerimientoAcompanante { get; set; } // Menor de edad / Discapacidad
        public string? TipoDiscapacidad { get; set; }

        public AcompananteRegistroDTO Acompanante { get; set; } = new();

        // Datos del médico tratante
        public string NombreMedico { get; set; } = "";
        public string DisciplinaMedico { get; set; } = "";
        public string NumeroRegistroIdoneidad { get; set; } = "";
        public string TelefonoMedico { get; set; } = "";
        public string InstalacionSaludMedico { get; set; } = "";

        // Tratamiento con cannabis medicinal
        public List<string> DiagnosticosSeleccionados { get; set; } = new();
        
        public List<string> CannabinoidesSeleccionados { get; set; } = new();
        public string OtroDiagnostico { get; set; } = "";
        public string ViaAdministracion { get; set; } = "";
        public string Dosis { get; set; } = "";
        public string Frecuencia { get; set; } = "";
        public string DuracionTratamiento { get; set; } = "";
    }
}