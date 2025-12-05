namespace DIGESA.Models.CannabisModels;

public class CarnetModel
{
    public string NumeroCarnet { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public bool EsAcompanante { get; set; }
    public string? FotoUrl { get; set; }
    public string? FirmaUrl { get; set; }
}

public class RenovacionPendienteModel
{
    public int SolicitudRenovacionId { get; set; }
    public string NumeroSolicitudRenovacion { get; set; } = string.Empty;
    public int? SolicitudOriginalId { get; set; }
    public string PacienteNombre { get; set; } = string.Empty;
    public string PacienteDocumento { get; set; } = string.Empty;
    public DateTime FechaSolicitudRenovacion { get; set; }
    public int DiasDesdeVencimiento { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime? FechaVencimientoOriginal { get; set; }
}

public class RenovacionSolicitudModel
{
    public int SolicitudOriginalId { get; set; }
    public List<int> DocumentosMedicosIds { get; set; } = new();
    public bool RequiereNuevaEvaluacionMedica { get; set; }
    public string? Comentario { get; set; }
}

public class CarnetVerificacionModel
{
    public string NumeroCarnet { get; set; } = string.Empty;
    public string? NombreCompleto { get; set; }
    public string? Documento { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public bool EsValido { get; set; }
    public string? Estado { get; set; }
    public string? Mensaje { get; set; }
    public bool EsAcompanante { get; set; }
}

public class RecordatorioConfigModel
{
    public int DiasAntesRecordatorio1 { get; set; } = 30;
    public int DiasAntesRecordatorio2 { get; set; } = 15;
    public int DiasAntesRecordatorio3 { get; set; } = 7;
    public bool EnviarRecordatorios { get; set; } = true;
    public string? AsuntoEmail { get; set; }
    public string? PlantillaEmail { get; set; }
}

public class InactivacionAutomaticaConfigModel
{
    public bool ActivarInactivacionAutomatica { get; set; } = true;
    public int DiasDespuesVencimiento { get; set; } = 0; // 0 = el mismo día
    public bool NotificarPaciente { get; set; } = true;
    public string? AsuntoEmailInactivacion { get; set; }
    public string? PlantillaEmailInactivacion { get; set; }
}

public enum TipoRecordatorio
{
    Vencimiento30Dias = 30,
    Vencimiento15Dias = 15,
    Vencimiento7Dias = 7,
    VencimientoHoy = 0,
    Vencido = -1
}