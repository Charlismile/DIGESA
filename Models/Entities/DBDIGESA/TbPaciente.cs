using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbPaciente
{
    public int Id { get; set; }

    public string? PrimerNombre { get; set; }

    public string? SegundoNombre { get; set; }

    public string? PrimerApellido { get; set; }

    public string? SegundoApellido { get; set; }

    public string? TipoDocumento { get; set; }

    public string? NumDocCedula { get; set; }

    public string? NumDocPasaporte { get; set; }

    public string? Nacionalidad { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public string? Sexo { get; set; }

    public string? InstalacionSalud { get; set; }

    public string? RegionSalud { get; set; }

    public bool? RequiereAcompanante { get; set; }

    public string? MotivoRequerimientoAcompanante { get; set; }

    public string? TipoDiscapacidad { get; set; }

    public string? TelefonoPersonal { get; set; }

    public string? TelefonoLaboral { get; set; }

    public string? CorreoElectronico { get; set; }

    public int? ProvinciaId { get; set; }

    public int? DistritoId { get; set; }

    public int? CorregimientoId { get; set; }

    public string? DireccionExacta { get; set; }

    public int? RegionId { get; set; }

    public int? InstalacionId { get; set; }

    public string? NombreInstalacion { get; set; }

    public virtual TbCorregimiento? Corregimiento { get; set; }

    public virtual TbDistrito? Distrito { get; set; }

    public virtual TbInstalacionSalud? Instalacion { get; set; }

    public virtual TbProvincia? Provincia { get; set; }

    public virtual TbRegionSalud? Region { get; set; }

    public virtual ICollection<TbAcompanantePaciente> TbAcompanantePaciente { get; set; } = new List<TbAcompanantePaciente>();

    public virtual ICollection<TbNombreProductoPaciente> TbNombreProductoPaciente { get; set; } = new List<TbNombreProductoPaciente>();

    public virtual ICollection<TbPacienteDiagnostico> TbPacienteDiagnostico { get; set; } = new List<TbPacienteDiagnostico>();

    public virtual ICollection<TbSolRegCannabis> TbSolRegCannabis { get; set; } = new List<TbSolRegCannabis>();
}
