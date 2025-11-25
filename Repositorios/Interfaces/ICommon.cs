using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositorios.Interfaces;

public interface ICommon
{
    Task<List<ItemListModel>> GetInstalaciones(string filtro);
    Task<List<ItemListModel>> GetRegiones();
    Task<List<ItemListModel>> GetProvincias();
    Task<List<ItemListModel>> GetDistritos(int ProvinciaId);
    Task<List<ItemListModel>> GetCorregimientos(int DistritoId);
    Task<List<ListaDiagnostico>> GetAllDiagnosticsAsync();
    Task<List<TbFormaFarmaceutica>> GetAllFormasAsync();
    Task<List<TbViaAdministracion>> GetAllViaAdmAsync();
    Task<List<ItemListModel>> GetUnidadId();
    Task<string> GetFakePassword();
    Task<List<ItemListModel>> GetAllUnidadesAsync();
}