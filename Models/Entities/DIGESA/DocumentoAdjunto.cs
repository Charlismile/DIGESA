using System;
using System.Collections.Generic;

namespace DIGESA.Models.Entities.DIGESA;

public partial class DocumentoAdjunto
{
    public int Id { get; set; }

    public int SolicitudId { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string? TipoContenido { get; set; }

    public string? RutaAlmacenamiento { get; set; }

    public DateTime? FechaSubida { get; set; }

    public virtual Solicitud Solicitud { get; set; } = null!;
}
