using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbNombreProductoPaciente
{
    [Key]
    public int Id { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? NombreProducto { get; set; }

    public int? PacienteId { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string? FormaFarmaceutica { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? CantidadConcentracion { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? NombreConcentracion { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? ViaConsumoProducto { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? DetDosisPaciente { get; set; }

    public bool UsaDosisRescate { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? ProductoUnidad { get; set; }

    public int? DosisDuracion { get; set; }

    public int? DosisFrecuencia { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string? NombreComercialProd { get; set; }

    [StringLength(300)]
    [Unicode(false)]
    public string? DetDosisRescate { get; set; }

    public int? ProductoUnidadId { get; set; }

    public int? FormaFarmaceuticaId { get; set; }

    public int? ViaAdministracionId { get; set; }

    [ForeignKey("FormaFarmaceuticaId")]
    [InverseProperty("TbNombreProductoPaciente")]
    public virtual TbFormaFarmaceutica? FormaFarmaceuticaNavigation { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("TbNombreProductoPaciente")]
    public virtual TbPaciente? Paciente { get; set; }

    [ForeignKey("ProductoUnidadId")]
    [InverseProperty("TbNombreProductoPaciente")]
    public virtual TbUnidades? ProductoUnidadNavigation { get; set; }

    [ForeignKey("ViaAdministracionId")]
    [InverseProperty("TbNombreProductoPaciente")]
    public virtual TbViaAdministracion? ViaAdministracion { get; set; }
}
