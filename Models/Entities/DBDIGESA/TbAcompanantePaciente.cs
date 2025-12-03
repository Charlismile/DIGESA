using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbAcompanantePaciente
{
    [Key]
    public int Id { get; set; }

    public int? PacienteId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PrimerNombre { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? SegundoNombre { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PrimerApellido { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? SegundoApellido { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? TipoDocumento { get; set; }

    [StringLength(100)]
    public string? NumeroDocumento { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Nacionalidad { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Parentesco { get; set; }

    [StringLength(15)]
    public string? TelefonoMovil { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("TbAcompanantePaciente")]
    public virtual TbPaciente? Paciente { get; set; }
}
