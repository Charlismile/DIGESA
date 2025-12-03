using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPaciente
{
    [Key]
    public int Id { get; set; }

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
    public string? DocumentoCedula { get; set; }

    [StringLength(100)]
    public string? DocumentoPasaporte { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? Nacionalidad { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Sexo { get; set; }

    public bool? RequiereAcompanante { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? MotivoRequerimientoAcompanante { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? TipoDiscapacidad { get; set; }

    [StringLength(15)]
    public string? TelefonoPersonal { get; set; }

    [StringLength(15)]
    public string? TelefonoLaboral { get; set; }

    [StringLength(200)]
    public string? CorreoElectronico { get; set; }

    public int? ProvinciaId { get; set; }

    public int? DistritoId { get; set; }

    public int? CorregimientoId { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? DireccionExacta { get; set; }

    public int? RegionId { get; set; }

    public int? InstalacionId { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? InstalacionPersonalizada { get; set; }

    [ForeignKey("CorregimientoId")]
    [InverseProperty("TbPaciente")]
    public virtual TbCorregimiento? Corregimiento { get; set; }

    [ForeignKey("DistritoId")]
    [InverseProperty("TbPaciente")]
    public virtual TbDistrito? Distrito { get; set; }

    [ForeignKey("InstalacionId")]
    [InverseProperty("TbPaciente")]
    public virtual TbInstalacionSalud? Instalacion { get; set; }

    [ForeignKey("ProvinciaId")]
    [InverseProperty("TbPaciente")]
    public virtual TbProvincia? Provincia { get; set; }

    [ForeignKey("RegionId")]
    [InverseProperty("TbPaciente")]
    public virtual TbRegionSalud? Region { get; set; }

    [InverseProperty("Paciente")]
    public virtual ICollection<TbAcompanantePaciente> TbAcompanantePaciente { get; set; } = new List<TbAcompanantePaciente>();

    [InverseProperty("Paciente")]
    public virtual ICollection<TbMedicoPaciente> TbMedicoPaciente { get; set; } = new List<TbMedicoPaciente>();

    [InverseProperty("Paciente")]
    public virtual ICollection<TbNombreProductoPaciente> TbNombreProductoPaciente { get; set; } = new List<TbNombreProductoPaciente>();

    [InverseProperty("Paciente")]
    public virtual ICollection<TbPacienteComorbilidad> TbPacienteComorbilidad { get; set; } = new List<TbPacienteComorbilidad>();

    [InverseProperty("Paciente")]
    public virtual ICollection<TbPacienteDiagnostico> TbPacienteDiagnostico { get; set; } = new List<TbPacienteDiagnostico>();

    [InverseProperty("Paciente")]
    public virtual ICollection<TbSolRegCannabis> TbSolRegCannabis { get; set; } = new List<TbSolRegCannabis>();
}
