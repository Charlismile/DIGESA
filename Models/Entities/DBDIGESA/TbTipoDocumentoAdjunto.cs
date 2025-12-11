using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbTipoDocumentoAdjunto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool? EsRequisitoParaPaciente { get; set; }

    public bool? EsRequisitoParaAcompanante { get; set; }

    public bool? EsParaMenorEdad { get; set; }

    public bool? EsParaMayorEdad { get; set; }

    public bool? EsParaRenovacion { get; set; }

    public bool? EsObligatorio { get; set; }

    public bool? EsSoloMedico { get; set; }

    public bool? IsActivo { get; set; }

    public virtual ICollection<TbDocumentoAdjunto> TbDocumentoAdjunto { get; set; } = new List<TbDocumentoAdjunto>();
}
