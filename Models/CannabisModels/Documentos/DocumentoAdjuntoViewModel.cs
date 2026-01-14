using Microsoft.AspNetCore.Components.Forms;

namespace DIGESA.Models.CannabisModels.Documentos;

public class DocumentoAdjuntoViewModel
{
    public int TipoDocumentoId { get; set; }
    public string NombreDocumento { get; set; }

    public IBrowserFile Archivo { get; set; }
    public bool Obligatorio { get; set; }
    
    public List<IBrowserFile> CedulaPaciente { get; set; } = new();
    public List<IBrowserFile> CertificacionMedica { get; set; } = new();
    public List<IBrowserFile> FotoPaciente { get; set; } = new();
    public List<IBrowserFile> CedulaAcompanante { get; set; } = new();
    public List<IBrowserFile> SentenciaTutor { get; set; } = new();
    public List<IBrowserFile> Antecedentes { get; set; } = new();
    public List<IBrowserFile> IdentidadMenor { get; set; } = new();
    public List<IBrowserFile> ConsentimientoPadres { get; set; } = new();
    public List<IBrowserFile> CertificadoNacimientoMenor { get; set; } = new();
    public List<IBrowserFile> FotoAcompanante { get; set; } = new();
}