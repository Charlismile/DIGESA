
using System.Threading.Tasks;

public interface IPacienteService
{
    Task<int> CreateAsync(PacienteRegistroDTO model);
}