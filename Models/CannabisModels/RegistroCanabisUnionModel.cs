namespace DIGESA.Models.CannabisModels;

public class RegistroCannabisUnionModel
{
    public PacienteModel Paciente { get; set; } = new();
    public AcompananteModel? Acompanante { get; set; }
    public MedicoModel Medico { get; set; } = new();
    public ProductoModel Producto { get; set; } = new(); 
    public DiagnosticoModel Diagnostico { get; set; } = new();
    public PacienteComorbilidadModel Comorbilidad { get; set; } = new(); 
}