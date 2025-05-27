using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class Paciente
{
    public int Id { get; set; }
    
    public string Nacionalidad { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string TipoDocumento { get; set; } = "Cedula";
    public string NumeroDocumento { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public string Residencia { get; set; } = string.Empty;
    public string TelefonoResidencial { get; set; } = string.Empty;
    public string TelefonoPersonal { get; set; } = string.Empty;
    public string TelefonoLaboral { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string InstalacionSalud { get; set; } = string.Empty;
    public string RegionSalud { get; set; } = string.Empty;
    public bool RequiereAcompanante { get; set; } = false;
    public Acompanante Acompanante { get; set; } = new();

    public virtual ICollection<Acompanante> Acompanantes { get; set; } = new List<Acompanante>();

    public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();

    public virtual Usuario Usuario { get; set; } = null!;
}
