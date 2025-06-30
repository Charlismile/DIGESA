using DIGESA.Models.DTOs;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Services.Interfaces
{
    public interface IPacienteService
    {
        Task<int> CreateAsync(PacienteRegistroDTO model);
        Task<Paciente?> GetByIdAsync(int id);
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task UpdateAsync(Paciente paciente);
        Task DeleteAsync(int id);
    }
}