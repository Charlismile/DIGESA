using DIGESA.Models.Entities.DBDIGESA;
using System.Threading.Tasks;

public interface IMedicoRepository
{
    Task AddAsync(Medico medico);
    Task<bool> ExistePorDocumento(string numeroRegistro);
}