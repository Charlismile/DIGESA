using DIGESA.Models.CannabisModels;
using DIGESA.Models.Entities.DBDIGESA;

namespace DIGESA.Repositorios.Interfaces;

public interface ISolicitudService
{
    Task<int> CrearSolicitudCompletaAsync(RegistroCanabisUnionModel registro, DocumentoMedicoModel documentos);
    Task<bool> ValidarSolicitudCompletaAsync(RegistroCanabisUnionModel registro);
    Task<int?> CrearOGuardarInstalacionPersonalizadaAsync(string nombreInstalacion);
    Task<Dictionary<string, int>> ObtenerTiposDocumentoAsync();
}