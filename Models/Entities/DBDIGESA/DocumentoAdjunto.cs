using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DBDIGESA;

public partial class DocumentoAdjunto
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string TipoDocumento { get; set; } = null!;

    public string? TipoContenidoMime { get; set; }

    public string RutaAlmacenamiento { get; set; } = null!;

    public DateTime? FechaSubida { get; set; }

    public int? SubidoPorUsuarioId { get; set; }

    public string? Descripcion { get; set; }

    public virtual Solicitud Solicitud { get; set; } = null!;

    public virtual Usuario? SubidoPorUsuario { get; set; }
}
