using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositorios.Interfaces;

public interface ICommon
{
    // Datos geográficos
    Task<List<ItemListModel>> GetRegiones();
    Task<List<ItemListModel>> GetProvincias();
    Task<List<ItemListModel>> GetDistritos(int provinciaId);
    Task<List<ItemListModel>> GetCorregimientos(int distritoId);
    
    // Datos de salud
    Task<List<ItemListModel>> GetInstalaciones(string? filtro = null);
    Task<List<ItemListModel>> GetAllDiagnosticosAsync();
    Task<List<ItemListModel>> GetAllFormasFarmaceuticasAsync();
    Task<List<ItemListModel>> GetAllViasAdministracionAsync();
    Task<List<ItemListModel>> GetAllUnidadesAsync();
    
    // Catálogos misceláneos
    Task<List<ItemListModel>> GetTiposDocumentoAdjuntoAsync();
    Task<List<ItemListModel>> GetEstadosSolicitudAsync();
    
    // Utilidades
    Task<string> GetFakePassword();
    Task<int?> CrearInstalacionPersonalizadaAsync(string nombre);
}