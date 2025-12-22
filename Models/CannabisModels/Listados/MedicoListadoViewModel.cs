namespace DIGESA.Models.CannabisModels.Listados;

public class MedicoListadoViewModel
{
    public int Id { get; set; }
    public string CodigoMedico { get; set; }
    public string NombreCompleto { get; set; }
    public string Especialidad { get; set; }
    public string Provincia { get; set; }

    public bool Activo { get; set; }
    public bool Verificado { get; set; }
}