namespace DIGESA.Models.CannabisModels;

public class RegistroCannabisUnionModel
{
    public PacienteModel Paciente { get; set; } = new();
    public AcompananteModel? Acompanante { get; set; }
    public MedicoModel Medico { get; set; } = new();
    public ProductoPacienteModel Producto { get; set; } = new();
    public PacienteDiagnosticoModel Diagnostico { get; set; } = new();
}