using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPacienteDiagnostico
{
    [Key]
    public int Id { get; set; }

    public int? PacienteId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? NombreDiagnostico { get; set; }

    public int? DiagnosticoId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string? Tipo { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? DetalleTratamiento { get; set; }

    public DateOnly? FechaDiagnostico { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("TbPacienteDiagnostico")]
    public virtual TbPaciente? Paciente { get; set; }
}
