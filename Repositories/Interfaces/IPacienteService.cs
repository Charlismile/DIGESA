using DIGESA.Models.Entities.DBDIGESA;

public interface IPacienteService
{
    Task<int> CreateAsync(PacienteRegistroDTO model);
    Task<List<Paciente>> GetAllAsync(); // Nuevo método
}