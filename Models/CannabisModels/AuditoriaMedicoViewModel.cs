using System.ComponentModel.DataAnnotations;

namespace DIGESA.Models.CannabisModels;

public class AuditoriaMedicoViewModel
{
    public int Id { get; set; }
    public int MedicoId { get; set; }
    public string Accion { get; set; }
    public string Usuario { get; set; }
    public DateTime Fecha { get; set; }
    public string Observaciones { get; set; }
    public string IpOrigen { get; set; }
}