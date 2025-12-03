using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbTipoDocumentoAdjunto
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    public bool? EsRequisitoParaPaciente { get; set; }

    public bool? EsRequisitoParaAcompanante { get; set; }

    public bool? EsParaMenorEdad { get; set; }

    public bool? EsParaMayorEdad { get; set; }

    public bool? EsParaRenovacion { get; set; }

    public bool? EsObligatorio { get; set; }

    public bool? EsSoloMedico { get; set; }

    public bool? IsActivo { get; set; }

    [InverseProperty("TipoDocumento")]
    public virtual ICollection<TbDocumentoAdjunto> TbDocumentoAdjunto { get; set; } = new List<TbDocumentoAdjunto>();
}
