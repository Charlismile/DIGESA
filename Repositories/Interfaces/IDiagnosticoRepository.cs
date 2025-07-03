using DIGESA.Models.Entities.DBDIGESA;
using System.Threading.Tasks;

public interface IDiagnosticoRepository
{
    Task<Diagnostico> GetByNombreOrCodigoAsync(string nombre, string codigoCie10);
    Task<Diagnostico> GetOrCreateAsync(Diagnostico diagnostico);
}