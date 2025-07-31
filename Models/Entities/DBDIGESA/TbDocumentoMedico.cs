using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbDocumentoMedico
{
    public int Id { get; set; }

    public int? MedicoId { get; set; }

    public string? Categoria { get; set; }

    public string? NombreOriginal { get; set; }

    public string? FileNameStored { get; set; }

    public string? Url { get; set; }

    public DateTime? FechaSubidaUtc { get; set; }

    public int? Version { get; set; }

    public bool? IsActivo { get; set; }

    public virtual TbMedicoPaciente? Medico { get; set; }
}
