using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.DTOs
{
    public class AcompananteRegistroDTO
    {
        public bool EsRequerido => !string.IsNullOrEmpty(Parentesco);

        public string NombreCompleto { get; set; } = "";
        public string TipoDocumento { get; set; } = "";
        public string NumeroDocumento { get; set; } = "";
        public string Nacionalidad { get; set; } = "";
        public string Parentesco { get; set; } = "";
    }
}