using DIGESA.Models.CannabisModels;

namespace DIGESA.Repositorios.Interfaces;

public interface ICommon
{
    Task<string> GetFakePassword();
    
    #region Dropdowns

    Task<List<ListSustModel>> GetInstalacionesSalud(int regionId);
    Task<List<ListSustModel>> GetRegionesSalud();
    Task<List<ListSustModel>> GetProvincias();
    Task<List<ListSustModel>>  GetDistritos(int provinciaId);
    Task<List<ListSustModel>> GetCorregimientos(int distritoId);
    #endregion
}