using System.Collections.Generic;
using System.Threading.Tasks;
using DIGESA.Models.Entities.DBDIGESA;

public interface IPacienteRepository
{
    Task<Paciente> GetByIdAsync(int id);
    Task<List<Paciente>> GetAllAsync(); // Este es el método faltante
    Task AddAsync(Paciente paciente);
    Task UpdateAsync(Paciente paciente);
    Task DeleteAsync(int id);
}