using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbMedicoPaciente
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PrimerNombre { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? PrimerApellido { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? MedicoDisciplina { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? MedicoIdoneidad { get; set; }

    [StringLength(15)]
    public string? MedicoTelefono { get; set; }

    public int? RegionId { get; set; }

    public int? InstalacionId { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string DetalleMedico { get; set; } = null!;

    public int? PacienteId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? InstalacionPersonalizada { get; set; }

    [ForeignKey("InstalacionId")]
    [InverseProperty("TbMedicoPaciente")]
    public virtual TbInstalacionSalud? Instalacion { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("TbMedicoPaciente")]
    public virtual TbPaciente? Paciente { get; set; }

    [ForeignKey("RegionId")]
    [InverseProperty("TbMedicoPaciente")]
    public virtual TbRegionSalud? Region { get; set; }
}
