using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class TbMedico
{
    public int Id { get; set; }

    public string CodigoMedico { get; set; } = null!;

    public string PrimerNombre { get; set; } = null!;

    public string? SegundoNombre { get; set; }

    public string PrimerApellido { get; set; } = null!;

    public string? SegundoApellido { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public string NumeroDocumento { get; set; } = null!;

    public string Especialidad { get; set; } = null!;

    public string? Subespecialidad { get; set; }

    public string NumeroColegiatura { get; set; } = null!;

    public string? TelefonoConsultorio { get; set; }

    public string? TelefonoMovil { get; set; }

    public string? Email { get; set; }

    public string? DireccionConsultorio { get; set; }

    public int? ProvinciaId { get; set; }

    public int? DistritoId { get; set; }

    public int? RegionSaludId { get; set; }

    public int? InstalacionSaludId { get; set; }

    public string? InstalacionPersonalizada { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? UsuarioRegistro { get; set; }

    public bool Activo { get; set; }

    public bool Verificado { get; set; }

    public DateTime? FechaVerificacion { get; set; }

    public string? UsuarioVerificador { get; set; }

    public string? Observaciones { get; set; }

    public virtual TbDistrito? Distrito { get; set; }

    public virtual TbInstalacionSalud? InstalacionSalud { get; set; }

    public virtual TbProvincia? Provincia { get; set; }

    public virtual TbRegionSalud? RegionSalud { get; set; }

    public virtual ICollection<TbAuditoriaMedico> TbAuditoriaMedico { get; set; } = new List<TbAuditoriaMedico>();

    public virtual ICollection<TbMedicoPaciente> TbMedicoPaciente { get; set; } = new List<TbMedicoPaciente>();
}
