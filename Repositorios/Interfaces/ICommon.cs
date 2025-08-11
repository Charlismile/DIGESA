using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositorios.Interfaces;

public interface ICommon
{
    Task<List<ListModel>> GetInstalaciones(string filtro);
    Task<List<ListModel>> GetRegiones();
    Task<List<ListModel>> GetProvincias();
    Task<List<ListModel>> GetDistritos(int ProvinciaId);
    Task<List<ListModel>> GetCorregimientos(int DistritoId);
    Task<List<ListaDiagnostico>> GetAllDiagnosticsAsync();
    Task<List<TbFormaFarmaceutica>> GetAllFormasAsync();
    Task<List<TbViaAdministracion>> GetAllViaAdmAsync();
}