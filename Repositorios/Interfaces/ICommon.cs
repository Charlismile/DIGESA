using DIGESA.DTOs;

namespace DIGESA.Repositorios.Interfaces;

public interface ICommon
{
    Task<string> GetFakePassword();
    
    #region Dropdowns
    Task<List<UbicacionDto>> ObtenerRegionesAsync();

    Task<List<ListDto>> GetProvincias();
    Task<List<ListDto>> GetDistritosPorProvincia(int provinciaId);
    Task<List<ListDto>> GetCorregimientos(int distritoId);
    #endregion
}