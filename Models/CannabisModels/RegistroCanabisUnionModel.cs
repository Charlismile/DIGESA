namespace DIGESA.Models.CannabisModels;

public class RegistroCanabisUnionModel
{
    public PacienteModel paciente { get; set; } = new();
    public AcompananteModel acompanante { get; set; } = new();
    public MedicoModel medico { get; set; } = new();
    public ProductoPacienteModel productoPaciente { get; set; } = new();
    public PacienteComorbilidadModel pacienteComorbilidad { get; set; } = new();
}