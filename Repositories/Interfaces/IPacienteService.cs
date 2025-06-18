using DIGESA.Models.DTOs;
using DIGESA.Models.Entities.DBDIGESA;

public interface IPacienteService
{
    Task<IEnumerable<Paciente>> GetAllAsync();
    Task<Paciente?> GetByIdAsync(int id);
    Task<int> CreateAsync(PacienteRegistroDTO model);
    Task UpdateAsync(Paciente paciente);
    Task DeleteAsync(int id);
}