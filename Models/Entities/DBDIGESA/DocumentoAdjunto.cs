using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class DocumentoAdjunto
{
    public int DocumentoAdjuntoId { get; set; }

    public int DocumentoAdjuntoSolicitudId { get; set; }

    public string DocumentoAdjuntoNombreArchivo { get; set; } = null!;

    public string DocumentoAdjuntoTipoDocumento { get; set; } = null!;

    public string? DocumentoAdjuntoTipoContenidoMime { get; set; }

    public string DocumentoAdjuntoRutaAlmacenamiento { get; set; } = null!;

    public DateTime? DocumentoAdjuntoFechaSubida { get; set; }

    public string? DocumentoAdjuntoSubidoPorUsuarioId { get; set; }

    public string? DocumentoAdjuntoDescripcion { get; set; }

    public virtual Solicitud DocumentoAdjuntoSolicitud { get; set; } = null!;
}
