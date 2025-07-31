using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class DocumentoMedicoModel
{
        public int Id { get; set; }

        public int? MedicoId { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public string? Categoria { get; set; }

        [Required(ErrorMessage = "El nombre original es obligatorio.")]
        public string? NombreOriginal { get; set; }

        public string? FileNameStored { get; set; }

        public string? Url { get; set; }

        public DateTime? FechaSubidaUtc { get; set; }

        public int? Version { get; set; }

        public bool? IsActivo { get; set; }
}

