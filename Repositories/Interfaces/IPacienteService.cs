using DIGESA.Models.DTOs;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Services.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<int> CreateAsync(PacienteRegistroDTO model);
    }
}