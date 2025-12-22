using Microsoft.AspNetCore.Components.Forms;

namespace DIGESA.Models.CannabisModels.Documentos;

public class DocumentoAdjuntoViewModel
{
    public int TipoDocumentoId { get; set; }
    public string NombreDocumento { get; set; }

    public IBrowserFile Archivo { get; set; }
    public bool Obligatorio { get; set; }
}