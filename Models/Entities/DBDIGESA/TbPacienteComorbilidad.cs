using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPacienteComorbilidad
{
    [Key]
    public int Id { get; set; }

    public int PacienteId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string NombreDiagnostico { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? DetalleTratamiento { get; set; }

    public DateOnly? FechaDiagnostico { get; set; }

    public bool TieneComorbilidad { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("TbPacienteComorbilidad")]
    public virtual TbPaciente Paciente { get; set; } = null!;
}
