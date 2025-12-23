using DIGESA.Models.CannabisModels.Common;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositorios.Interfaces;

public interface ICommon
{
    Task<string> GetFakePassword();

    Task<List<ListSustModel>> GetProvincias();
    Task<List<ListSustModel>> GetDistritos(int provinciaId);
    Task<List<ListSustModel>> GetCorregimientos(int distritoId);
    Task<List<ListSustModel>> GetRegionesSalud();
    Task<List<ListSustModel>> GetInstalacionesSalud(int regionId = 0);
    Task<List<ListaDiagnostico>> GetAllDiagnosticsAsync();
    Task<List<TbFormaFarmaceutica>> GetAllFormasAsync();
    Task<List<TbViaAdministracion>> GetAllViaAdmAsync();
    Task<List<ListSustModel>> GetUnidadId();
}